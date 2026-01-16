# Roguelike Project

Developed with Unity (6000.0.31).<br>
This was a solo term project for a programming course at Bow Valley College, and my first project in unity of this scale.\
I decided to create a simple turn-based dungeon-crawler, and experiment with enemy AI decision-making.

## Combat and Gameplay
The player will control a party of adverturers through a series of rooms. Each room starts with a patrolling enemy: the player will have to hit the enemy with their own fireball attack before getting caught in order to start the combat sequence off with an advantage.
![Patrolling Enemy](https://github.com/raboull/Director_AI_Game/assets/60552485/2e49166e-f27e-43ec-af2f-98454a4336e6)

During combat, each unit will manage their own 'Battle Points' or **BP**. These points are used to execute skills, where more powerful skills cost more BP. Units gain one BP at the start of their turn, and may choose to 'guard' or use a 'basic attack' (free actions) in order to accumulate BP. After choosing an action, the player may be prompted to choose a target if applicable.

Enemies may use skills as well! Those available to them will become more powerful as the enemy difficulty increases.
![Combat HUD](https://github.com/raboull/Director_AI_Game/assets/60552485/2e49166e-f27e-43ec-af2f-98454a4336e6)

Clearing all three enemies will result in a victory for that room, and players may collect their reward and proceed through one of two new doors to the next one. The doors are marked with **Cleric**, **Ranger**, or **Knight**, denoting the party member that will receive a reward on completion of the following room. These rewards take the form of skill upgrades or level ups (increasing two stats). The player will always receive a choice of two random upgrades of this kind, one if which is guaranteed to be a skill upgrade.
![Reward Menu](https://github.com/raboull/Director_AI_Game/assets/60552485/2e49166e-f27e-43ec-af2f-98454a4336e6)

As this is an “endless” game, the player's goal is to make it through as many rooms as possible. Some fun stats will be presented at the end of each run!

## Enemy AI Overview
Enemies are separated into four difficulty types: **easy**, **mild**, **moderate**, and **hard** - each with their own set of stats. Harder enemies are a darker red, while easier enemies have a lighter colour. 

On their turn, the enemy will score each affordable action-target pair (including basic attacks), and then pick one randomly from a pool whose scores are within a certain threshold. This **Max Prioroty Threshold (MPT)** is a value in the range \[0, 1\] that is directly multiplied to the top-scoring pair -- any pair with a score larger or equal to this product is considered as a possible action that turn. For example, an MPT of 0 means that all valid actions are considered, while and MPT of 1 means only the highest scoring action will be considered. An MPT of 0.7 would mean that any action-target pair that is within 30% of the highest-scoring options would be put into the pool.

The priority score of each action is calculated based on its type: _Healing, Attack,_ or _Stat Modifier_. These scores are then directly multiplied by an enemy-specific constant (**C_Healing, C_Attack and C_StatMod** respectively), each also in the range \[0,1\]. This allows different enemy types to further prioritize different types of moves. For example, a Goblin Medic would prioritize using a healing skill on a damaged ally over attacking the player.

1) _**Healing Skills**_ are scored as a function of the percentage of max health the target has:
<p align="center">
  <img src="https://github.com/user-attachments/assets/bd509fb8-fe5b-4c43-8720-88d73aa3e766" alt="Healing Graph" width = "750">
</p>
It grows linearly as the percentage decreases until it hits the 50% threshold, then grows quadratically so that low-health targets are slightly more prioritized. <br><br>

2) _**Attack Skills**_ are calculated as a function of the target’s percent max health after damage is applied from the attack:
<p align="center">
  <img src="https://github.com/user-attachments/assets/ae505c75-22a9-421d-9db0-7faf97d304ed" alt="Attack Graph" width = "750">
</p>
Skills that deal more damage are prioritized. There are assignable minScore and hpThreshold values (for each skill instance) that determine what the base score is and at what HP threshold the score plateaus at this value. For the above example, minScore = 0.2 and hpThreshold = 0.7.<br><br>

4) _**Stat Modifying Skills**_ grow linearly with the target’s percent HP, but have an assignable minimum and maximum value:
<p align="center">
  <img src="https://github.com/user-attachments/assets/32ac337e-3f3c-4323-9ddb-67bf86ef97db" alt="Stat Mod Graph" width = "750">
</p>
It is best when the minimum and maximum values clamp the score more tightly, so that stat modifier skills are only considered when there is no urgent need to attack or heal. In this example, the minimum and maximum values are 0.2 and 0.6 respectively. <br><br>

A given enemy's **(MPT), C_Healing, C_Attack,** and **C_StatMod** values are specified in a [csv file](Assets/Resources/EnemyInfo.csv), alongside their stats and available skills.

## Windows Build
A zipped folder containing a windows build can be found [here]().<br>
Unzip the folder and launch Roguelike3D.exe to try out the game.

## Assets and Acknowledgements
[Character Models](https://assetstore.unity.com/packages/3d/characters/humanoids/fantasy/free-low-poly-modular-character-pack-fantasy-dream-321521)\
[Modular Dungeon Assets](https://assetstore.unity.com/packages/3d/environments/dungeons/dungeon-modular-pack-295430)
