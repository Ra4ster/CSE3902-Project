# Link collisions

This change builds on the `ICollidable` framework merged in PR #73. It covers Link's body, solid tiles, enemy contact, and sword attacks. Room loading/transition logic and item/projectile collision behavior are separate work.

## Behavior

- Link's body is a 16 by 16 sprite scaled by 6, offset from `Position` using the existing (8, 13) drawing origin. Attacking never enlarges the body collider.
- Square, keystone, statue, brick, void, and water tiles block Link. Speckled floor, sand, stairs, and ladder tiles are walkable. Stairs remain walkable until room-transition behavior is connected.
- Barrier contact pushes Link out along the detected collision side. Correcting position also corrects the drawn sprite. Link stays inside the logical room/window bounds, independent of window resizing.
- Every enemy inherits a collider based on its actual sprite size and scale, rather than using a Stalfos-only hard-coded box. Enemy contact removes one heart and flashes Link red/white for 0.5 seconds. Further contacts during this interval cannot remove more health or restart the timer. Health stops at zero, but collision feedback continues because this test scene has no death state. An on-screen HP and hits-taken display makes each accepted contact visible; R rebuilds Link and resets both.
- The sword has its own collider, active only during attack animation frames 1 and 2 (zero based). It extends in the facing direction and can damage each enemy once per swing. Item-use animations never activate it.
- Enemies turn red for 0.3 seconds on a sword hit. Defeated enemies stop drawing/colliding and are removed by the enemy manager. Removing the last enemy leaves drawing and cycling safe.
- Debug outlines: Link/enemy bodies are red, the selected block is green, and the active sword is yellow.

Simplifications relative to NES Zelda: sword bounds approximate the blade using rectangles; all current enemies, including the boss and BladeTrap, start with three health and accept sword damage. Special defenses, directional shields, knockback, and player death/game-over behavior are outside this change.

## Structure and room integration

`LinkCollisionHandler.Register` installs body/enemy, body/block, body/door, and sword/enemy responses. `CollisionDispatcher` reverses the collision side when it reverses object order, so handlers always receive the side relative to their first object.

`CollisionProperties.BlocksPlayer` determines whether a tile blocks movement; `IsStatic` alone does not imply that it is a barrier. Non-barrier tiles keep their colliders but do not move Link.

The current scene still exposes only the selected block and enemy. `Game.ProcessCollisions` resolves the selected barrier and room bounds first, then detects body/sword contacts at the corrected position. This preserves the existing main-branch scene while the room work is developed.

When integrating rooms:

1. Replace selected-object collection with the current room's complete collidable list. Resolve barriers before detecting sword/body contacts, and include `link.Sword` when its bounds are nonempty.
2. Use the actual room bounds with `ClampToRoom`. Match block drawing scale and collider size if the room loader changes tile scale.
3. Give door tiles `CollidableTypes.Door` and set `Properties.OnCollide` to the room system's transition callback. It receives the colliding Link. The handler invokes that callback; this change does not implement or claim working room transitions.
4. Ensure `UpdateDamage` runs for every active enemy each frame, including enemies whose movement update can return early. The current `EnemyManager` does this for its selected enemy.
5. Keep pickup removal and projectile responses in their corresponding handlers. Neither is registered here.

## Verification

Run the headless regression checks from the repository root:

```sh
dotnet run --project Tests/CollisionChecks
```

The console project uses the real collision, Link, tile, sword, and enemy classes with graphics-free test doubles. It needs no additional test framework packages. Checks cover barrier contact on all four sides in either collider order, walkable tiles, damage cooldown/tint recovery, scaled enemy bounds, sword direction and attack timing, once-per-swing damage, item-use exclusion, bounds clamping, door callback dispatch, and removing the final enemy.

If MonoGame tools have already been restored, use `-p:AutoRestoreMGCBTool=false` to skip restoring those tools again.

Manual smoke check:

```sh
dotnet run --project Byte/Byte.csproj
```

- Walk toward the selected block at (1200, 300) from each side. Use T/Y to compare solid tiles with walkable floor, stairs, and ladder tiles.
- Approach the selected enemy and confirm the red/white flash and rising hits-taken counter. The counter should keep rising after HP reaches zero, at most once per 0.5 seconds. O/P cycles the current enemy for testing.
- Press Z/N while facing an enemy. The yellow sword box should appear during the swing; the enemy flashes and disappears after three separate hits.
- Walk to each screen edge and resize the window. Link's body should stay inside the logical room, aligned with its outline.
- Press R to rebuild the level with fresh health and colliders.

These manual steps describe what to check; the automated checks do not render or validate the native game window.
