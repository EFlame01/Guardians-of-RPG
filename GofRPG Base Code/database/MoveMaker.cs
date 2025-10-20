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
    private readonly string _moveDatabasePath = "/database/moves.csv";
    private readonly string _priorityMoveDatabasePath = "/database/priority_moves.csv";
    private readonly string _protectMoveDatabasePath = "/database/protect_moves.csv";
    private readonly string _statChangingMoveDatabasePath = "/database/stat_changing_moves.csv";
    private readonly string _statusChangingMoveDatabasePath = "/database/status_condition_effects.csv";

    /// <summary>
    /// Gets and returns the <c>Move</c> object based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <returns>the <c>Move</c> objects or <c>null</c> if the move could not be found.</returns>
    public Move GetMoveBasedOnName(string name)
    {
        if (name == null)
            return null;

        string[] mainAttributes;

        DataEncoder.Instance.DecodeFile(_moveDatabasePath);
        mainAttributes = DataEncoder.Instance.GetRowOfData(name).Split(',');
        DataEncoder.ClearData();

        return GetMove(mainAttributes, mainAttributes[5]);
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
        List<Move> listOfMoves = new List<Move>();

        DataEncoder.Instance.DecodeFile(_moveDatabasePath);
        // Debug.Log(DataEncoder.GetData());
        moveListData = DataEncoder.Instance.GetRowsOfData();
        DataEncoder.ClearData();
        // Debug.Log(moveListData);
        foreach (string moveData in moveListData)
        {
            try
            {
                if (moveData != null || moveData.Length > 0)
                {
                    Debug.Log(moveData.Length);
                    string[] moveAttributes = moveData.Split(',');
                    if (int.Parse(moveAttributes[7]) <= level && (moveAttributes[8].Equals(archetype) || moveAttributes[8].Equals(classtype)))
                        listOfMoves.Add(GetMove(moveAttributes, moveAttributes[5]));
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("WARNING: " + e.Message);
                return listOfMoves.ToArray();
            }
        }
        return listOfMoves.ToArray();
    }

    /// <summary>
    /// Helper function that retrieves move based on the 
    /// <paramref name="mainAttributes"/> and <paramref name="moveType"/>
    /// </summary>
    /// <param name="mainAttributes">list of attributes the move has</param>
    /// <param name="moveType">the type of move it is</param>
    /// <returns>the <c>Move</c> object or null if no move was found</returns>
    private Move GetMove(string[] mainAttributes, string moveType)
    {
        Move move = null;
        string[] additionalAttributes;

        switch (moveType)
        {
            case "REGULAR":
                move = new RegularMove
                (
                    mainAttributes[0],
                    mainAttributes[1].Replace('~', ','),
                    double.Parse(mainAttributes[2]),
                    double.Parse(mainAttributes[4]),
                    mainAttributes[8],
                    int.Parse(mainAttributes[7]),
                    Move.ConvertToMoveTarget(mainAttributes[3]),
                    Move.ConvertToMoveType(mainAttributes[5]),
                    double.Parse(mainAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0])
                );
                break;
            case "PRIORITY":
                DataEncoder.Instance.DecodeFile(_priorityMoveDatabasePath);
                additionalAttributes = DataEncoder.Instance.GetRowOfData(mainAttributes[0]).Split(',');
                DataEncoder.ClearData();
                move = new PriorityMove
                (
                    mainAttributes[0],
                    mainAttributes[1].Replace('~', ','),
                    double.Parse(mainAttributes[2]),
                    double.Parse(mainAttributes[4]),
                    mainAttributes[8],
                    int.Parse(mainAttributes[7]),
                    Move.ConvertToMoveTarget(mainAttributes[3]),
                    Move.ConvertToMoveType(mainAttributes[5]),
                    double.Parse(mainAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0]),
                    int.Parse(additionalAttributes[1])
                );
                break;
            case "HEALING":
                move = new HealingMove
                (
                    mainAttributes[0],
                    mainAttributes[1].Replace('~', ','),
                    double.Parse(mainAttributes[2]),
                    double.Parse(mainAttributes[4]),
                    mainAttributes[8],
                    int.Parse(mainAttributes[7]),
                    Move.ConvertToMoveTarget(mainAttributes[3]),
                    Move.ConvertToMoveType(mainAttributes[5]),
                    double.Parse(mainAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0])
                );
                break;
            case "KNOCK_OUT":
                move = new KnockoutMove
                (
                    mainAttributes[0],
                    mainAttributes[1].Replace('~', ','),
                    double.Parse(mainAttributes[4]),
                    mainAttributes[8],
                    int.Parse(mainAttributes[7]),
                    Move.ConvertToMoveTarget(mainAttributes[3]),
                    Move.ConvertToMoveType(mainAttributes[5]),
                    double.Parse(mainAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0])
                );
                break;
            case "PROTECT":
                DataEncoder.Instance.DecodeFile(_protectMoveDatabasePath);
                additionalAttributes = DataEncoder.Instance.GetRowOfData(mainAttributes[0]).Split(',');
                DataEncoder.ClearData();
                move = new ProtectMove
                (
                    mainAttributes[0],
                    mainAttributes[1].Replace('~', ','),
                    double.Parse(mainAttributes[2]),
                    mainAttributes[8],
                    int.Parse(mainAttributes[7]),
                    Move.ConvertToMoveTarget(mainAttributes[3]),
                    Move.ConvertToMoveType(mainAttributes[5]),
                    double.Parse(mainAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0]),
                    additionalAttributes[1],
                    int.Parse(additionalAttributes[2])
                );
                break;
            case "COUNTER":
                move = new CounterMove
                (
                    mainAttributes[0],
                    mainAttributes[1].Replace('~', ','),
                    double.Parse(mainAttributes[2]),
                    double.Parse(mainAttributes[4]),
                    mainAttributes[8],
                    int.Parse(mainAttributes[7]),
                    Move.ConvertToMoveType(mainAttributes[5]),
                    double.Parse(mainAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0])
                );
                break;
            case "STAT_CHANGING":
                DataEncoder.Instance.DecodeFile(_statChangingMoveDatabasePath);
                additionalAttributes = DataEncoder.Instance.GetRowOfData(mainAttributes[0]).Split(',');
                DataEncoder.ClearData();
                move = new StatChangingMove
                (
                    mainAttributes[0],
                    mainAttributes[1].Replace('~', ','),
                    double.Parse(mainAttributes[2]),
                    double.Parse(mainAttributes[4]),
                    mainAttributes[8],
                    int.Parse(mainAttributes[7]),
                    Move.ConvertToMoveTarget(mainAttributes[3]),
                    Move.ConvertToMoveType(mainAttributes[5]),
                    double.Parse(mainAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0]),
                    additionalAttributes[1].Split('~'),
                    Array.ConvertAll(additionalAttributes[2].Split('~'), int.Parse)
                );
                break;
            case "STATUS_CHANGING":
                DataEncoder.Instance.DecodeFile(_statusChangingMoveDatabasePath);
                additionalAttributes = DataEncoder.Instance.GetRowOfData(mainAttributes[0]).Split(',');
                DataEncoder.ClearData();
                move = new StatusChangingMove
                (
                    mainAttributes[0],
                    mainAttributes[1].Replace('~', ','),
                    double.Parse(mainAttributes[2]),
                    double.Parse(mainAttributes[4]),
                    mainAttributes[8],
                    int.Parse(mainAttributes[7]),
                    Move.ConvertToMoveTarget(mainAttributes[3]),
                    Move.ConvertToMoveType(mainAttributes[5]),
                    double.Parse(mainAttributes[9]),
                    EffectMaker.Instance.GetEffectsBasedOnName(mainAttributes[0]),
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