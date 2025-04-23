
/// <summary>
/// BattleStateMachine is a class that controls 
/// the flow of the battle by means of the BattleStates
/// </summary>
public class BattleStateMachine
{
    //public variable
    public BattleState CurrentState {get; private set;}

    /// <summary>
    /// Initializes and runs the <c>BattleStateMachine</c>
    /// by setting up the <paramref name="battleState"/>
    /// as the start state.
    /// </summary>
    /// <param name="battleState">the start state</param>
    public void StartState(BattleState battleState)
    {
        CurrentState = battleState;
        CurrentState.Enter();
    }

    /// <summary>
    /// Changes the current state of the <c>BattleStateMachine</c>
    /// to the <paramref name="battleState"/>.
    /// </summary>
    /// <param name="battleState">the next state</param>
    public void ChangeState(BattleState battleState)
    {
        CurrentState.NullNextState();
        CurrentState.Exit();
        StartState(battleState);
    }

    /// <summary>
    /// Ends the current state of the <c>BattleStateMachine</c>
    /// without chaning the state, effectively ending the 
    /// <c>BattleStateMachine</c>.
    /// </summary>
    public void EndStateMachine()
    {
        CurrentState.Exit();
    }
}