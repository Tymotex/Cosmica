# Cosmica

Hold down the front lines with a small army of warships to protect the galaxy against the invading alien menace!

PROTECT THE GALAXY!\
Triumph against 28 different types of alien invaders across 50 intense levels.

HONE YOUR STRATEGY!\
Pick and plan your battlefield with 7 different types of defenders, each with 3 permanent upgrade tiers of their own. 

EMBRACE THE SCI-FI EXPERIENCE!\
Feel like a boss as you journey through a picturesque side-scrolling space environment listening to glorious sci-fi tunes!

## Links:
- Google Play listing: https://play.google.com/store/apps/details?id=com.Cosmica.Cosmica
- Showcase/Trailer: https://www.youtube.com/watch?v=D7K1GOqns1w
- Development showcase: https://www.youtube.com/watch?v=ganf7AlCvV0

<p float="left">
  <img src="https://github.com/Tymotex/Cosmica/blob/master/PublicImages/Thumbnail.PNG?raw=true" width="35%" />
  <img src="https://github.com/Tymotex/Cosmica/blob/master/PublicImages/Triumph.png?raw=true" width="35%" /> 
</p>
<p float="left">
  <img src="https://github.com/Tymotex/Cosmica/blob/master/PublicImages/Screenshot_2020-02-22-21-19-47-41_fda667b7fd960e9419fea7f2286c2314.png?raw=true" width="50%" />
  <img src="https://github.com/Tymotex/Cosmica/blob/master/PublicImages/Shop_1.png?raw=true" width="35%" />
</p>

# Development Logs

Cosmica_4:
- Added 2 fighter ships to be selected at the bottom bar
Extended Selector.cs to handle the idea of having a currently selected unit. When the player makes a selection, then clicking on the tile will spawn the selected unit. Making a different selection spawns that newly selected ship
- Also extended Selector.cs to spawn a glow effect to indicate which unit is currently selected

