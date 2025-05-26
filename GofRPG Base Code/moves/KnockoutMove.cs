
///<summary>
/// KnockoutMove is a class that extends the
/// Move class. KnockoutMoves bypass the
/// defense and health of the target and 
/// reduce thier base hp to 0, knocking them
/// out.
///</summary>
public class KnockoutMove : Move
{
    //Constructor
    public KnockoutMove(string name, string description, double accuracy, string archetypeName, int level, MoveTarget target, MoveType type, double elixirPoints, Effect[] secondaryEffects)
    {
        Name = name;
        Description = description;
        Accuracy = accuracy;
        ArchetypeName = archetypeName;
        Level = level;
        Target = target;
        Type = type;
        EP = elixirPoints;
        SecondaryEffects = secondaryEffects;
    }
    
    ///<summary>
    /// Sets the <paramref name="target"/>'s base hp to 0.
    /// There will be exceptions if the <paramref name="target"/>
    /// has certain abilities that do not allow
    /// for this.
    ///</summary>
    ///<param name="user"> the user of the move. </param>
    ///<param name="target"> the target for the move. </param>
    public override void UseMove(Character user, Character target)
    {
        //TODO: check if target has abilities to counter this.
        base.UseMove(user, target);
        target.BaseStats.SetHp(0);
    }

    public override void UseMove(Character user, Character target, double epMultiplyer)
    {
        UseMove(user, target);
    }
}