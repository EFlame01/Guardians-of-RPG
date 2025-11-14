# Guardians of RPG Framework

Created by S.A.

## Introduction

Guardians of RPG is a 16-bit turn based Role Playing Game where you wake up as the main character in the middle of the town with no memories of your past life. In your quest to find out who you are and your overall purpose, you uncover a plot by an evil organization who intends to use 3 ancient relics to refactor the world into their own image.

This README file explains the mechanics behind the framework used to code the game. In other words, cloning this repo will not give you access to the version of the game for yourself, it will give you the skeletal structure to design a game with similar mechanics. All you need to do is study this README file to understand how everything works.

## Table of Contents

### [Section 1: How Code is Organized](https://github.com/EFlame01/Guardians-of-RPG/blob/main/README.md/#section-1-how-code-is-organized-1)

### [Section 2: GofRPG Base Code](https://github.com/EFlame01/Guardians-of-RPG/blob/main/README.md/#section-2-gofrpg-base-code-1)

1. Character
   - Character
   - Player
2. Archetype
   - Archetype
   - RegularSwordsman
   - DualSwordsman
   - Knight
   - HeavyShielder
   - EnergyManipulator
   - NatureManipulator
   - MixedMartialArtist
   - Berserker
   - CombatSpecialist
   - WeaponSpecialist
3. Stats
   - BaseStats
4. Quests
   - Quest
   - QuestManager
5. Items
   - Item
   - ItemType
   - FoodItem
   - HealingItem
   - KeyItem
   - MedicalItem
   - PriorityItem
   - StatChangingItem
   - Inventory
6. Moves
   - Move
   - MoveType
   - MoveTarget
   - RegularMove
   - PriorityMove
   - StatChangingMove
   - StatusChangingMove
   - HealingMove
   - KnockoutMove
   - ProtectMove
   - CounterMove
   - MoveManager
7. Status
   - BattleStatus
   - Blind
   - Burn
   - Charm
   - Confuse
   - Deafen
   - Exhaustion
   - Flinch
   - Frighten
   - Frozen
   - Petrified
   - Poison
   - Restrain
   - Sleep
   - Stun
   - TurnStatus
8. Effects
   - Effect
   - EffectType
   - EffectOrigin
   - AnnounceEffect
   - HealthBoostEffect
   - ImmunityEffect
   - NegationEffect
   - RechargeEffect
   - RecoilEffect
   - StabBoostEffect
   - StatChangeEffect
   - StatusConditionEffect
9. Abilities
   - Ability
   - AbilityManager
10. Database
    - CharacterMaker
    - MoveMaker
    - ItemMaker
    - QuestMaker
    - EffectMaker
    - AbilityMaker
    - StoryFlagMaker
    - MapDescMaker
    - DataEncoder
11. Units

### [Section 3 Game Design](https://github.com/EFlame01/Guardians-of-RPG/blob/main/README.md/#section-3-game-design-1)

1. Singleton
   - DialogueManager
   - GameManager
   - Singleton
2. Game Commands
   - InputHandler
   - PlayerDirection
   - PlayerState
3. Audio
   - AudioManager
   - SceneMusicPlayer
   - Sound
4. Cycle
   - DayNightCycle
   - LightCycle
5. Story
   - StoryFlag
   - StoryFlagManager
6. Objects
   - Interactable Objects
     - CutSceneObject
     - DoorObject
     - InteractableObject
     - ItemObject
     - MedicalObject
     - NPCObject
     - Shop Object
       - ShopObject
       - ShopList
     - SignObject
     - WellObject
   - Non Interactable Objects
     - Portal
     - NPC Features
       - NPCCompanions
       - NPCPathFinder
   - Object Sprites
     - ObjectSprites
   - Player Object
     - PlayerSpawn
     - PlayerSprite
   - ActivateObject
7. Cut Scene
   - Cut Scene State
     - AnimationState
     - DecisionState
     - DialogueState
     - DialogueState
     - EndState
     - FightState
     - QuestState
     - SetChapterState
     - SetSceneState
     - SubCutSceneState
8. Scene
   - Scene Changes
     - ChapterScene
     - StartScene
   - Intro
   - SceneLoader
   - TransistionTexture
   - TransitionType
9. Tracking
   - CharacterPos
   - LocationInformation
   - WalkCycleState
   - WayPoint
10. Wild Encounter
    - WildEncounter
    - WildGrass
11. XP
    - Level
12. Battle
    - Battle Character
      - BattleCharacter
      - BattleCharacterData
    - Battle Graphics
      - BattleActionEffect
      - EnvironmentDetail
      - MoveEffects
    - Battle States
      1. InitializeState
      2. OptionState
      3. Roll Options
         - RollBlockState
         - RollInitiativeState
         - RollRunState
      4. CharacterActionState
      5. ActionEffectState
      6. ActionEffectState2
      7. KnockoutState
      8. AfterRoundState
      9. BattleOverState
    - NPCLogic
    - BattleInformation
    - BattleOrderBST
    - BattleSimStatus
    - BattleSimulator
    - BattleStateMachine
