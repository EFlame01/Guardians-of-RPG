using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCutSceneState : CutSceneState
{
    [SerializeField] public CutScene CutScene;

    public override void Enter()
    {
        base.Enter();
        CutScene.StartCutScene();
    }

    public override void Update()
    {
        base.Update();
        CutScene.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
