using UnityEngine;

///<summary>
/// StatusChangingMove is a class that
/// extends the Move class. StatusChangingMoves
/// inflict status conditions on the target.
///</summary>
public class StatusChangingMove : Move
{
    public StatusCondition _statusCondition;

    //Constructor
    public StatusChangingMove(string name, string description, double power, double accuracy, string archetypeName, int level, MoveTarget target, MoveType type,  double elixirPoints, Effect[] secondaryEffects, StatusCondition statusCondition)
    {
        Name = name;
        Description = description;
        Power = power;
        Accuracy = accuracy;
        ArchetypeName = archetypeName;
        Level = level;
        Target = target;
        Type = type;
        EP = elixirPoints;
        SecondaryEffects = secondaryEffects;

        _statusCondition = statusCondition;
    }
    
    ///<summary>
    /// Loops though the list of status conditions
    /// and applies them to the <paramref name="target"/>. If the status condition
    /// is not compatible with what the <paramref name="target"/> already has,
    /// then it will not stack.
    ///</summary>
    ///<param name="user"> the user of the move. </param>
    ///<param name="target"> the target for the move. </param>
    public override void UseMove(Character user, Character target)
    {
        base.UseMove(user, target);
        if(StatusCondition.CanStackStatusCondition(target, _statusCondition.Name))
            target.BattleStatus.StatusConditions.Add(_statusCondition.Name, _statusCondition);
    }

    public override void UseMove(Character user, Character target, double epMultiplyer)
    {
        UseMove(user, target);
    }
}