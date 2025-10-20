using System.Collections.Generic;
using UnityEngine;

public class Level
{

    public static int DetermineXPForQuest()
    {
        return (int)Mathf.Pow(Player.Instance().Level, 2);
    }

    public static int DetermineXPForBattle(int level)
    {
        return (int)(2.5 * level);
    }

    public static void GainXP(int xp)
    {
        Player.Instance().SetCurrentXP(Player.Instance().CurrXP + xp);
    }

    public static bool CanLevelUp()
    {
        return Player.Instance().CurrXP >= Player.Instance().LimXP;
    }

    public static void LevelUpPlayer()
    {
        Player player = Player.Instance();

        if (!CanLevelUp())
            return;

        player.SetLevel(player.Level + 1);
        player.SetCurrentXP(player.CurrXP - player.LimXP);
        player.SetLimitXP((int)(Mathf.Pow(player.Level, (float)Units.STAGE_POS_1) * Units.STAGE_POS_1));
        player.BaseStats.LevelUpStats(player.Archetype.ChooseStatBoostRandomly());

        if (CanLevelUp())
            LevelUpPlayer();
    }

    public static Move[] DetermineLearnedMoves()
    {
        Player player = Player.Instance();
        Move[] moves = MoveMaker.Instance.GetLevelUpMoves(player.Level, player.Archetype.ArchetypeName, player.Archetype.ClassName);
        List<Move> learnedMoves = new List<Move>();

        if (moves.Length <= 0)
            return null;

        foreach (Move move in moves)
            if ((move.Level == player.Level) && !player.MoveManager.MoveExistsInBattleSlot(move.Name))
                learnedMoves.Add(move);

        return learnedMoves.ToArray();
    }

    public static void AddLearnedMoves()
    {
        Player player = Player.Instance();
        Move[] moves = MoveMaker.Instance.GetLevelUpMoves(player.Level, player.Archetype.ArchetypeName, player.Archetype.ClassName);

        if (moves.Length <= 0)
            return;

        foreach (Move move in moves)
            player.MoveManager.AddMove(move.Name);
    }
}