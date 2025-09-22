using UnityEngine;

/// <summary>
/// EndState is a class that extends from the 
/// <c>CutSceneState</c> class. This class simply
/// creates a state that ends immediately after
/// it starts.
/// </summary>
public class EndState : CutSceneState
{

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        Exit();
    }
}