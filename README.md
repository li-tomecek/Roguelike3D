# Roguelike Project

Developed with Unity (6000.0.31).<br>
This was a solo term project for a programming course at Bow Valley College, and my first project in unity of this scale.\
I decided to create a simple turn-based dungeon-crawler, and experiment with enemy AI decision-making.

## Combat and Gameplay
The player will control a party of adverturers through a series of rooms. Each room starts with a patrolling enemy: the player will have to hit the enemy with their own fireball attack before getting caught in order to start the combat sequence off with an advantage./
![Patrolling Enemy](https://github.com/raboull/Director_AI_Game/assets/60552485/2e49166e-f27e-43ec-af2f-98454a4336e6)

During combat, each unit will manage their own 'Battle Points' or '**BP**'. These points are used to execute skills, with more powerful skills costing more BP. Units gain one BP at the start of their turn, and may need to use a basic attacks or guard in order to accumulate BP before spending them. After choosing an action, the player may be prompted to choose a target if applicable.\
Enemies may use skills as well! Those available to them will become more powerful as the enemy difficulty increases.
![Combat HUD](https://github.com/raboull/Director_AI_Game/assets/60552485/2e49166e-f27e-43ec-af2f-98454a4336e6)


Clearing all three enemies will result in a victory for that room, and players may collect their reward and proceed through one of two new doors to the next one. The doors are marked with **C** (Cleric), **R**(Ranger), or **K**(Knight), and denote the party member that will receive a reward the player will recieve on completing that room. These rewards take the form of skill upgrades, or level ups (increasing two stats). The player will always receive a choice of two random upgrades of this kind.  
![Reward Menu](https://github.com/raboull/Director_AI_Game/assets/60552485/2e49166e-f27e-43ec-af2f-98454a4336e6)

As this is an “endless” game, the player's goal is to make it through as many rooms as possible. Some fun stats will be presented at the end of each run!

## Controls
![Gane Controls](https://github.com/raboull/Director_AI_Game/assets/60552485/18c2d19d-d8d8-4ddd-b66f-757eb6f1377b)\

## Enemy AI Overview
Enemies are separated into four difficulty types: **easy**, **mild**, **moderate**, and **hard** - each with their own set of stats. Harder enemies are a darker red, while easier enemies have a lighter colour.

## Windows Build
A zipped folder containing a windows build can be found [here]().<br>
Unzip the folder and launch Roguelike3D.exe to try out the game.

## Assets and Acknowledgements
[Character Models](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/free-low-poly-modular-character-pack-fantasy-dream-321521)\
[Modular Dungeon Assets](https://assetstore.unity.com/packages/3d/environments/dungeons/dungeon-modular-pack-295430)
