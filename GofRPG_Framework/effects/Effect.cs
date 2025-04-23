
///<summary>
/// Effect is a class that creates the effects
/// for characters' abilities, moves, and items.
///</summary>
public abstract class Effect
{
    public string Id {get; protected set;}
    public string Name {get; protected set;}
    public EffectOrigin Origin {get; protected set;}
    public EffectType Type {get; protected set;}
    public MoveTarget Target {get; protected set;}
    public double Accuracy {get; protected set;}

    /// <summary>
    /// Uses the effect on the <paramref name="target"/>. The effect
    /// changes on what type it is.
    /// </summary>
    /// <param name="target">the target of the effect</param>
    /// <returns>an array of strings listing the effects that happened.</returns>
    public abstract string[] UseEffect(Character target);
    
    /// <summary>
    /// Gets the <c>EffectOrigin</c> class based on the 
    /// string value <paramref name="origin"/>.
    /// </summary>
    /// <param name="origin">string value of the origin effect</param>
    /// <returns>the <c>EffectOrigin</c> of the effect or <c>null</c> if the origin type does not exist.</returns>
    public static EffectOrigin GetEffectOrigin(string origin)
    {
        return origin switch
        {
            "ABILITY" => EffectOrigin.ABILITY,
            "MOVE" => EffectOrigin.MOVE,
            _ => EffectOrigin.NONE
        };
    }

    /// <summary>
    /// Gets the <c>EffectType</c> class based on the 
    /// string value <paramref name="type"/>.
    /// </summary>
    /// <param name="type">string value of the effect type</param>
    /// <returns>the <c>EffectType</c> of the effect or <c>null</c> if the effect type does not exist.</returns>
    public static EffectType GetEffectType(string type)
    {return type switch
        {
            "ANNOUNCE" => EffectType.ANNOUNCE,
            "HEALTH_BOOST" => EffectType.HEALTH_BOOST,
            "IMMUNITY" => EffectType.IMMUNITY,
            "NEGATION" => EffectType.NEGATION,
            "RECHARGE" => EffectType.RECHARGE,
            "RECOIL" => EffectType.RECOIL,
            "STAB_BOOST" => EffectType.STAB_BOOST,
            "STAT_CHANGE" => EffectType.STAT_CHANGE,
            "STATUS_CONDITION" => EffectType.STATUS_CONDITION,
            _ => EffectType.NONE
        };
    }
}