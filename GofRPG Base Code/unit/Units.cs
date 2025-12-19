using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro.Examples;

///<summary>
/// Unit is a class that holds all the unchangable variables/units
/// that the Framework uses for basic calculations, assigning values,
/// and more.
///</summary>
public static class Units
{
    public const float MAX_VOLUME = 1f;
    public const float MIN_VOLUME = 0f;
    public const float MAX_TEXT_SPEED = 0.01f;
    public const float MIN_TEXT_SPEED = 0.03f;
    public const float PLAYER_SPEED = 6f;

    public const double CRIT_DMG = 0.25;

    public const int PRIORITY_ITEM_STAGE_1 = 20;
    public const int PRIORITY_ITEM_STAGE_2 = 10;
    public const int PRIORITY_ITEM_STAGE_3 = 5;

    public const int HEAL_1 = -1; //10%
    public const int HEAL_2 = -2; //25%
    public const int HEAL_3 = -3; //33%
    public const int HEAL_4 = -4; //50%
    public const int HEAL_5 = -5; //66%
    public const int HEAL_6 = -6; //75%
    public const int HEAL_7 = -7; //100%

    public const int FLINCH_STAGE_1 = 100;
    public const int FLINCH_STAGE_2 = 25;
    public const int FLINCH_STAGE_3 = 5;

    public const double STAB_DMG = 0.5;

    public const double BRAWLER_ELIXIR_RATE = 1.25;
    public const double DEFENDER_ELIXIR_RATE = 1.25;
    public const double ESOIC_ELIXIR_RATE = 1.75;
    public const double SPECIALIST_ELIXIR_RATE = 1.5;
    public const double SWORDSMAN_ELIXIR_RATE = 1.25;

    public const int ATK_INDEX = 0;
    public const int DEF_INDEX = 1;
    public const int EVA_INDEX = 2;
    public const int HP_INDEX = 3;
    public const int SPD_INDEX = 4;
    public const int ELX_INDEX = 5;

    public const int BASE_ELX = 5;
    public const double BASE_ACC = 100.0 / 100.0;
    public const double BASE_CRT = 5.0 / 100.0;
    public const double LOWEST_CRT = 125.0 / 10000.0;

    public const double STAGE_POS_6 = 8.0 / 2.0;
    public const double STAGE_POS_5 = 7.0 / 2.0;
    public const double STAGE_POS_4 = 6.0 / 2.0;
    public const double STAGE_POS_3 = 5.0 / 2.0;
    public const double STAGE_POS_2 = 4.0 / 2.0;
    public const double STAGE_POS_1 = 3.0 / 2.0;
    public const double STAGE_0 = 2.0 / 2.0;
    public const double STAGE_NEG_1 = 2.0 / 3.0;
    public const double STAGE_NEG_2 = 2.0 / 4.0;
    public const double STAGE_NEG_3 = 2.0 / 5.0;
    public const double STAGE_NEG_4 = 2.0 / 6.0;
    public const double STAGE_NEG_5 = 2.0 / 7.0;
    public const double STAGE_NEG_6 = 2.0 / 8.0;

    public const double BURN_DMG = 1.0 / 10.0;

    public const int FREEZE_CHANCE_1 = 25;
    public const int FREEZE_CHANCE_2 = 50;
    public const int FREEZE_CHANCE_3 = 75;
    public const int FREEZE_DURATION = 3;

    public const int PETRIFIED_RATE = 90;
    public const int PETRIFIED_ROUNDS = 3;

    public const double POISON_DMG_STG_1 = 5.0 / 100.0;
    public const double POISON_DMG_STG_2 = 10.0 / 100.0;
    public const double POISON_DMG_STG_3 = 15.0 / 100.0;

    public const int STUN_PROB_1 = 25;
    public const int STUN_PROB_2 = 50;
    public const int STUN_PROB_3 = 75;

    public const int CHARM_CHANCE = 30;
    public const int LOWER_CHARM_CHANCE = 15;

    public const int EXHAUSTION_LEVEL_1 = 1;
    public const int EXHAUSTION_LEVEL_2 = 2;
    public const int EXHAUSTION_LEVEL_3 = 3;
    public const int EXHAUSTION_LEVEL_4 = 4;
    public const int EXHAUSTION_LEVEL_5 = 5;
    public const int EXHAUSTION_LEVEL_6 = 6;

    public const int CONFUSION_RATE = 30;
    public const double CONFUSION_HIT = 1.0 / 8.0;

    public static Move BASE_ATTACK = new RegularMove("Base Attack", "User performs a normal attack for no Elixir.", 0.4, 1.0, "NONE", 1, MoveTarget.ENEMY, MoveType.REGULAR, 0, null);

    public const double SUCCESSION_RATE_1 = 1.0 / 4.0;
    public const double SUCCESSION_RATE_2 = 2.0 / 4.0;
    public const double SUCCESSION_RATE_3 = 3.0 / 4.0;
    public const double SUCCESSION_RATE_4 = 4.0 / 4.0;

    public const double ELIXIR_REGEN_RATE = 1.0 / 10.0;

