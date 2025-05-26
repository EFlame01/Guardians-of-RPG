
///<summary>
/// CounterMove is a class that extends
/// the Move class. CounterMoves are moves
/// that sets up a counter. This allows incoming physical
/// attacks to be reflected back at the opponent.
///</summary>
public class CounterMove : Move
{
    //Constructor
    public CounterMove(string name, string description, double power, double accuracy, string archetypeName, int level, MoveType type, double elixirPoints, Effect[] secondaryEffects)
    {
        Name = name;
        Description = description;
        Power = power;
        Accuracy = accuracy;
        ArchetypeName = archetypeName;
        Level = level;
        Target = MoveTarget.USER;
        Type = type;
        EP = elixirPoints;
        SecondaryEffects = secondaryEffects;
    }

    ///<summary>
    /// Sets the <paramref name="target"/> 
    /// (in this case the <paramref name="user"/>'s)
    /// protection status to 'COUNTER'
    ///</summary>
    ///<param name="user">the user of the move.</param>
    ///<param name="target">the target for the move.</param>
    public override void UseMove(Character user, Character target)
    {
        base.UseMove(user, target);
        if(target.BattleStatus.ProtectionStatus == "NONE")
            target.BattleStatus.SetProtectionStatus("COUNTER");
    }

    public override void UseMove(Character user, Character target, double epMultiplyer)
    {
        UseMove(user, target);
    }
}