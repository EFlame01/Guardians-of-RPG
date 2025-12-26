using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerDataCloud
{
    //DEFAULT PLAYER INFORMATION
    //General Info
    public string PlayerName;
    public int Level;
    public int Bits;
    public string ArchetypeName;
    public string Sex;

    //Move Info
    public string[] BattleMoves = new string[4];
    public string[] MovesLearned;

    //Base State Info
    public int FullHp;
    public int Atk;
    public int Def;
    public int Eva;
    public int Hp;
    public int Spd;
    public int Elx;
    public double Acc;
    public double Crt;

    //Ability Info
    public string AbilityName;
    public string[] AbilityList;

    public string EquippedItem;

    //XP Info
    public int CurrentXP;
    public int LimitXP;

    //Player Location Info
    public string PlayerSceneName;
    public string MapLocationName;
    public Vector3 LocationPosition;

    //CHAPTER INFORMATION
    public string SceneName;
    public string PartName;
    public string ChapterName;
    public string ChapterStoryFlag;

    //DAY NIGHT CYCLE INFORMATION
    public int TimeOfDay;
    public bool StartDayNightCycle;
    public int NumberOfDays;

    //INVENTORY INFORMATION
    public string[] ItemList;
    public int[] ItemAmount;

    //ITEM LIST INFORMATION
    public ItemDataContainer ItemDataContainer;

    //MEDICAL CENTER LIST INFORMATION
    public MedicalCenterDataContainer MedicalCenterDataContainer;

    //NPC LIST DATA
    public NpcDataContainer NpcDataContainer;

    //QUEST LIST DATA
    public QuestData[] QuestDatas;

    //STORY FLAG LIST DATA
    public StoryFlagData[] StoryFlagDatas;

    //WELL LIST DATA
    public WellDataContainer WellDataContainer;

    //TIME DATA
    public double TotalSavedPlayTime;

    public PlayerDataCloud()
    {
        Player player = Player.Instance();

        PlayerName = player.Name;
        Level = player.Level;
        Bits = player.Bits;
        ArchetypeName = player.Archetype == null ? null : player.Archetype.ArchetypeName;
        Sex = player.Sex;

        for (int i = 0; i < player.BattleMoves.Length; i++)
            BattleMoves[i] = player.BattleMoves[i] != null ? player.BattleMoves[i].Name : null;
        MovesLearned = MoveManager.MoveDictionary.Keys.ToArray();

        FullHp = player.BaseStats.FullHp;
        Atk = player.BaseStats.Atk;
        Def = player.BaseStats.Def;
        Eva = player.BaseStats.Eva;
        Spd = player.BaseStats.Spd;
        Hp = player.BaseStats.Hp;
        Elx = player.BaseStats.Elx;
        Acc = player.BaseStats.Acc;
        Crt = player.BaseStats.Crt;

        AbilityName = player.Ability == null ? null : player.Ability.Name;
        AbilityList = AbilityManager.AbilityDictionary.Keys.ToArray();

        EquippedItem = player.Item != null ? player.Item.Name : null;

        CurrentXP = player.CurrXP;
        LimitXP = player.LimXP;

        MapLocationName = MapLocation.GetCurrentMapLocation();
        LocationPosition = PlayerSpawn.PlayerPosition;

        // Chapter Data
        SceneName = ChapterScene.SceneName;
        PartName = ChapterScene.PartName;
        ChapterName = ChapterScene.ChapterName;
        ChapterStoryFlag = ChapterScene.ChapterStoryFlag;

        // Day/Night Cycle Data
        TimeOfDay = GameManager.Instance == null ? 0 : GameManager.Instance.TimeOfDay;
        NumberOfDays = GameManager.Instance == null ? 0 : GameManager.Instance.NumberOfDays;
        StartDayNightCycle = GameManager.Instance == null ? false : GameManager.Instance.StartDayNightCycle;

        // Inventory
        ItemList = player.Inventory.ItemList.Keys.ToArray();
        ItemAmount = player.Inventory.ItemList.Values.ToArray();

        // ItemDataList
        ItemDataContainer = new ItemDataContainer();

        // MedicalCenterDataList
        MedicalCenterDataContainer = new MedicalCenterDataContainer();

        // NpcDataList
        NpcDataContainer = new NpcDataContainer();

        // QuestDataList
        player.QuestManager.UpdateQuestData();
        QuestDatas = player.QuestManager.QuestDatas;

        // StoryFlagDataList
        player.StoryFlagManager.UpdateFlagData();
        StoryFlagDatas = player.StoryFlagManager.StoryFlagDatas;

        // WellDataList
        WellDataContainer = new WellDataContainer();
    }

    public void LoadData()
    {
        Player player = Player.Instance();

        player.SetName(PlayerName);
        player.SetLevel(Level);
        player.SetArchetype(ArchetypeName);
        player.SetBits(Bits);
        player.SetSex(Sex);

        for (int i = 0; i < BattleMoves.Length; i++)
        {
            string moveName = BattleMoves[i];
            if (!string.IsNullOrEmpty(moveName))
            {
                Move move = MoveMaker.Instance.GetMoveBasedOnName(moveName);
                player.BattleMoves[i] = move;
            }
        }
        for (int i = 0; i < MovesLearned.Length; i++)
        {
            string moveName = MovesLearned[i];
            if (!string.IsNullOrEmpty(moveName))
            {
                Move move = MoveMaker.Instance.GetMoveBasedOnName(moveName);
                MoveManager.MoveDictionary.Add(moveName, move);
            }
        }

        player.SetBaseStats(FullHp, Atk, Def, Eva, Hp, Spd, Elx, Acc, Crt);

        Ability ability = AbilityMaker.Instance.GetAbilityBasedOnName(AbilityName);
        player.SetAbility(ability);
        player.AbilityManager.AddAbilitiesToList(AbilityList);

        Item item = ItemMaker.Instance.GetItemBasedOnName(EquippedItem);
        player.SetItem(item);

        player.SetCurrentXP(CurrentXP);
        player.SetLimitXP(LimitXP);

        MapLocation.SetCurrentMapLocation(MapLocationName);
        PlayerSpawn.PlayerPosition = LocationPosition;

        //Chapter Data
        ChapterScene.ChapterName = ChapterName;
        ChapterScene.ChapterStoryFlag = ChapterStoryFlag;
        ChapterScene.PartName = PartName;
        ChapterScene.SceneName = SceneName;

        //Day/Night Cycle Data
        GameManager.Instance.TimeOfDay = TimeOfDay;
        GameManager.Instance.StartDayNightCycle = StartDayNightCycle;
        GameManager.Instance.NumberOfDays = NumberOfDays;

        //Inventory Data
        for (int i = 0; i < ItemList.Length; i++)
            player.Inventory.AddItem(ItemList[i], ItemAmount[i]);

        //Items Data
        ItemDataContainer.LoadItemDataIntoGame();

        //Medical Center Data
        MedicalCenterDataContainer.LoadMedicalCenterDataIntoGame();

        //NPC Data
        NpcDataContainer.LoadNpcDataIntoGame();

        //Quest Data
        player.QuestManager.QuestDatas = QuestDatas;
        player.QuestManager.LoadUpdatedQuestData();

        //StoryFlag Data
        player.StoryFlagManager.StoryFlagDatas = StoryFlagDatas;
        player.StoryFlagManager.LoadUpdatedFlagData();

        //Well Data
        WellDataContainer.LoadWellDataIntoGame();

        //Time Data
        TimeTracker.Instance().SetTotalSavedPlayTime(TotalSavedPlayTime);
        TimeTracker.Instance().StartTime();
    }
}