using System;


[Serializable]
public class ChapterData 
{
    public string SceneName;
    public string PartName;
    public string ChapterName;
    public string ChapterStoryFlag;

    public ChapterData()
    {
        SceneName = ChapterScene.SceneName;
        PartName = ChapterScene.PartName;
        ChapterName = ChapterScene.ChapterName;
        ChapterStoryFlag = ChapterScene.ChapterStoryFlag;
    }

    public void LoadChapterData()
    {
        ChapterScene.SceneName = SceneName;
        ChapterScene.PartName = PartName;
        ChapterScene.ChapterName = ChapterName;
        ChapterScene.ChapterStoryFlag = ChapterStoryFlag;
    }
}