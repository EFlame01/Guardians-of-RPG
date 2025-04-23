using UnityEngine;

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