using System.Collections.Generic;

///<summary>
/// MoveManager is a class that organizes
/// the <c>Player</c>'s moveset.
///</sumamry>
public class MoveManager
{
    public static Dictionary<string, Move> MoveDictionary { get; private set; }

    //Constructor
    public MoveManager()
    {
        MoveDictionary = new Dictionary<string, Move>();
    }

    public void AddMove(Move move)
    {
        AddToMovesLearned(move);
        AddToBattleMoves(move);
    }

    /// <summary>
    /// Adds move to MoveDictionary based on the 
    /// <paramref name="moveName"/>.
    /// </summary>
    /// <param name="moveName">name of the move</param>
    public void AddToMovesLearned(Move move)
    {
        // AddMovesLearned(moveName);
        if (MoveDictionary.ContainsKey(move.Name))
            return;
        MoveDictionary.Add(move.Name, move);
    }

    /// <summary>
    /// Adds move to BattleMoves
    /// slot if there is space for one based on the 
    /// <paramref name="moveName"/>.
    /// </summary>
    /// <param name="moveName">name of the move</param>
    public void AddToBattleMoves(Move move)
    {
        if (MoveExistsInBattleSlot(move.Name))
            return;

        int index = FindAvailableIndex();

        if (index < 0)
            return;

        if (MoveDictionary.ContainsKey(move.Name))
            Player.Instance().BattleMoves[index] = MoveDictionary[move.Name];
        else
            Player.Instance().BattleMoves[index] = move;
    }

    /// <summary>
    /// Returns the total number of moves learned.
    /// </summary>
    /// <returns>the total number of moves in the MoveDictionary</returns>
    public int TotalMovesLearned()
    {
        return MoveDictionary.Count;
    }

    public int TotalBattleMoves()
    {
        int total = 0;
        foreach (Move move in Player.Instance().BattleMoves)
        {
            if (move != null)
                total++;
        }

        return total;
    }

    public void RemoveBattleMove(string name)
    {
        Player player = Player.Instance();
        int index = 0;

        foreach (Move move in player.BattleMoves)
        {
            if (move != null && move.Name == name)
                break;
            index++;
        }

        player.BattleMoves[index] = null;
    }

    /// <summary>
    /// Checks if move exists in BattleMoves slots
    /// based on <paramref name="moveName"/>.
    /// </summary>
    /// <param name="moveName">name of the move</param>
    /// <returns></returns>
    public bool MoveExistsInBattleSlot(string moveName)
    {
        Move[] moves = Player.Instance().BattleMoves;
        foreach (Move move in moves)
            if (move != null && move.Name == moveName)
                return true;
        return false;
    }

    /// <summary>
    /// Finds index where move can be placed in BattleSlots.
    /// </summary>
    /// <returns>An index where an empty slot is available.
    /// Returns <c>-1</c> if there are no slots.</returns>
    private int FindAvailableIndex()
    {
        Move[] moves = Player.Instance().BattleMoves;
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i] == null || moves[i].Name.Length <= 0)
                return i;
        }
        return -1;
    }
}