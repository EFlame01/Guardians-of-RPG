using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SetChapterState is a class that extends from the 
/// <c>CutSceneState</c> class. SetChapterState
/// is responsible for setting the chapter
/// information and the story flags for the ChapterScene
/// </summary>
public class SetChapterState : CutSceneState
{
    //Serialized variables
    public string SceneName;
    public string PartName;
    public string ChapterName;
    public string ChapterStoryFlag;

    //private variables
    private bool start = false;

    public override void Enter()
    {
        base.Enter();
        start = true;
    }

    public override void Update()
    {
        if (start)
        {
            start = false;
            SetUpChapterScene();
            Exit();
        }
        return;
    }

    public override void Exit()
    {
        base.Exit();
    }

    /// <summary>
    /// Adds the chapter information to the
    /// ChapterScene.
    /// </summary>
    private void SetUpChapterScene()
    {
        ChapterScene.SceneName = SceneName;
        ChapterScene.PartName = PartName;
        ChapterScene.ChapterName = ChapterName;
        ChapterScene.ChapterStoryFlag = ChapterStoryFlag;
    }
}
