# Sprint 2 Design Document

This document explains how the Sprint 2 game framework is put together: what each major component does, which design patterns it uses, and how it talks to the rest of the game. Sprint 2 is a non-interactive framework: every object draws, moves, animates and changes state, but nothing collides with anything else yet.

## Overview

```mermaid
flowchart LR
    KB[KeyboardController] -->|Execute| CMD[ICommand objects]
    CMD --> Link
    CMD --> PM[ProjectileManager]
    CMD -->|RequestReset| Game
    Game -->|BuildLevel| RL[RoomLoader]
    RL --> RM[RoomManager]
    RM --> Rooms
    Game -->|Update / Draw| Link & RM & PM
```

Each frame, `Program.Update` first polls the controllers, which run the commands bound to the pressed keys. Then it updates the current room, projectiles, and Link. `Program.Draw` asks each active component to draw into one shared `SpriteBatch`.

| Pattern                       | Where it is used                                                                                                       |
| ----------------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| Command                       | Every key action is an `ICommand` stored in a key → command dictionary                                                 |
| State                         | Link's behavior is split into `IPlayerState` classes                                                                   |
| Factory                       | `BlockFactory`, `ItemFactory`, `ProjectileFactory`, `EnemyFactory` create objects and own the sprite sheet coordinates |
| Template method / inheritance | `AbstractEnemy` → `PatrolEnemy` / `WaitingPatrolEnemy` / `WanderingEnemy` → concrete enemies                           |

---

## 1. Core loop and level loading (`Byte/Core`)

**What it does.** `Program` owns the MonoGame loop, the `SpriteBatch`, the controllers, and the room and projectile managers. `GameAssets` loads every texture once and exposes it through a singleton. `GameConstants` holds shared rendering and window values.

**Level loading.** At startup, `RoomLoader` reads every JSON file in `Content/Rooms`. `RoomData` stores each room's dimensions, sparse tile placements, player spawn, enemy and item spawns, and door destinations. Positions in these files are grid coordinates; omitted tile positions are empty. `Room` converts positions to world coordinates and creates blocks, enemies, and items through their factories. `RoomManager` owns the room collection and updates/draws only its current room.

**Room file format.** `tiles` maps each block type accepted by `BlockFactory` to a list of `[x, y]` positions within the declared `width` and `height`; unlisted cells are empty. Player, enemy, item, door, and enemy-path positions also use `[x, y]` grid coordinates. Each door has a cardinal `direction`, a `targetRoom` name, and an optional target `arrivalPosition`. Exactly one room must set `isStartingRoom` to `true`; loading and manager setup reject invalid dimensions, positions, duplicate tile coordinates or room names, and unresolved door targets. Example format:

```json
{
  "name": "Demo",
  "background": "room1",
  "isStartingRoom": true,
  "playerSpawn": [1, 1],
  "width": 6,
  "height": 5,
  "tiles": {
    "Brick": [
      [0, 0],
      [1, 0],
      [2, 0]
    ],
    "SpeckledFloor": [
      [1, 1],
      [2, 1],
      [3, 1]
    ]
  },
  "enemies": [
    {
      "type": "Stalfos",
      "position": [3, 2]
    }
  ],
  "items": [
    {
      "type": "Heart",
      "position": [2, 3]
    }
  ],
  "doors": [
    {
      "position": [5, 2],
      "direction": "East",
      "targetRoom": "Demo",
      "arrivalPosition": [1, 2]
    }
  ]
}
```

**Doors.** Each door entry becomes a `Door` collidable covering its grid tile. `Program.LoadRooms` sets every door's `OnCollide` to remember which door Link touched; once collision dispatch is done, `ProcessCollisions` calls `RoomManager.EnterDoor`, which swaps the current room and returns the arrival tile. Link is put on that tile with zero velocity so he does not walk straight back through. `]` / `[` call `RoomManager.NextRoom` / `PreviousRoom` to jump between rooms for debugging.

**Reset (`R`).** `BuildLevel()` reloads the room files, creates a new projectile manager and a new `Link` at the starting room's data-defined spawn, and rebuilds all keybindings so every command points at the new objects. `ResetCommand` calls `Program.RequestReset()`, which only sets a flag; `Update` runs `BuildLevel()` after the controllers finish. This avoids changing the keybinding dictionary while `KeyboardController` is still looping over it.

