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

    private const int MOVE_DATA_LENGTH = 10;
    private const int PRIORITY_MOVE_DATA_LENGTH = 2;
    private const int PROTECT_MOVE_DATA_LENGTH = 3;
    private const int STAT_CHANGING_MOVE_DATA_LENGTH = 3;
    private const int STATUS_CHANGING_MOVE_DATA_LENGTH = 10;

    public string[] GENERIC_BASE_MOVE_DATA = new string[10] { "Base Attack", "This move is replaced with the Base Attack because the original move could not be found in the database. Please report bug if found.", "0.4", "ENEMY", "1", "REGULAR", "NONE", "1", "", "0" };
    public string[] GENERIC_PRIORITY_MOVE_DATA = new string[2] { "Unknown Priority Move", "3" };
    public string[] GENERIC_PROTECT_MOVE_DATA = new string[3] { "Unknown Protect Move", "PROTECT", "1" };
    public string[] GENERIC_STAT_CHANGING_MOVE_DATA = new string[3] { "Unknown Stat Changing Move", "ATK~ATK", "1~-1" };
    public string[] GENERIC_STATUS_CHANGING_MOVE_DATA = new string[10] { "Unkown Status Changing Move", "0", "0", "0", "0", "0", "0", "0", "0", "0" };


    /// <summary>
    /// Gets and returns the <c>Move</c> object based on the <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <returns>the <c>Move</c> objects or <c>null</c> if the move could not be found.</returns>
    public Move GetMoveBasedOnName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        string moveData = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[MOVE_INDEX], name);
        if (string.IsNullOrEmpty(moveData) || moveData.Split(',').Length != MOVE_DATA_LENGTH)
        {
            Debug.LogWarning($"WARNING: The move {name} could not be found in the database. The move has been replaced with the Base Attack instead. Please report this bug.");
            return Units.BASE_ATTACK;
        }
        string[] moveAttributes = moveData.Split(',');
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

                string[] moveAttributes = moveData.Split(',').Length != MOVE_DATA_LENGTH ? GENERIC_BASE_MOVE_DATA : moveData.Split(",");

                int moveLevel = int.TryParse(moveAttributes[7], out int value) ? value : 0;
                if (moveLevel <= level && (moveAttributes[8].Equals(archetype) || moveAttributes[8].Equals(classtype)))
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
        string additionalData;
        string[] additionalAttributes;

        switch (moveType)
        {
            case "REGULAR":
                move = new RegularMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.TryParse(moveAttributes[2], out double regMovePower) ? regMovePower : 0.4,
                    double.TryParse(moveAttributes[4], out double regMoveAccuracy) ? regMoveAccuracy : 1.0,
                    moveAttributes[8],
                    int.TryParse(moveAttributes[7], out int regMoveLevel) ? regMoveLevel : 0,
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.TryParse(moveAttributes[9], out double regMoveElixirPoints) ? regMoveElixirPoints : 5,
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0])
                );
                break;
            case "PRIORITY":
                additionalData = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[PRIORITY_MOVE_INDEX], moveAttributes[0]);
                additionalAttributes = (string.IsNullOrEmpty(additionalData) || additionalData.Split(',').Length != PRIORITY_MOVE_DATA_LENGTH) ? GENERIC_PRIORITY_MOVE_DATA : additionalData.Split(',');

                move = new PriorityMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.TryParse(moveAttributes[2], out double priorityMovePower) ? priorityMovePower : 0.4,
                    double.TryParse(moveAttributes[4], out double priorityMoveAccuracy) ? priorityMoveAccuracy : 1.0,
                    moveAttributes[8],
                    int.TryParse(moveAttributes[7], out int priorityMoveLevel) ? priorityMoveLevel : 3,
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.TryParse(moveAttributes[9], out double priorityMoveElixirPoints) ? priorityMoveElixirPoints : 5,
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0]),
                    int.TryParse(additionalAttributes[1], out int priorityMovePriorityLevel) ? priorityMovePriorityLevel : 1
                );
                break;
            case "HEALING":
                move = new HealingMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.TryParse(moveAttributes[2], out double healingMovePower) ? healingMovePower : 0.4,
                    double.TryParse(moveAttributes[4], out double healingMoveAccuracy) ? healingMoveAccuracy : 1.0,
                    moveAttributes[8],
                    int.TryParse(moveAttributes[7], out int healingMoveLevel) ? healingMoveLevel : 0,
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.TryParse(moveAttributes[9], out double healingMoveElixirPoints) ? healingMoveElixirPoints : 5,
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0])
                );
                break;
            case "KNOCK_OUT":
                move = new KnockoutMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.TryParse(moveAttributes[4], out double knockoutMoveAccuracy) ? knockoutMoveAccuracy : 1.0,
                    moveAttributes[8],
                    int.TryParse(moveAttributes[7], out int knockoutMoveLevel) ? knockoutMoveLevel : 3,
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.TryParse(moveAttributes[9], out double knockoutMoveElixirPoints) ? knockoutMoveElixirPoints : 5,
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0])
                );
                break;
            case "PROTECT":
                additionalData = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[PROTECT_MOVE_INDEX], moveAttributes[0]);
                additionalAttributes = (string.IsNullOrEmpty(additionalData) || additionalData.Split(',').Length != PROTECT_MOVE_DATA_LENGTH) ? GENERIC_PROTECT_MOVE_DATA : additionalData.Split(',');

                move = new ProtectMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.TryParse(moveAttributes[2], out double protectMovePower) ? protectMovePower : 0.4,
                    moveAttributes[8],
                    int.TryParse(moveAttributes[7], out int protectMoveLevel) ? protectMoveLevel : 0,
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.TryParse(moveAttributes[9], out double protectMoveElixirPoints) ? protectMoveElixirPoints : 5,
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0]),
                    additionalAttributes[1],
                    int.TryParse(additionalAttributes[2], out int protectMoveRate) ? protectMoveRate : 1
                );
                break;
            case "COUNTER":
                move = new CounterMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.TryParse(moveAttributes[2], out double counterMovePower) ? counterMovePower : 0.4,
                    double.TryParse(moveAttributes[4], out double counterMoveAccuracy) ? counterMoveAccuracy : 1.0,
                    moveAttributes[8],
                    int.TryParse(moveAttributes[7], out int counterMoveLevel) ? counterMoveLevel : 0,
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.TryParse(moveAttributes[9], out double counterMoveElixirPoints) ? counterMoveElixirPoints : 5,
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0])
                );
                break;
            case "STAT_CHANGING":
                additionalData = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STAT_CHANGING_MOVE_INDEX], moveAttributes[0]);
                additionalAttributes = (string.IsNullOrEmpty(additionalData) || additionalData.Split(',').Length != STAT_CHANGING_MOVE_DATA_LENGTH) ? GENERIC_STAT_CHANGING_MOVE_DATA : additionalData.Split(',');

                move = new StatChangingMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.TryParse(moveAttributes[2], out double statChangingMovePower) ? statChangingMovePower : 0.4,
                    double.TryParse(moveAttributes[4], out double statChangingMoveAccuracy) ? statChangingMoveAccuracy : 1.0,
                    moveAttributes[8],
                    int.TryParse(moveAttributes[7], out int statChangingMoveLevel) ? statChangingMoveLevel : 0,
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.TryParse(moveAttributes[9], out double statChangingMoveElixirPoints) ? statChangingMoveElixirPoints : 5,
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0]),
                    additionalAttributes[1].Split('~'),
                    Array.ConvertAll(additionalAttributes[2].Split('~'), int.Parse)
                );
                break;
            case "STATUS_CHANGING":
                additionalData = DataRetriever.Instance.GetDataBasedOnID(DataRetriever.Instance.Database[STATUS_CHANGING_MOVE_INDEX], moveAttributes[0]);
                additionalAttributes = (string.IsNullOrEmpty(additionalData) || additionalData.Split(',').Length != STATUS_CHANGING_MOVE_DATA_LENGTH) ? GENERIC_STATUS_CHANGING_MOVE_DATA : additionalData.Split(',');

                move = new StatusChangingMove
                (
                    moveAttributes[0],
                    moveAttributes[1].Replace('~', ','),
                    double.TryParse(moveAttributes[2], out double statusChangingMovePower) ? statusChangingMovePower : 0.4,
                    double.TryParse(moveAttributes[4], out double statusChangingMoveAccuracy) ? statusChangingMoveAccuracy : 1.0,
                    moveAttributes[8],
                    int.TryParse(moveAttributes[7], out int statusChangingMoveLevel) ? statusChangingMoveLevel : 0,
                    Move.ConvertToMoveTarget(moveAttributes[3]),
                    Move.ConvertToMoveType(moveAttributes[5]),
                    double.TryParse(moveAttributes[9], out double statusChangingMoveElixirPoints) ? statusChangingMoveElixirPoints : 5,
                    EffectMaker.Instance.GetEffectsBasedOnName(moveAttributes[0]),
                    StatusCondition.GenerateStatusCondition
                    (
                        additionalAttributes[1],
                        int.TryParse(additionalAttributes[2], out int charmDuration) ? charmDuration : 0,
                        int.TryParse(additionalAttributes[3], out int confuseDuration) ? confuseDuration : 0,
                        int.TryParse(additionalAttributes[4], out int rollAdvantage) ? rollAdvantage : 0,
                        int.TryParse(additionalAttributes[5], out int freezeRate) ? freezeRate : 0,
                        int.TryParse(additionalAttributes[6], out int poisonRate) ? poisonRate : 0,
                        int.TryParse(additionalAttributes[7], out int restrainRate) ? restrainRate : 0,
                        int.TryParse(additionalAttributes[8], out int sleepRate) ? sleepRate : 0,
                        int.TryParse(additionalAttributes[9], out int stunRate) ? stunRate : 0
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