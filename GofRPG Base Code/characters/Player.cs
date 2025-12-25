
///<summary>
/// Player is a class that
/// extends the Character class. This 
/// class is only designed for the player
/// and allows the player's extra
/// information to be updated, such as their
/// movesets, inventory, quests, and more.
///</summary>
public class Player : Character
{
    private static Player _player;
    public int CurrXP { get; private set; }
    public int LimXP { get; private set; }
    public Inventory Inventory { get; private set; }
    public QuestManager QuestManager { get; private set; }
    public MoveManager MoveManager { get; private set; }
    public AbilityManager AbilityManager { get; private set; }
    public StoryFlagManager StoryFlagManager { get; private set; }

    private Player()
    {
        Id = "0";
        Name = "Adam";
        Type = "PLAYER";
        Level = 1;
        Bits = 500;
        CurrXP = 0;
        LimXP = 1;
        Archetype = null;
        Sex = null;
        BattleMoves = new Move[4] { null, null, null, null };
        BaseStats = new BaseStats(5, 5, 5, 5, 5);
        BattleStatus = new BattleStatus();
        Item = null;
        Ability = null;
        Inventory = new Inventory();
        QuestManager = new QuestManager();
        MoveManager = new MoveManager();
        AbilityManager = new AbilityManager();
        StoryFlagManager = new StoryFlagManager();
    }

    //Getters and Setters
    public void SetCurrentXP(int currXP)
    {
        if (currXP < 0)
            currXP = 0;
        CurrXP = currXP;
    }
    public void SetLimitXP(int limXP)
    {
        if (limXP < 1)
            limXP = 1;
        LimXP = limXP;
    }
    public void SetQuestManager(QuestManager questManager)
    {
        QuestManager = questManager;
    }
    public void SetInventory(Inventory inventory)
    {
        Inventory = inventory;
    }
    public void SetStoryFlagManager(StoryFlagManager storyFlagManager)
    {
        StoryFlagManager = storyFlagManager;
    }

    ///<summary>
    /// Allows the game to get the static
    /// instance of the player's information 
    /// every time. If an instance was not
    /// created, it will instantiate one first.
    ///</summary>
    ///<returns> An instance of the player. </returns>
    public static Player Instance()
    {
        _player ??= new Player();

        return _player;
    }

    /// <summary>
    /// Unequips an item from the player and places it
    /// back inside its inventory.
    /// </summary>
    public void UnequipItemFromPlayer()
    {
        if (Item == null)
            return;

        Inventory.ChangeItemAmount(Item.Name, 1);
        Item = null;
    }

    /// <summary>
    /// Method that determines if the player's avatar
    /// is male or female leaning.
    /// </summary>
    /// <returns></returns>
    public string MaleOrFemale()
    {
        if (Sex == null)
            SetSex("MALEFE");

        if (Sex.Equals("MALE"))
            return "MALE";
        else if (Sex.Equals("FEMALE"))
            return "FEMALE";

        if (GameManager.Instance.Leaning.Equals("MALE"))
            return "MALE";
        else if (GameManager.Instance.Leaning.Equals("FEMALE"))
            return "FEMALE";

        return null;
    }

    public void NullPlayer()
    {
        _player = null;
    }

    public void CreateNewInstance()
    {
        _player = null;
        Instance();
    }
}