**Resizing.** Everything is drawn in a fixed 1600×1600 virtual space. When the window changes size, `OnClientSizeChanged` rebuilds a scale-and-translate matrix that is passed to `SpriteBatch.Begin`, so the game keeps its aspect ratio at any window size.

## 2. Input handling (`Byte/Controller`, `Byte/Command`)

**Pattern: Command.** `KeyboardController` holds a `Dictionary<Keys, ICommand>`. Each frame it looks up every bound key that is down and calls `Execute(GameTime)` on its command. The controller knows nothing about Link, enemies or blocks, and commands know nothing about the keyboard. Changing a key binding is a one-line change in `Game.BuildLevel()`.

**Press-only keys.** Some actions should run once per key press rather than every frame while the key is held (`E`, `R`). The controller keeps a set of these keys and runs their commands only on the frame the key goes down.

**Case.** MonoGame reports physical keys, so upper and lower case letters trigger the same binding.

`MouseController` follows the same idea with a `Dictionary<Rectangle, ICommand>` per mouse button; right-clicking inside the window quits.

## 3. Player (`Byte/Player`)

**Pattern: State.** `Link` holds the player's data: position, facing direction, health, damage flag and current sprite. Its behavior lives in its current `IPlayerState`:

| State                               | Behavior                                                 |
| ----------------------------------- | -------------------------------------------------------- |
| `PlayerIdleState`                   | Still frame for the facing direction                     |
| `PlayerMoveUp/Down/Left/RightState` | Two-frame walking animation that follows Link's position |
| `PlayerAttackState`                 | Four-frame sword swing played once, then back to idle    |
| `PlayerUseItemState`                | Item-use pose played once, then back to idle             |

`Link.Update` and `Link.Draw` forward to the current state. `Link.GetNewState` swaps states and clears the old sprite, so the next state builds its own animation. Attack and use-item set `IsAttacking`, which blocks movement commands until the animation finishes.

**Commands → Link.** Movement commands set Link's direction and speed and switch to the matching move state. `AttackCommand` calls `Link.Attack()`, `UseProjectileCommand` calls `Link.UseItem()`, and `PlayerTakeDamageCommand` calls `Link.TakeDamage()`.

**Damage.** `TakeDamage()` lowers health and starts a 0.5 second timer. While the timer runs, `Link.Update` tints the current sprite red; when it expires the tint is cleared. The tint is applied after the state updates, so it works in every state.

## 4. Sprites (`Byte/Sprite`)

All visuals go through the `ISprite` interface (`Update`, `Draw`, color, flip effects, origin, looping). Implementations:

- `StaticSprite`: one frame at a fixed position.
- `AnimatedSprite`: loops through frames at a fixed position.
- `MovingAnimatedSprite`: frames plus a position that can be moved each frame; supports play-once animations (`Loop = false`, `IsFinished`) and per-frame origins so uneven attack frames stay lined up.

Game objects hold an `ISprite` and decide where it goes and when it changes; the sprite decides how it is drawn. This keeps drawing details out of gameplay logic.

## 5. Blocks (`Byte/Block`)

**Pattern: Factory.** `BlockFactory` is the only class that knows the tile sheet. The tiles sit on a grid (16×16 tiles, 17 px apart), so each `CreateXBlock` method just picks a column and row. There are 11 block classes (square, keystone, two statues, void, speckled floor, water, stairs, brick, ladder, sand). They all extend `AbstractBlock`, which stores a sprite and a position and implements `IBlock`.

Rooms own their block lists. The active room updates and draws all of its blocks.

## 6. Items (`Byte/Item`)

**Pattern: Factory.** `ItemFactory` creates the five pickups (rupee, heart, boomerang, bow, bomb) from the item sheet. Rupee, heart and boomerang use an `AnimatedSprite` that flashes between two colors; bow and bomb use a `StaticSprite`. Every item implements `IItem`.

Every item extends `AbstractItem`, which holds the sprite, position and the scaled frame size used as its hitbox. Items are `ICollidable` (type `ItemPickup`); when Link touches one the handler calls `Collect()`, the item stops drawing and the room drops it on the next update. Rooms own their item lists and update and draw all items in the active room.

## 7. Projectiles (`Byte/Projectile`)

Every projectile implements `IProjectile` (`Update`, `Draw`, `IsExpired`). `ProjectileManager` updates and draws all live projectiles and removes expired ones each frame, so each projectile class only has to decide when it is done.

