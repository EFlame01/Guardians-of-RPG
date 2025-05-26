# Guardians of RPG Framework
Created by S.A.
## Introduction
Guardians of RPG is a 16-bit turn based Role Playing Game where you wake up as the main character in the middle of the town with  no memories of your past life. In your quest to find out who you are and your overall purpose, you uncover a plot by an evil organization who intends to use 3 ancient relics to refactor the world into their own image.

This README file explains the mechanics behind the framework used to code the game. In other words, cloning this repo will not give you access to the version of the game for yourself, it will give you the skeletal structure to design a game with similar mechanics. All you need to do is study this README file to understand how everything works.
## Table of Contents
### [Section 1: How Code is Organized](https://github.com/EFlame01/Guardians-of-RPG/blob/main/README.md/#section-1-how-code-is-organized-1)
### [Section 2: GofRPG_Framework](https://github.com/EFlame01/Guardians-of-RPG/blob/main/README.md/#section-2-gofRPG_framework-1)
1. Character
    * Character
    * Player
2. Archetype
    * Archetype
    * RegularSwordsman
    * DualSwordsman
    * Knight
    * HeavyShielder
    * EnergyManipulator
    * NatureManipulator
    * MixedMartialArtist
    * Berserker
    * CombatSpecialist
    * WeaponSpecialist
3. Stats
    * BaseStats
4. Quests
    * Quest
    * QuestManager
5. Items
    * Item
    * ItemType
    * FoodItem
    * HealingItem
    * KeyItem
    * MedicalItem
    * PriorityItem
    * StatChangingItem
    * Inventory
6. Moves
    * Move
    * MoveType
    * MoveTarget
    * RegularMove
    * PriorityMove
    * StatChangingMove
    * StatusChangingMove
    * HealingMove
    * KnockoutMove
    * ProtectMove
    * CounterMove
    * MoveManager
7. Status
    * BattleStatus
    * Blind
    * Burn
    * Charm
    * Confuse
    * Deafen
    * Exhaustion
    * Flinch
    * Frighten
    * Frozen
    * Petrified
    * Poison
    * Restrain
    * Sleep
    * Stun
    * TurnStatus
8. Effects
    * Effect
    * EffectType
    * EffectOrigin
    * AnnounceEffect
    * HealthBoostEffect
    * ImmunityEffect
    * NegationEffect
    * RechargeEffect
    * RecoilEffect
    * StabBoostEffect
    * StatChangeEffect
    * StatusConditionEffect
9. Abilities
    * Ability
    * AbilityManager
10. Database
    * CharacterMaker
    * MoveMaker
    * ItemMaker
    * QuestMaker
    * EffectMaker
    * AbilityMaker
    * StoryFlagMaker
    * MapDescMaker
    * DataEncoder
11. Units
### [Section 3 Game Design](https://github.com/EFlame01/Guardians-of-RPG/blob/main/README.md/#section-3-game-design-1)
1. Singleton
    * DialogueManager
    * GameManager
    * Singleton
2. Game Commands
    * InputHandler
    * PlayerDirection
    * PlayerState
3. Audio
    * AudioManager
    * SceneMusicPlayer
    * Sound
4. Cycle
    * DayNightCycle
    * LightCycle
5. Story
    * StoryFlag
    * StoryFlagManager
6. Objects
    * Interactable Objects
        * CutSceneObject
        * DoorObject
        * InteractableObject
        * ItemObject
        * MedicalObject
        * NPCObject
        * Shop Object
            * ShopObject
            * ShopList
        * SignObject
        * WellObject
    * Non Interactable Objects
        * Portal
        * NPC Features
            * NPCCompanions
            * NPCPathFinder
    * Object Sprites
        * ObjectSprites
    * Player Object
        * PlayerSpawn
        * PlayerSprite
    * ActivateObject
7. Cut Scene
    * Cut Scene State
        * AnimationState
        * DecisionState
        * DialogueState
        * DialogueState
        * EndState
        * FightState
        * QuestState
        * SetChapterState
        * SetSceneState
        * SubCutSceneState
8. Scene
    * Scene Changes
        * ChapterScene
        * StartScene
    * Intro
    * SceneLoader
    * TransistionTexture
    * TransitionType
9. Tracking
    * CharacterPos
    * LocationInformation
    * WalkCycleState
    * WayPoint
10. Wild Encounter
    * WildEncounter
    * WildGrass
11. XP
    * Level
12. Battle
    * Battle Character
        * BattleCharacter
        * BattleCharacterData
    * Battle Graphics
        * BattleActionEffect
        * EnvironmentDetail
        * MoveEffects
    * Battle States
        1. InitializeState
        2. OptionState
        3. Roll Options
            * RollBlockState
            * RollInitiativeState
            * RollRunState
        4. CharacterActionState
        5. ActionEffectState
        6. ActionEffectState2
        7. KnockoutState
        8. AfterRoundState
        9. BattleOverState
    * NPCLogic
    * BattleInformation
    * BattleOrderBST
    * BattleSimStatus
    * BattleSimulator
    * BattleStateMachine
13. UI
    * Battle UI
        * CharacterHUD
        * PlayerHUD
        * Options UI
            * BattleOptions
            * InitiativeOption
            * ItemOption
            * MoveOption
            * Options
            * TargetOption
        * BattleTimer
        * TargetButton
    * Dialogue
        * DialogueData
        * IDialogue
        * TextBox
            * TextBox
            * TextBoxCharacter
            * TextBoxConfirmation
            * TextBoxDecision
    * Map Menu
        * Map
        * MapCursor
        * MapLocation
    * Menu
        * DiscardMenuOption
        * InventoryMenu
        * MenuButton
        * MenuState
        * MenuStateManager
        * MoveSetMenu
        * OptionsMenu
        * PlayerMenu
        * QuestMenu
        * ShopMenu
    * ButtonUI
    * SliderBar
14. CameraFocus
15. BlockCharacterCollision
16. Game Data
    * InventoryData
    * ItemData
    * ItemDataContainer
    * MedicalCenterData
    * MedicalCenterDataContainer
    * NpcData
    * NpcDataContainer
    * PlayerData
    * QuestData
    * SaveSystem
    * SettingsData
    * StoryFlagData
    * WellData
    * WellDataContainer
## Section 1: How Code is Organized
## Section 2: GofRPG_Framework
## Section 3: Game Design