    public const string INITIALIZE_STATE = "INITIALIZE STATE";
    public const string BEFORE_ROUND_STATE = "BEFORE ROUND STATE";
    public const string OPTION_STATE = "OPTION STATE";
    public const string CHARACTER_ACTION_STATE = "CHARACTER ACTION STATE";
    public const string ACTION_EFFECT_STATE = "ACTION EFFECT STATE";
    public const string KNOCK_OUT_STATE = "KNOCK OUT STATE";
    public const string AFTER_ROUND_STATE = "AFTER ROUND STATE";
    public const string BATTLE_OVER_STATE = "BATTLE OVER STATE";
    public const string END_BATTLE = "END BATTLE";

    public const float TIME_PER_PART = 60;

    public const int MORNING = 0;
    public const int EVENING = 1;
    public const int NIGHT = 2;

    public const int ORIGINAL = 0;
    public const int NARRATION = 1;
    public const int CONFIRMATION = 2;
    public const int DECISION = 3;

    public const int DOWN = 0;
    public const int LEFT = 1;
    public const int RIGHT = 2;
    public const int UP = 3;

    public class Music
    {
        public const string OPENING_THEME = "opening_theme";
        public const string ENDING_THEME = "ending_theme";
        public const string OUTDOORS_1 = "outdoors_01";
        public const string INDOORS_1 = "indoors_01";
        public const string INDOORS_2 = "indoors_02";
        public const string INDOORS_3 = "indoors_03";
        public const string INDOORS_4 = "indoors_04";
        public const string REGULAR_BATTLE_THEME = "regular_battle_theme";
        public const string WILD_BATTLE_THEME = "wild_battle_theme";
        public const string RIVAL_BATTLE_THEME = "rival_battle_theme";
        public const string BOSS_BATTLE_THEME = "boss_battle_theme";
        public const string AGENT_BOSS_BATTLE_THEME = "agent_boss_battle_theme";
        public const string BINARY_BATTLE_THEME = "binary_battle_theme";
        public const string BINARY_BOSS_BATTLE_THEME = "binary_boss_battle_theme";
        public const string TEAM_SQL_BATTLE_THEME = "team_sql_battle_theme";
        public const string FINAL_BOSS_BATTLE_THEME = "final_boss_battle_theme";
        public const string VICTORY_THEME = "victory_theme";
        public const string GAME_OVER_THEME = "game_over";
        public const string DUNGEON_THEME = "dungeon_theme";
        public const string ROUTE_1 = "route_01";
        public const string ARGUS_BEACH_THEME = "argus_beach_theme";
        public const string TIRO_TOWN_THEME = "tiro_town_theme";
        public const string TIRO_TOWN_AUTHORITY_THEME = "tiro_town_authority_theme";
        public const string TOXIC_TRAILS_THEME = "toxic_trails_theme";
        public const string LOST_CITY_THEME = "lost_city_theme";
        public const string ESOIC_CITY_THEME = "esoic_city_theme";
        public const string ROUTE_2 = "route_02";
        public const string LONATURUS_THEME = "lonaturus_theme";
        public const string ROUTE_3 = "route_3";
    }

    public class SoundEffect
    {
        public const string DOOR_OPEN_1 = "door_open_01";
        public const string SCROLL_1 = "scroll_05";
        public const string OPEN_UI_1 = "open_01";
        public const string CLOSE_UI_4 = "close_04";
        public const string CLICK_1 = "click_01";
        public const string RECIEVED = "recieved";
        public const string QUEST_COMPLETED = "quest_completed";
        public const string HEALTH_RECHARGE = "health_recharge";
        public const string EMOTE = "emote";
        public const string PURCHASED = "purchased";
    }

    public static string[] DATABASE_PATHS =
    {
        "/database/abilities.csv",
        "/database/base_stats.csv",
        "/database/characters.csv",
        "/database/effects.csv",
        "/database/health_boost_effects.csv",
        "/database/immunity_effects.csv",
        "/database/items.csv",
        "/database/map.csv",
        "/database/moves.csv",
        "/database/negation_effects.csv",
        "/database/priority_moves.csv",
        "/database/protect_moves.csv",
        "/database/quests.csv",
        "/database/recoil_effects.csv",
        "/database/stat_change_effects.csv",
        "/database/stat_changing_moves.csv",
        "/database/status_changing_moves.csv",
        "/database/status_condition_effects.csv",
        "/database/story_flags.csv"
    };

    public static string[] SAVE_DATA_PATHS =
    {
        "/database",
        "/game_data",
        "/game_data/player_info",
    };

    public const string SETTINGS_DATA_PATH = "/game_data/game_settings.rpg";
    public const string PLAYER_DATA_PATH = "/game_data/player_info/player.rpg";
    public const string INVENTORY_DATA_PATH = "/game_data/player_info/inventory.rpg";
    public const string STORY_FLAG_DATA_PATH = "/game_data/story_flags.rpg";
    public const string QUEST_DATA_PATH = "/game_data/quest.rpg";
    public const string ITEM_DATA_PATH = "/game_data/items.rpg";
    public const string NPC_DATA_PATH = "/game_data/npcs.rpg";
    public const string WELL_DATA_PATH = "/game_data/wells.rpg";
    public const string MEDICAL_CENTER_DATA_PATH = "/game_data/medical_centers.rpg";
    public const string MAP_DATA_PATH = "/game_data/maps.rpg";
    public const string DAY_NIGHT_CYCLE_DATA_PATH = "/game_data/day_night_cycle_.rpg";
    public const string CHAPTER_DATA_PATH = "/game_data/chapter.rpg";
}