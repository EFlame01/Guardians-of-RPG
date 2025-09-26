using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SubCutSceneState is a class that extends from the 
/// <c>CutSceneState</c> class. SubCutSceneState
/// nests another <c>CutScene</c> with it's own <c>CutSceneStates</c> inside 
/// of an existing <c>CutScene</c>. This allows for multiple
/// branching endings to a specific <c>CutScene</c>.
/// </summary>
public class SubCutSceneState : CutSceneState
{
    public CutScene CutScene;

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
