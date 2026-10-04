# CSE3902 Project: Interactive Systems
> Group 5, AU2026 @ OSU

This is a repository built by Group 5 for [OSU's CSE3902 class: Interactive Systems Project](https://jholewinski.github.io/cse3902/index.html).

### Authors:

- Ayush Saggar
  - GitHub: [Podzied](https://github.com/Podzied)
  - Contact: [saggar.9@osu.edu](mailto:saggar.9@osu.edu)
- Baowen Liu
  - GitHub: [PowerSixxx](https://github.com/PowerSixxx)
  - Contact: [liu.11884@osu.edu](mailto:liu.11884@osu.edu)
- Ethan Singh
  - GitHub: [ethansingh123](https://github.com/ethansingh123)
  - Contact: [singh.2164@osu.edu](mailto:singh.2164@osu.edu)
- Jack Rose
  - GitHub: [Ra4ster](https://github.com/ra4ster)
  - Contact: [rose.1775@osu.edu](mailto:rose.1775@osu.edu)
- Tyler Brown
  - GitHub: [Brownt13487](https://github.com/Brownt13487)
  - Contact: [brown.9452@osu.edu](mailto:brown.9452@osu.edu)
- ~~Trish Pham~~

For this project we were tasked with implementing the first few features as part of a recreation of the original legend of Zelda and it's first dungeon. As part of the sprint 2 portion of this project this group was tasked with implementing movement and graphics, a player character that can move, attack, and take damage (have some indicator for damage taken), as well as implementing 10 block sprites, multiple enemies, item pickups, and projectiles.

Program Controls:
Player Movement WASD or Arrow Keys for basic movement of player, Z or N for player attacking
1,2,3 for 3 different items spawned from the player
Pressing e causes the player to flash red to indicate taking damage

Using the keys u and i you can cycle between the item currently being displayed
using the t and y keys will cycle the block being displayed
Using o and p causes the enemy to cycle between an enemy that is being displayed

pressing r will restart the simulation to inital values
pressing q will quit the game

Current known bugs - sprites do not properly scale with window size and thus objects can be lost if the window is not scaled high enough.


## How to use Git:

You can clone the repository using `git clone`. You are also able to publish your changes using the following CLI pattern:

```bash
# Tell git to track your changes:
git add .
# Tell git to save changes:
git commit -m "[Your message here]"
# Publish your updates to the group:
git push -u origin [branch-name]
```

If you would like to switch branches, run `git checkout [branch-name]`. For creating a new branch, do `git checkout -b [new-branch-name]`.

For any push you are not certain is a 'final push' (but rather still in development), consider checking out a new branch and creating a **pull request** (PR).

If you have not pushed a solution to a problem but still want to write it down, consider creating an **issue**!

## Useful Resources:

- [Class documentation](https://jholewinski.github.io/cse3902/resources.html)
- [Microsoft XNA (4.0) Docs](https://learn.microsoft.com/en-us/previous-versions/windows/xna/bb203894(v=xnagamestudio.41))
- [Git CLI cheatsheet](https://git-scm.com/cheat-sheet)
- [Agile Development, by Atlassian](https://www.atlassian.com/agile)
