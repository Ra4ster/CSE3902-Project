# Requirements of LOZ Game:

## Table of Contents

1.  [Sprint 1](#sprint-1): Introduction
2.  [Sprint 2](#sprint-2): Enemy, Player, Projectiles, Blocks, Items
3.  [Sprint 3](#sprint-3): _N/A (not implementing)_
4.  [Sprint 4](#sprint-4): _N/A (not not implementing)_

---

## Sprint 1

- [x] Figure out meeting times and dates.
- [x] Assign each job for sprint 2, including one for **pull reviews** and **documentation**.
- [x] Initialize repository using team members' Sprint 0.

## Sprint 2

- Enemies:
  - [x] Load enemy art for the 3 enemies + boss, saving the sprite's bounds.
  - [x] Create a path-based approach to each enemy, allowing them to patrol around; possibly using player-tracking.
  - [x] Create a test case in `Game.cs` for showing all enemies' movement (_no hitboxes, dmg, walls, etc. required_).
  - [x] Implement Aquamentus boss, different from the smaller enemies.
    - [x] Aquamentus spits a spread of three fireballs on a timer.
  - [x] Create an `EnemyFactory` with _ONE_ method, and move the enemy loading logic to `EnemyManager`.
  - [x] `O` / `P` cycle to the previous / next enemy, showing exactly one at a time.
  - [x] Enemy positions, patrol points and wander targets are clamped so the whole sprite stays inside the window.
  - [x] `R` rebuilds every enemy at its starting position and returns to the first enemy.
- Player:
  - [x] Link moves in the four cardinal directions at 200 px/s with `W`/`A`/`S`/`D` or the arrow keys.
  - [x] Movement changes Link's facing direction; vertical input takes priority over horizontal, so Link never moves diagonally (as in the original game).
  - [x] Idle state shows a still frame for the current facing direction (west is the east frame flipped).
  - [x] Walking state plays a two-frame walk animation for each direction.
  - [x] `Z` / `N` plays a four-frame sword attack in the facing direction; Link cannot move until the attack finishes.
  - [x] `1` / `2` / `3` play Link's item-use pose in the facing direction while the item is spawned.
  - [x] `E` damages Link: health drops by one and Link is tinted red, clearing after 0.5 seconds.
  - [x] Each behavior is its own `IPlayerState` class (Idle, MoveUp/Down/Left/Right, Attack, UseItem); `Link` delegates update and draw to its current state.
  - [x] `R` rebuilds Link at the starting position, facing south, idle and undamaged.
- Projectiles:
  - [x] Create an `IProjectile` interface (`Update`, `Draw`, `IsExpired`) shared by every projectile.
  - [x] Create a projectile factory that loads the projectile art from the link sheet (only the factory knows the sheet coordinates).
  - [x] Create a projectile manager that updates and draws every live projectile and removes expired ones.
  - [x] Arrow: flies straight in Link's facing direction and despawns after a set distance or off screen.
  - [x] Bomb: placed in front of Link, plays the explosion animation after a short fuse, then despawns.
  - [x] Boomerang: flies out to its range, returns along the same path, then despawns.
  - [x] Add 1 / 2 / 3 keybindings to use the arrow, bomb and boomerang, with a delay so holding the key doesn't spam.
  - [x] `R` rebuilds the projectile manager so every projectile in flight is cleared.
  - [x] Aquamentus fireball projectile (boss attack).
- Blocks:
  - [x] Load the dungeon tile sheet and save the bounds for each tile.
  - [x] Create the 11 block classes with a block factory and manager (only the factory knows the sheet coordinates).
  - [x] Add t / y keybindings to cycle through the blocks, with a delay so holding the key doesn't skip past blocks.
  - [x] `R` rebuilds the block list so it goes back to the starting block.
- Items:
  - [x] Five pickups: rupee, heart, boomerang, bow and bomb.
  - [x] Rupee, heart and boomerang flash between two color frames once per second; bow and bomb are static, matching the original game.
  - [x] Create every item through an `ItemFactory` (only the factory knows the item sheet coordinates).
  - [x] An `ItemManager` draws one item at a time; `U` / `I` cycle to the previous / next item with a 0.2 second repeat delay.
  - [x] Items are stationary and do not interact with other objects this sprint.
  - [x] Items are usable through the projectile keys: `1` fires the bow's arrow, `2` places a bomb, `3` throws the boomerang.
  - [x] `R` rebuilds the item list and returns to the first item.

## Sprint 3

## Sprint 4

> Other sprints are `TODO`...
