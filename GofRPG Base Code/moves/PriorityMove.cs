using UnityEngine;

///<summary>
/// PriorityMove is a class that extends the Move
/// class. PriorityMoves are moves that hit the
/// target and have the potential to make them 
/// FLINCH. The likelyhood of the target flinches
/// decreases every time the move is used.
///</summary>
public class PriorityMove : Move
{
    public int PriorityLevel {get; private set;}
    private int _stage;
    private int _flinchPercent;

    //Constructor
    public PriorityMove(string name, string description, double power, double accuracy, string archetypeName, int level, MoveTarget target, MoveType type, double elixirPoints, Effect[] secondaryEffects, int priorityLevel)
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

        PriorityLevel = priorityLevel;
        SetFlinchPercent(priorityLevel);
    }

    public override void UseMove(Character user, Character target)
    {
        UseMove(user, target, 1);
    }

    ///<summary>
    /// Damages the <paramref name="target"/>. After damaging the
    /// <paramref name="target"/>, it will then check if the <paramref name="target"/> should
    /// flinch. After this, it will change the likelihood
    /// of flinch working in the future.
    ///</summary>
    ///<param name="user"> the user of the move. </param>
    ///<param name="target"> the target for the move. </param>
    ///<param name="epMultiplyer"> the elixir point multiplyer. </param>
    public override void UseMove(Character user, Character target, double epMultiplyer)
    {
        base.UseMove(user, target, epMultiplyer);
        int dmg = CalculateDamage(user, target, epMultiplyer);
        target.BaseStats.SetHp(target.BaseStats.Hp - dmg);
        if(Flinched())
        {
            if(target.BattleStatus.StatusConditions.ContainsKey("FLINCH"))
                target.BattleStatus.StatusConditions["FLINCH"] = new Flinch();
            else
                target.BattleStatus.StatusConditions.Add("FLINCH", new Flinch());
        }
        _stage++;
        SetFlinchPercent(_stage);
    }

    ///summary>
    /// Resets the Stage and the 
    /// FlinchPercent based on the PriorityLevel
    /// of the move.
    ///</summary>
    public override void ResetMove()
    {
        SetFlinchPercent(PriorityLevel);
    }

    /// <summary>
    /// Checks if the user flinched.
    /// </summary>
    /// <returns><c>TRUE</c> if the user flinched. 
    /// <c>FALSE</c> if the user did not flinch.</returns>
    private bool Flinched()
    {
        int percent = Random.Range(0, 100) + 1;
        
        return _flinchPercent >= percent;
    }

    /// <summary>
    /// Sets the probability of the target
    /// flinching based on which <paramref name="stage"/>
    /// the move is in.
    /// </summary>
    /// <param name="stage">level of flinch</param>
    private void SetFlinchPercent(int stage)
    {
        _stage = Mathf.Clamp(stage, 1, 3);

        _flinchPercent = _stage switch
        {
            1 => Units.FLINCH_STAGE_1,
            2 => Units.FLINCH_STAGE_2,
            3 => Units.FLINCH_STAGE_3,
            _ => Units.FLINCH_STAGE_3,
        };

    }
}