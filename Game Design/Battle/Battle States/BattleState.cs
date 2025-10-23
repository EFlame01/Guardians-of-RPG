
/// <summary>
/// Battle State is an abstract class that
/// forms the backbone for the rest of the 
/// BattleStates for the <c>BattleStateMachine</c>
/// and <c>BattleSimulator</c>.
/// </summary>
public abstract class BattleState
{
    //public variables
    public string PrevState { get; protected set; }
    public string NextState { get; protected set; }
    public string CurrentState { get; protected set; }

    protected bool _roundOver;

    //public abstract variables
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

    public void SetPrevState(string prevState)
    {
        PrevState = prevState;
    }

    /// <summary>
    /// Sets the NextState variable to null.
    /// </summary>
    public void NullNextState()
    {
        NextState = null;
    }

    /// <summary>
    /// Sets the PrevState variabhle to null.
    /// </summary>
    public void NullPrevState()
    {
        PrevState = null;
    }

    /// <summary>
    /// Checks if the battle is over.
    /// </summary>
    /// <returns><c>TRUE</c> if the battle is over, <c>FALSE</c> if otherwise</returns>
    public bool BattleOver()
    {
        if (BattleSimStatus.Enemies.Count == 0)
            return true;
        if (BattleSimStatus.Allies.Count == 0 && Player.Instance().BaseStats.Hp == 0)
            return true;

        return false;
    }

    /// <summary>
    /// Checks the winner of the battle.
    /// </summary>
    /// <returns><c>"PLAYER"</c> if the player won, <c>"ENEMY"</c> if the player lost. (May also return <c>"NO ONE"</c>).</returns>
    public string Winner()
    {
        if (BattleSimStatus.Enemies.Count == 0)
            return "PLAYER";
        if (BattleSimStatus.Allies.Count == 0 && Player.Instance().BaseStats.Hp == 0)
            return "ENEMY";

        return "NO ONE";
    }

    protected bool RoundOver()
    {
        _roundOver = BattleSimStatus.BattleQueue.Count == 0;
        return _roundOver;
    }
}