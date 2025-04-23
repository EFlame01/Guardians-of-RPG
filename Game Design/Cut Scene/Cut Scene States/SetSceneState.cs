using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetSceneState : CutSceneState
{
    [SerializeField] public string sceneName;
    [SerializeField] public TransitionType transitionType;

    public override void Enter()
    {
        base.Enter();
        SetStoryFlagsInCutScene();
        SceneLoader.Instance.LoadScene(sceneName, transitionType);
    }
}