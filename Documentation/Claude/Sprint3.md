The Sprint 3 version is a big step up: rooms load from validated JSON, there's a pair-based collision dispatcher, enemy projectiles go through the shared manager, and `IFactory<TData,TObject>` is cleaner. Below are the issues I found by reading the code. I haven't run the game, and the README/REQUIREMENTS/DESIGN files weren't in this pack, so I couldn't check whether the room format is documented.

## Hard requirements not met

1. **Chamber and Hall have no pick-up items.** Both have `"items": []`. Every room needs at least one.
2. **There are no walls.** All rooms are filled with `Square` (non-blocking, so effectively floor), and nothing forms a boundary.
   - `ClampToRoom` clamps Link to `GameConstants.WINDOW_SIZE` (1600×1600), not the room's own width and height. A 12×7 room is only 1152×672, so Link can walk off into empty space.
   - Enemies and projectiles are also clamped or expired against the window.
   - The `background` field is required by the validator but never used.
3. **Player projectiles die almost immediately in three of the four rooms.** `ProjectileCollisionHandler` calls `Collided()` on any `Block` and never checks `BlocksPlayer`. Chamber, Hall and Vault are covered in `Square` floor, so arrows, bombs and boomerangs hit the floor tile they spawn on. The debug room only works because it has few tiles. Enemy projectiles have no block handler at all, so fireballs pass through solid blocks, which is inconsistent.
4. **Room switching is only sequential.** `[`/`]` and the mouse buttons step through rooms in order, which is acceptable per the assignment. But `Directory.GetFiles` order isn't guaranteed (alphabetical on Windows, arbitrary elsewhere), so "next room" is nondeterministic across machines. Sort explicitly or use a manifest.

## Data bugs in the room files

- **Link arrives inside barrier tiles in Hall.**
  - Chamber's west door arrives at Hall `[10,3]`, which is a `StatueRight`.
  - DebugRoom's door arrives at Hall `[1,3]`, which is a `StatueLeft`.
  - The block resolver will shove him out in some direction. Extend `RoomLoader.Validate` to reject spawn and arrival cells that overlap a blocking tile.
- DebugRoom's door sends Link back to `[3,2]`, where the Boomerang sits, so he picks it up on arrival.
- `Gel` appears in no room, so one enemy type is never exercised.
- **Path semantics are inconsistent.**
  - JSON paths are converted to absolute world positions.
  - The factory treats patrol paths as "translate so point 0 = position" but treats `BladeTrap` paths as offsets from position.
  - Hall's BladeTrap only works because its position is `[0,0]`. Pick one meaning and document it.

## Behavior bugs

- **Wrong sprite after direction changes.** `Link.GetNewState` no longer sets `currentSprite = null` (Sprint 2 did).
  - Move states only build a sprite when the current one isn't a `MovingAnimatedSprite`. Going W→A without passing through idle keeps the walk-up frames while moving left.
  - `PlayerUseItemState` also reuses the stale walking sprite when you press `1`/`2`/`3` while walking. The 100 ms fallback timer added there patches the symptom of this root cause.
  - Fix: build the sprite when the state is entered, not by checking the sprite type.
- **BladeTrap turns red and stays red after one hit.** `MechanicalEnemy.Update` doesn't call `base.Update`, so `UpdateDamage` never runs and `damageTimer` never decays. Goriya has the same flaw while its boomerang is out, because it returns early.
- **BladeTrap resets to (0,0) on room re-entry.** The `MovingSprite` constructor of `AbstractEnemy` never sets `startPos`, and room entry does `enemy.position = enemy.startPos`. It also doesn't reset `goingHome`, waypoints or timers.
- **Bombs deal damage every frame.** The explosion stays in the manager for 0.3 s, and each frame overlapping an enemy calls `TakeDamage()`, which has no cooldown. A 3 HP enemy dies in about 3 frames. The bomb also flies at 300 px/s instead of being placed.
- **`PatrolEnemy.OnBlocked` advances the waypoint once per colliding block.** Touching two adjacent tiles in one frame skips two waypoints.
- **`UseProjectileCommand` still doesn't check `IsAttacking`**, so `1`/`2`/`3` can interrupt a sword swing. Link also slides about 40 px after you release a key (the 2000 px/s² deceleration), which the original doesn't do.
- Holding `Z`/`N` re-triggers attacks, because those keys aren't press-only.

## Design and quality

- **`Program` does too much.** It is the entry point, rendering, collision orchestration and room switching. Extract a collision system, and make `DrawBoundingBoxes` and `BoundingBoxDrawer` instance state rather than public statics.
- **Collision allocates heavily.** `ProcessCollisions` creates a new `CollisionDetector` and a list for every block × projectile and block × enemy pair, every frame. Blocks sit on a grid, so you could look up only the cells under a hitbox instead of scanning every block.
- **Link's hitbox is a full 96×96 tile.** `CornerAssist` is a workaround that shifts Link sideways by up to 24 px on contact. A smaller hitbox (feet or lower half) is the usual fix.
- **Scattered constants.** The same 6 appears as `GameConstants.TILE_SIZE * 6`, `RoomGrid.BLOCK_SCALE`, `Link.SpriteScale`, and literal `6.0f` in states and projectiles. Default enemy paths still use window coordinates (Stalfos now patrols a window-sized zigzag).
- **Mutable shared state.** `AbstractProjectile.PlayerProjectileProperties` and `EnemyProjectileProperties` are static shared objects with public mutable fields. Goriya creates a player boomerang and then swaps its properties, even though `CreateEnemyBoomerang` exists unused.
- **Link still reaches into input.** `Link` takes the keyboard controller and reads pressed keys, and `MoveLeft`/`MoveRight` call `Keyboard.GetState()` directly.
- **Dead code.** `DoorState`/`Door.doorState`, closed door sprites, `linkSprite`, `gameGet` fields, `Link.Move`, `isGameStart`, `CreateEnemyBoomerang`, and `LinkSheet3/4` in the content file.
- **Debug and balance defaults.** Bounding boxes default to on. The original deviations to document are: BladeTrap is destructible and patrols continuously, every enemy has 3 HP (including Aquamentus), the player boomerang vanishes on hit, and boomerangs return to their throw point rather than to Link.

## Suggested fix order

1. Add items to Chamber and Hall, and fix the Hall arrival cells (data-only).
2. Add real wall tiles or a room-derived boundary, and clamp to room bounds. Also make the projectile-vs-block handler respect `BlocksPlayer`.
3. Fix state-entered sprite creation, the `MechanicalEnemy` damage timer, and `startPos`.
4. Extend validation (spawn/arrival not in barriers, and room ordering).
5. Extract collision orchestration out of `Program`.

> **Note**: This code was generated from: https://claude.ai/share/2d77e52c-caa5-44da-8d88-9df88a6f00d5