Cosmica_5:
- Added rocket booster particle effects
- Added a currency system (energy) and assigned costs to ships. The player cannot place an unlimited number of ships anymore
- Added popups for invalid player decisions (eg. attempting to spend more energy than the player has, or attempting to place a ship when no ship type has been selected

Cosmica_6:
- Fixed bug where multiple defenders could be instantiated on the same tile
- Placed units will only shoot if there is an enemy in their lane
- Added 2 new columns (the final will have more than this)
- Added a simple pulse animation for the tiles when they are hovered over

Cosmica_7:
- Added a health bar which dynamically shrinks proportionately to damage taken
- Greatly extended Health.cs: added a popup for when a unit gets damaged (refactored parent-child animator controller issues), attached a health bar, added an energy reward when the player defeats enemies

Cosmica_8:
- Added mechanisms for control to be increased/decreased (destroying enemies, enemies crossing the border, enemies destroying friendly units)
- Added 'shredders' to prevent gameobjects like enemies and projectiles from translating indefinitely off-screen. These shredders handle decrementing control when triggered by crossing enemies
- Scene transitions to a fail/success screen when control reaches 0% or 100%
- Added a dynamic control bar and energy bar UI elements

Cosmica_9:
- Added a timer bar UI element which keeps track of the time elapsed (realtime) since the level started in mins:secs format
- Added new win condition: when the timer exceeds the maximum time (5 mins for production), it goes into overtime where enemies stop spawning and the player has to deal with remaining units. By the end of overtime, if the player has achieved >=50% control, then they win, otherwise they fail
- Added several level epilogue and level prologue animations including fading in, fading out UI elements, text and banner popup, win condition SFX, etc.
- Added a timescaler that sets Time.timeScale

Cosmica_10:
- Added new fade transitions between all scenes
- Added a basic sfx playing system handle mostly by Projectile.cs for impact sounds
- Added persistent background music that plays a random track and follows the singleton pattern

Cosmica_11:
- Added a functional options scene which allows the music volume and game volume to be toggled and have these settings persist across different sessions (using Unity's PlayerPrefs system). Soundtrack changing still unimplemented
- Selected defenders can now be deselected by pressing them again
- Added a pause menu when ESC is pressed at any point during the game (setting on and off canvas UI gameobject)

Cosmica_12:
- Added new classes (SceneData, SceneDataManager, OutcomeWriter) to handle passing data to the level outcome screen where we want to display data. This prevents the need to have designated success/failure screen for EVERY level 

Cosmica_13:
- Major changes to EnemySpawner. Added a difficulty ramping system where, for instance, for every 30 seconds the player stays in the game, the higher the chance of stronger enemy units spawning. Introduced a new class, SpawnChances, which lets the level designer manually set the spawn chance of a particular enemy on each difficulty interval. Eg. Tougher enemies may have 0% spawn chance for the first 1 minute, then have 25% afterwards for 1min - 2min, then have 100% afterwards
- Added a preparation phase for start of the level. The player has a fixed energy allocated at the start, allowing them to plan out the battlefield without time pressure
- Quit to main menu from the pause menu is now working

Cosmica_14:
- Major changes to Enemy and Defender scripts. Refactored Enemy and Defender classes as EnemyBehaviour and DefenderBehaviour and added a wrapper gameobject which can now be used to control animations properly. More sophisticated animations are now possible with the animation controller being attached to the parent controller
- Added a trigger near the end of the left side of the screen where if any enemy enters its region, they speed up towards the left shredder (crossing the border and causing the player to lose control)
- Added new animations, eg. the spawned defenders will first swerve in and continue its idle pulse animation
- Added level completion animations where enemies retreat, defenders advance and projectiles dissolve

Cosmica_15:
- Added lots of new SFX, eg. level ending SFX for success/failure, spawning SFX for defender ships and enemy ships
- Implemented unit highlighting and movement feature. The player can now click any unit on the battlefield to highlight it and then click any valid tile to move the reposition the unit
- Added a background decoration spawner

Cosmica_16:
- Player is now able to toggle the music in the options menu and pause menu
- 20 new soundtracks have been introduced
- All the GUI has been overhauled with new assets. Lots of small details added, eg audio has been added to buttons, header UI shows the current level, etc.
- New LevelSelection scene which lets the player pick the level to play (currently only 1 so far)
- Nebula background now transitions colour over the 5 minutes of gameplay

Cosmica_17:
- Added persistent currency and coin dropping system where coins of different worth have a chance to be dropped on each enemy kill
- Added level unlocking system (currently designed to only work with 5 zones each with 10 levels)
- Added a shop scene for players to spend credits on permanent ship upgrades (currently not functional)
- Greatly improved the info panel when hovering over defender units in the selection bar

Cosmica_18:
- Added a functional shop system that lets players spend credits and have their ship upgrades persist across sessions
- Added all 7 defender unit prefabs, all 3 of their upgrade tiers as well as a whole bunch of ammunition for them (currently poorly tuned)
- Added a scoring system which takes into account the score achieved on destroying enemies, the time taken to win and the total energy spent to grade the player on their performance on a given level

Cosmica_19:
- Added persistent high score keeping. The level outcome screen now displays the player's high score and the level rewards (credits and newly unlocked ships). Also made sure that completing a level for the first time gives a 'base' reward while every subsequent time doesn't
- Added a panel on the prep phase UI where the player can check the stats of the enemies by hovering over the icons

Cosmica_20:
- Added new explosion VFX and muzzle VFX
- Added spawn interval difficulty ramping
- Added new defender tile spawning effects
- Improved performance by minisming calls to inefficient methods
- Added 22 new enemies and their ammunition
- Added tips display
- Added a tutorial scene

Cosmica_21:
- Added a game startup loading screen (still unreliable)
- Improved the background OST by adding 8 new soundtracks and removing some existing ones. 
- Improved load speed by fixing some data compression optimisation issues: http://blog.theknightsofunity.com/wrong-import-settings-killing-unity-game-part-1/?_ga=2.152208505.1101337095.1582258954-2035885584.1578100776
- Added a method for the player to reset all prefs

Cosmica_22:
- First production release! https://play.google.com/store/apps/details?id=com.Cosmica.Cosmica


