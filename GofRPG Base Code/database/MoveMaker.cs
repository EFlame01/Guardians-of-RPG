using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MoveMaker is a class that parses through
/// the data to create <c>Move</c> objects for
/// the <c>Character</c> class.
/// </summary>
public class MoveMaker : Singleton<MoveMaker>
{
    private const int MOVE_INDEX = 8;
    private const int PRIORITY_MOVE_INDEX = 10;
    private const int PROTECT_MOVE_INDEX = 11;
    private const int STAT_CHANGING_MOVE_INDEX = 15;
    private const int STATUS_CHANGING_MOVE_INDEX = 16;

    /// <summary>
    /// Gets and returns the <c>Move</c> object based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <returns>the <c>Move</c> objects or <c>null</c> if the move could not be found.</returns>
    public Move GetMoveBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        string[] moveAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[MOVE_INDEX], name).Split(',');

        return GetMove(moveAttributes, moveAttributes[5]);
    }

    /// <summary>
    /// Gets all the moves a player can learn based on their
    /// <paramref name="level"/> and their <paramref name="archetype"/> or
    /// <paramref name="classtype"/>.
    /// </summary>
    /// <param name="level">the current level of the player</param>
    /// <param name="archetype">the archetype of the player</param>
    /// <param name="classtype">the class of the player</param>
    /// <returns>an array of <c>Move</c> objects or null if no moves could be found</returns>
    public Move[] GetLevelUpMoves(int level, string archetype, string classtype)
    {
        string[] moveListData;
        List<Move> listOfMoves = new();

        moveListData = DataRetriever.Instance.SplitDataBasedOnRow(DataRetriever.Instance.Database[MOVE_INDEX]);
        foreach (string moveData in moveListData)
        {
            try
            {
                if (string.IsNullOrEmpty(moveData))
                    continue;

                string[] moveAttributes = moveData.Split(',');

                if (int.Parse(moveAttributes[7]) <= level && (moveAttributes[8].Equals(archetype) || moveAttributes[8].Equals(classtype)))
                {
                    Move move = GetMove(moveAttributes, moveAttributes[5]);
                    if (move != null)
                        listOfMoves.Add(move);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("WARNING: " + e.Message);
                Debug.LogWarning("Move Data: " + moveData);
                return listOfMoves.ToArray();
            }
        }

        return listOfMoves.ToArray();
    }

    /// <summary>
    /// Helper function that retrieves move based on the 
    /// <paramref name="moveAttributes"/> and <paramref name="moveType"/>
    /// </summary>
    /// <param name="moveAttributes">list of attributes the move has</param>
    /// <param name="moveType">the type of move it is</param>
    /// <returns>the <c>Move</c> object or null if no move was found</returns>
    private Move GetMove(string[] moveAttributes, string moveType)
    {
        Move move = null;
        string[] additionalAttributes;

        switch (moveType)
        {
            case "REGULAR":
                move = new RegularMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.Parse(moveAttributes[2]),
                    double.Parse(moveAttributes[4]),
                    moveAttributes[8],
                    int.Parse(moveAttributes[7]),
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.Parse(moveAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0])
                );
                break;
            case "PRIORITY":
                additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[PRIORITY_MOVE_INDEX], moveAttributes[0]).Split(',');

                move = new PriorityMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.Parse(moveAttributes[2]),
                    double.Parse(moveAttributes[4]),
                    moveAttributes[8],
                    int.Parse(moveAttributes[7]),
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.Parse(moveAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0]),
                    int.Parse(additionalAttributes[1])
                );
                break;
            case "HEALING":
                move = new HealingMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.Parse(moveAttributes[2]),
                    double.Parse(moveAttributes[4]),
                    moveAttributes[8],
                    int.Parse(moveAttributes[7]),
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.Parse(moveAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0])
                );
                break;
            case "KNOCK_OUT":
                move = new KnockoutMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.Parse(moveAttributes[4]),
                    moveAttributes[8],
                    int.Parse(moveAttributes[7]),
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.Parse(moveAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0])
                );
                break;
            case "PROTECT":
                additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[PROTECT_MOVE_INDEX], moveAttributes[0]).Split(',');

                move = new ProtectMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.Parse(moveAttributes[2]),
                    moveAttributes[8],
                    int.Parse(moveAttributes[7]),
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.Parse(moveAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0]),
                    additionalAttributes[1],
                    int.Parse(additionalAttributes[2])
                );
                break;
            case "COUNTER":
                move = new CounterMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.Parse(moveAttributes[2]),
                    double.Parse(moveAttributes[4]),
                    moveAttributes[8],
                    int.Parse(moveAttributes[7]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.Parse(moveAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0])
                );
                break;
            case "STAT_CHANGING":
                additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STAT_CHANGING_MOVE_INDEX], moveAttributes[0]).Split(',');

                move = new StatChangingMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.Parse(moveAttributes[2]),
                    double.Parse(moveAttributes[4]),
                    moveAttributes[8],
                    int.Parse(moveAttributes[7]),
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.Parse(moveAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0]),
                    additionalAttributes[1].Split('~'),
                    Array.ConvertAll(additionalAttributes[2].Split('~'), int.Parse)
                );
                break;
            case "STATUS_CHANGING":
                additionalAttributes = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STATUS_CHANGING_MOVE_INDEX], moveAttributes[0]).Split(',');

                move = new StatusChangingMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.Parse(moveAttributes[2]),
                    double.Parse(moveAttributes[4]),
                    moveAttributes[8],
                    int.Parse(moveAttributes[7]),
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.Parse(moveAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0]),
                    StatusCondition.GenerateStatusCondition
                    (
                        additionalAttributes[1],
                        int.Parse(additionalAttributes[2]),
                        int.Parse(additionalAttributes[3]),
                        int.Parse(additionalAttributes[4]),
                        int.Parse(additionalAttributes[5]),
                        int.Parse(additionalAttributes[6]),
                        int.Parse(additionalAttributes[7]),
                        int.Parse(additionalAttributes[8]),
                        int.Parse(additionalAttributes[9])
                    )
                );
                break;
            default:
                move = null;
                break;
        }
        return move;
    }

}