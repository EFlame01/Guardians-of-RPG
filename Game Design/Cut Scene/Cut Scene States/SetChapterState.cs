using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChapterState : CutSceneState
{
    [SerializeField] public string SceneName;
    [SerializeField] public string PartName;
    [SerializeField] public string ChapterName;
    [SerializeField] public string ChapterStoryFlag;

    private bool start = false;

    public override void Enter()
    {
        base.Enter();
        start = true;
    }

    public override void Update()
    {
        if(start)
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

    private void SetUpChapterScene()
    {
        ChapterScene.SceneName = SceneName;
        ChapterScene.PartName = PartName;
        ChapterScene.ChapterName = ChapterName;
        ChapterScene.ChapterStoryFlag = ChapterStoryFlag;
    }
}
