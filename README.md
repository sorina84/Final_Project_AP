# Project_AP
Project Ap Space Shooter

## Student Information

Name: Sorina Afshary
Student ID:403433033

Name: Negar Ehsanipoor
Student ID:403471006


# Project Overview
Space Shooter is a 2D arcade shooting game developed using C# and Windows Forms.
The main goal of this project is to design and implement a complete game while applying
object-oriented programming principles such as encapsulation, inheritance, and polymorphism.
The project includes player control, enemy AI behaviors, shooting mechanics, collision
detection, wave management, power-ups, shop system, and permanent data storage using SQLite.


# Technologies Used
- C#
- Windows Forms
- .NET Framework
- SQLite Database
- Visual Studio 2019


# Implemented Features

## Player System

The player controls a spaceship with smooth movement mechanics.

Implemented features:
- Acceleration-based movement
- Velocity and momentum system
- Friction-based stopping
- Shooting system
- Health management
- Shield ability
- Triple shot ability
- Fire rate upgrade support


## Enemy System

The enemy system is designed using inheritance and polymorphism.
The base class:
is extended by different enemy types:
- StandardEnemy
- ScoutEnemy
- ShooterEnemy
- HeavyTankEnemy
- TerroristEnemy


Each enemy has its own:
- Movement behavior
- Attack pattern
- Health value
- Special abilities


## Bullet System

The Bullet class manages all projectile behaviors.

Features:
- Player bullets
- Enemy bullets
- Multiple shooting directions
- Different bullet styles


## Collision System

Collision detection is managed separately using: WaveManager
Features:

- Multiple enemy waves
- Increasing difficulty
- Different enemy combinations
- Final challenge wave


## Power-Up System

Power-ups are managed using:PowerUp
Implemented abilities:

- Health Pack
- Shield
- Triple Shot
- Fire Rate Booster


## Coin and Score System

The project includes:

### CoinManager

Responsible for:
- Managing player coins
- Handling coin collection


### ScoreManager

Responsible for:
- Tracking current score
- Managing player achievements


## Shop System

The shop system is implemented using:ShopManager
Players can purchase and equip:

- Ship skins
- Bullet styles
- Backgrounds
- Additional boosters


## Database System

Permanent player data storage is implemented using SQLite.

Database management is handled by:DataManager
Stored information:

- Total coins
- High score
- Shop equipment
- Game settings

The database file is automatically created when the game starts.


# Project Architecture
```SpaceShooter
│
├── Forms
│   ├── MainMenuForm
│   ├── GameForm
│   ├── ShopForm
│   ├── OptionsForm
│   └── AboutForm
│
├── Core
│   ├── GameWorld
│   ├── DataManager
│   ├── WaveManager
│   ├── CollisionManager
│   └── ShopManager
│
├── Game Objects
│   ├── GameEntity
│   ├── Player
│   ├── Bullet
│   ├── Coin
│   ├── PowerUp
│   └── Explosion
│
└── Enemies
├── Enemy
├── StandardEnemy
├── ScoutEnemy
├── ShooterEnemy
├── HeavyTankEnemy
└── TerroristEnemy
```

# Object-Oriented Design

The project applies the main OOP concepts:

## Encapsulation
Each class manages its own internal data and behaviors.


## Inheritance
Enemy classes inherit from the base Enemy class.

Example:
Enemy
|
|– StandardEnemy
|– ScoutEnemy
|– ShooterEnemy
|– HeavyTankEnemy
|– TerroristEnemy


## Polymorphism

Different enemy behaviors are implemented using overridden methods.
For example:Update() , Attack()
are customized for each enemy type.


# How To Run The Project
1. Open the solution file in Visual Studio 2019.
2. Restore NuGet packages.
3. Build the solution.
4. Run the project.

# Required Packages
The project requires:
- System.Data.SQLite


# Game Controls
Keyboard Controls:
A / Left Arrow  : Move Left
D / Right Arrow : Move Right
Space           : Shoot

# Database Location
After running the project, SQLite database will be created automatically: bin/Debug/game.db

# Conclusion
Space Shooter is a complete Windows Forms based game project that demonstrates
object-oriented programming concepts, modular architecture, game logic design,
database management, and user interface development.
