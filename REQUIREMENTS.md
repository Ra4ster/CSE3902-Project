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
  - [x] Create an `EnemyFactory` with _ONE_ method, and move the enemy loading logic to `EnemyManager`.
- Player:
- Projectiles:
  - [x] Create an `IProjectile` interface (`Update`, `Draw`, `IsExpired`) shared by every projectile.
  - [x] Create a projectile factory that loads the projectile art from the link sheet (only the factory knows the sheet coordinates).
  - [x] Create a projectile manager that updates and draws every live projectile and removes expired ones.
  - [x] Arrow: flies straight in Link's facing direction and despawns after a set distance or off screen.
  - [x] Bomb: placed in front of Link, plays the explosion animation after a short fuse, then despawns.
  - [x] Boomerang: flies out to its range, returns along the same path, then despawns.
  - [x] Add 1 / 2 / 3 keybindings to use the arrow, bomb and boomerang, with a delay so holding the key doesn't spam.
  - [x] Hook projectiles into the r reset so every projectile in flight is cleared.
  - [ ] Aquamentus fireball projectile (boss attack).
- Blocks:
  - [x] Load the dungeon tile sheet and save the bounds for each tile.
  - [x] Create the 11 block classes with a block factory and manager (only the factory knows the sheet coordinates).
  - [x] Add t / y keybindings to cycle through the blocks, with a delay so holding the key doesn't skip past blocks.
  - [x] Hook blocks into the r reset so they go back to the starting block.
- Items:

## Sprint 3

## Sprint 4

> Other sprints are `TODO`...