| Projectile  | Behavior                                           | Expires                                    |
| ----------- | -------------------------------------------------- | ------------------------------------------ |
| `Arrow`     | Flies straight in Link's facing direction          | After 700 px or when it leaves the window  |
| `Bomb`      | Sits for a 1 s fuse, then plays the explosion once | When the explosion animation finishes      |
| `Boomerang` | Flies out 450 px, then reverses                    | When it returns to where it was thrown     |
| `Fireball`  | Aquamentus's attack; flies in a straight line      | After 1200 px or when it leaves the window |

**Pattern: Factory.** `ProjectileFactory` owns all projectile sprite coordinates (Link's sheet for the player's items, the boss sheet for fireballs). `UseProjectileCommand` takes a factory method as a delegate, so one command class handles keys `1`, `2` and `3`: it computes the spawn point in front of Link and adds whatever projectile the delegate creates.

## 8. Enemies (`Byte/Sprite/Enemy`)

**Movement through inheritance.** `AbstractEnemy` holds the position, velocity and sprite. Its `Update` calls the abstract `Move` to pick a velocity, applies it, and clamps the result so the whole sprite stays inside `GameConstants.WINDOW_SIZE`. Subclasses only decide _how_ to move:

| Base class           | Movement                                                          | Used by                                                        |
| -------------------- | ----------------------------------------------------------------- | -------------------------------------------------------------- |
| `PatrolEnemy`        | Walks between waypoints along cardinal directions                 | `Stalfos` (square loop, sprite flips every 0.25 s)             |
| `WaitingPatrolEnemy` | Same, but pauses at each waypoint                                 | `Gel` (moves, then waits), `Aquamentus` (paces back and forth) |
| `WanderingEnemy`     | Heads to random nearby points, picking a new one on arrival (RNG) | `Keese` (also speeds up over its first 2 s)                    |

Waypoints and wander targets are clamped to the window too, so an enemy never gets stuck pushing against an edge toward a point it cannot reach.

**Boss attack.** `Aquamentus` adds a fire timer: every 2 seconds it creates three fireballs from its mouth in a leftward spread through `ProjectileFactory`. It keeps the fireballs in its own `ProjectileManager`, so they update and draw only while Aquamentus is the enemy on screen.

**Creation.** `EnemyFactory` builds the enemy described by each room's data, including its path and timing settings. Rooms own their enemy lists and update and draw all enemies in the active room.

---

## 9. Collisions (`Byte/Collidable`)

**Pattern: Strategy.** Each pair of object types gets its own handler. Anything that can touch something implements `ICollidable`, which is just a `BoundingBox` plus a `CollisionProperties` (type string, `BlocksPlayer`, `DamagesPlayer`, `DamagesEnemies`, and an optional `OnCollide` callback). `CollisionDetector` takes a list of collidables and returns every pair that overlaps, along with which side they hit on (whichever overlap axis is smaller). `CollisionDispatcher` maps a `(typeA, typeB)` pair to one `CollisionHandler` and registers the flipped pair for you, so a rule only has to be written once. `LinkCollisionHandler.Register` sets up the rules we have so far:

| Pair | What happens |
|---|---|
| Player / Block | If the block has `BlocksPlayer`, Link gets pushed back out on the side he hit. If he only clipped the corner by 24 px or less he gets nudged sideways instead, which is what makes one-tile door gaps passable. |
| Player / Enemy | `Link.TakeDamage()`: lose 1 HP, flash red, 0.5 s invulnerable. |
| Sword / Enemy | `SwordHitbox.Hit` hurts an enemy once per swing. At 0 HP the room drops it. |
| Player / ItemPickup | `IItem.Collect()`, the room drops the item next update. |
| Player / Door | The door's `OnCollide` remembers the door, and the room switch happens after dispatch. |

`Program.ProcessCollisions` runs right after `Link.Update`. Blocks go first so everything after sees Link's corrected position, then he is clamped to the window, then one dispatch over Link, the sword hitbox, and the current room's enemies, items and doors. The sword is its own `ICollidable` (`SwordHitbox`) and only has a box during the active swing frames, so a sword hit can't be confused with Link bumping into something.

## Known design debt

- Projectiles aren't `ICollidable` yet, so arrows, bombs and boomerangs go through everything.
- Enemies clamp to the window, not to the room's walls.
- There is no camera. Rooms draw in the fixed 1600×1600 space and a door is a hard cut to the next room.
- `Link` still has its own frame rectangles instead of getting them from a sprite factory.