13. UI
    - Battle UI
      - CharacterHUD
      - PlayerHUD
      - Options UI
        - BattleOptions
        - InitiativeOption
        - ItemOption
        - MoveOption
        - Options
        - TargetOption
      - BattleTimer
      - TargetButton
    - Dialogue
      - DialogueData
      - IDialogue
      - TextBox
        - TextBox
        - TextBoxCharacter
        - TextBoxConfirmation
        - TextBoxDecision
    - Map Menu
      - Map
      - MapCursor
      - MapLocation
    - Menu
      - DiscardMenuOption
      - InventoryMenu
      - MenuButton
      - MenuState
      - MenuStateManager
      - MoveSetMenu
      - OptionsMenu
      - PlayerMenu
      - QuestMenu
      - ShopMenu
    - ButtonUI
    - SliderBar
14. CameraFocus
15. BlockCharacterCollision
16. Game Data
    - InventoryData
    - ItemData
    - ItemDataContainer
    - MedicalCenterData
    - MedicalCenterDataContainer
    - NpcData
    - NpcDataContainer
    - PlayerData
    - QuestData
    - SaveSystem
    - SettingsData
    - StoryFlagData
    - WellData
    - WellDataContainer

## Section 1: How Code is Organized

The code is organized into 2 folders: The GofRPG Base Code and the Game Design code.

- GofRPG Base Code is the folder that holds the core mechanics of the Guardians of RPG.
- Game Design is the folder that houses the files that interact specifically with the Unity game engine.

In other words, There is an order of levels in which the code directly interacts with the user:

> GofRPG Base Code folder >> Game Design folder>> Unity Engine >> User

To learn more about a specific part, you may either explore section 2 for the base code, or section 3 for the game design code.

## Section 2: GofRPG Base Code

### Chapter 1: Character

In Guardians of RPG, a Character is a specialized NPC with attributes that make them able to battle, use items, and more. The following chapter will discuss the different types of Character classes in the base code.

#### Character

Character is a class that holds all the character's basic information needed for the GofRPG Base Code.

The Character class is used primarily for battle. As the player, you need certain attributes to fight other characters, and vice versa. The character class is also used as a base class for the Player class.

The Character class has the following attributes:

- ID:
  - The value of the character with its specific information. This is unique to each Character class.
  - string
- Type:
  - What kind of character the game is interfacing with. The code treats each type differently.
  - PLAYER, ALLY, ENEMY, BOSS
  - string
- Name:
  - The name of the character. Multiple Character classes can have the same name.
  - string
- Level:
  - What level the character is in.
  - int
- Bits
  - In game currency.
  - int
- Archetype
  - The specific class the character operates under. This is important for other variables used in battle.
  - Archetype
- Sex
  - What the character's sex is.
  - MALE, FEMALE, MALEFE
  - string
- BattleMoves
  - List of Moves that the character can use during battle
  - Move[]
- BaseStats
  - Information on all of the Character's stats used for battle.
  - BaseStats
- BattleStatus
  - Information on the Character's battle status during battle.
  - BattleStatus
- Ability
  - The skill that the Character can use during battle (this code is not yet used).
  - Ability
- Item
  - The held item that the Character has. This can be loot collected from the Player after battle, or a held item that a Character uses for battle.
  - Item

The Character class has 2 constructors: The main constructor used as a way to put all of the character information inside, and an empty constructor to have the character's information defaulted. If you ever want to update the attributes for the character, you can use the getter and setter methods for this class. There are setter methods for the following attributes:

- Name
  - SetName(string name)
  - Cannot be null
- Level
  - SetLevel(int level)
  - Cannot be less than 1 or more than 100
- Bits
  - SetBits(int bits)
  - Cannot be less than 0
- Archetype
  - SetArchetype(string archetype)
  - Cannot be null
  - Must be an actual archetype in the game
- Sex
  - SetSex(string sex)
  - Must be MALE, FEMALE, or MALEFE
- MoveSet
  - SetMoveSet(string[] moves)
  - Cannot be null
  - Cannot be an empty string array
  - Move names must be valid and in database
- BaseStats
  - SetBaseStats(int fullhp, int atk, int def, int eva, int hp, int spd, int acc, int crt)
  - Stats cannot be less than 1
- Ability
  - SetAbility(Ability ability)
  - Only works if Character.Ability is null
- Item
  - SetItem(Item item)
  - Only works if Character.Item is null

While there will be plenty of "characters" in the game, they will not have a Character class associated with them. In fact, Character is not a component, so you cannot attach a Character class onto a character in the game.

The way the Character class is incorporated is by means of speficic NPCs in the game. These NPCs will have a character_id attached to them, and that ID will be sent to the _CharacterMaker_ class that instantiates a Character class during the _BattleSimulator_.

> NPC has ID >> CharacterMaker creates Character with ID >> Character used in BattleSimulator

The only other time the Character class is used is as the _Player_ class.

#### Player

Player is a class that extends the Character class. This class is only designed for the player and allows the player's extra information to be updated, such as their movesets, inventory, quests, and more.

## Section 3: Game Design
