using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SetSceneState is a class that extends from the 
/// <c>CutSceneState</c> class. SetSceneState
/// is responsible for transitioning the player
/// to the next scene.
/// </summary>
public class SetSceneState : CutSceneState
{
    //Serialized variables
    [SerializeField] public string sceneName;
    [SerializeField] public TransitionType transitionType;

    public override void Enter()
    {
        base.Enter();
        SetStoryFlagsInCutScene();
        SceneLoader.Instance.LoadScene(sceneName, transitionType);
    }
}