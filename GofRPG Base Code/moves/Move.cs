using UnityEngine;

///<summary>
/// Move is an abstract class that holds
/// all the basic information for each 
/// type of move.
///</summary>
public class Move
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public double Power { get; protected set; }
    public double Accuracy { get; protected set; }
    public string ArchetypeName { get; protected set; }
    public int Level { get; protected set; }
    public MoveTarget Target { get; protected set; }
    public MoveType Type { get; protected set; }
    public double EP { get; protected set; }
    public Effect[] SecondaryEffects { get; protected set; }

    ///<summary>
    /// Let's the <paramref name="user"/> perform
    /// said move on the <paramref name="target"/>.
    /// The move will vary depending on the type of move.
    ///</summary>
    ///<param name="user"> the user of the move.</param>
    ///<param name="target"> the target for the move.</param>
    public virtual void UseMove(Character user, Character target)
    {
        user.BaseStats.SetElx(user.BaseStats.Elx - (int)EP);
        return;
    }
    public virtual void UseMove(Character user, Character target, double epMultiplyer)
    {
        user.BaseStats.SetElx(user.BaseStats.Elx - (int)(EP * epMultiplyer));
        return;
    }

    ///<summary> Resets any value or status of a move. </summary>
    public virtual void ResetMove()
    {
        return;
    }

    ///<summary>
    /// Determines if the <paramref name="user"/> was
    /// able to hit <paramref name="target"/> with said move.
    ///</summary>
    ///<param name="user"> the user of the move. </param>
    ///<param name="target"> the target for said move. </param>
    ///<returns> <c>TRUE</c> if the move hit. <c>FALSE</c> if the move missed </returns>
    public bool DidMoveHit(Character user, Character target)
    {
        int percent = Random.Range(0, 100) + 1;
        double chanceToHit = Accuracy * user.BaseStats.Acc;
        double chanceOfNotDodging = 1 - (double)target.BaseStats.Eva / (double)target.BaseStats.GetBaseStatTotal();
        double CTH_PLUS_COND = chanceToHit + chanceOfNotDodging;
        double CTH_TIMES_COND = chanceToHit * chanceOfNotDodging;

        /*
            DidMoveHit = P(chanceToHit OR chanceOfNotDoding)
            DidMoveHit = P(chanceToHit) + P(chanceOfNotDodging) - P(chanceToHit AND chanceOfNotDodging)
            DidMoveHit = (chanceToHit + chanceOfNotDodging) - (chanceToHit * chanceOfNotDodging)
        */
        int didMoveHit = (int)((CTH_PLUS_COND - CTH_TIMES_COND) * 100.0);

        return didMoveHit >= percent;
    }

    ///<summary>
    /// Calculates the damage of a move based on the <paramref name="user"/>'s
    /// attack, the <paramref name="target"/>'s defense, whether the move
    /// is of the same archetype of the <paramref name="user"/>, the <paramref name="user"/>'s 
    /// abilities, the <paramref name="target"/>'s abilities, and the <paramref name="user"/>'s
    /// chance of landing a critical hit.
    ///</summary>
    ///<param name="user"> the user of the move.</param>
    ///<param name="target"> the target for the move.</param>
    ///<returns> The calculated damage for said move.</returns>
    protected int CalculateDamage(Character user, Character target, double epMultiplyer)
    {
        double critPercent = (double)((Random.Range(0, 100) + 1) / 100.0);
        int userAtk = user.BaseStats.Atk;
        int targetDef = target.BaseStats.Def;
        int dmg;
        double newPower = Power;
        double chanceOfCrit = user.BaseStats.Crt;
        int rollDef = Random.Range(1, 20) + 1;

        if (user.Archetype.ClassName == ArchetypeName || user.Archetype.ArchetypeName == ArchetypeName)
            newPower += Power * Units.STAB_DMG;
        if (chanceOfCrit >= critPercent)
            newPower += Mathf.Ceil((float)(Power * Units.CRIT_DMG));
        if (epMultiplyer > 0)
            newPower += Power * epMultiplyer;
        
        if(rollDef <= 5)
            targetDef -= (int)(Mathf.Ceil((float)(Power * Units.CRIT_DMG)));
        if(rollDef <= 10)
            targetDef = 0;
        if(rollDef <= 15)
            targetDef += (int)(Mathf.Ceil((float)(Power * Units.CRIT_DMG)));
        if(rollDef == 20)
            targetDef *= 2;

        userAtk = (int)(userAtk * newPower);
        dmg = Mathf.Clamp(userAtk - targetDef, 1, userAtk);
        return dmg;
    }

    /// <summary>
    /// Converts the string parameter <paramref name="moveType"/>
    /// into a <c>MoveType</c> enunm.
    /// </summary>
    /// <param name="moveType">the string value of the move type</param>
    /// <returns>a <c>MoveType</c> enum</returns>
    public static MoveType ConvertToMoveType(string moveType)
    {
        return moveType switch
        {
            "COUNTER" => MoveType.COUNTER,
            "HEALING" => MoveType.HEALING,
            "KNOCK_OUT" => MoveType.KNOCK_OUT,
            "PRIORITY" => MoveType.PRIORITY,
            "PROTECT" => MoveType.PROTECT,
            "REGULAR" => MoveType.REGULAR,
            "STAT_CHANGING" => MoveType.STAT_CHANGING,
            "STATUS_CHANGING" => MoveType.STATUS_CHANGING,
            _ => MoveType.NONE
        };

    }

    /// <summary>
    /// Converts the string parameter <paramref name="moveTarget"/>
    /// into a <c>MoveTarget</c> enunm.
    /// </summary>
    /// <param name="moveTarget">the string value of the move type</param>
    /// <returns>a <c>MoveTarget</c> enum</returns>
    public static MoveTarget ConvertToMoveTarget(string moveTarget)
    {
        return moveTarget switch
        {
            "USER" => MoveTarget.USER,
            "ENEMY" => MoveTarget.ENEMY,
            "ALL_ENEMIES" => MoveTarget.ALL_ENEMIES,
            "ALLY" => MoveTarget.ALLY,
            "ALL_ALLIES" => MoveTarget.ALL_ALLIES,
            "ALLY_SIDE" => MoveTarget.ALLY_SIDE,
            "EVERYONE" => MoveTarget.EVERYONE,
            _ => MoveTarget.USER
        };

    }
}