using UnityEngine;

///<summary>
/// ProtectMove is a class that extends
/// the Move class. ProtectMoves are moves
/// that sets the character's protection status
/// to protect the character for PHYSICAL,
/// SPECIAL, STATUS, or ALL types of moves.
///</summary>
public class ProtectMove : Move
{
    public string _protectType;
    public double _successionRate;
    public double _tempAccuracy;

    //Constructor
    public ProtectMove(string name, string description, double power, string archetypeName, int level, MoveTarget target, MoveType type, double elixirPoints, Effect[] secondaryEffects, string protectType, double successtionRate)
    {
        Name = name;
        Description = description;
        Power = power;
        Accuracy = 1.0;
        ArchetypeName = archetypeName;
        Level = level;
        Target = target;
        Type = type;
        EP = elixirPoints;
        SecondaryEffects = secondaryEffects;

        _protectType = protectType;
        _successionRate = successtionRate;
        _tempAccuracy = 1.0;
    }

    ///<summary>
    /// Takes the <paramref name="target"/>'s protection
    /// status and sets it based on the _protectionType
    /// of the move. After this, the accurracy will continue
    /// to decrease if the move is successful. If the move is
    /// not successful, the move will become successful the next
    /// time it is used.
    ///</summary>
    ///<param name="user"> the user of the move. </param>
    ///<param name="target"> the target for the move. </param>
    public override void UseMove(Character user, Character target)
    {
        base.UseMove(user, target);
        if(ProtectMoveWork(target))
        {
            target.BattleStatus.SetProtectionStatus(_protectType);
            _tempAccuracy *= _successionRate;
        }
        else
            _tempAccuracy = Accuracy;
    }

    public override void UseMove(Character user, Character target, global::System.Double epMultiplyer)
    {
        UseMove(user, target);
    }

    ///<summary> Resets the TempAccuracy of the move. </summary>
    public override void ResetMove()
    {
        _tempAccuracy = Accuracy;
    }

    /// <summary>
    /// Checks if the protect move worked on the 
    /// <paramref name="character"/>.
    /// </summary>
    /// <param name="character">the character experienceing the protect move.</param>
    /// <returns><c>TRUE</c> if it works. <c>FALSE</c> if it does not work.</returns>
    private bool ProtectMoveWork(Character character)
    {
        double percent = Random.Range(1, 100)/100;
        if(_tempAccuracy < percent)
            return false;
        if(character.BattleStatus.ProtectionStatus != "NONE")
            return false;
        return true;
    }
}