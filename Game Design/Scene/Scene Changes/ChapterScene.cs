using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// ChapterScene is a class that displays
/// the information in the ChapterScene
/// </summary>
public class ChapterScene : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds3 = new WaitForSeconds(3f);

    //Serialized varialbes
    [SerializeField] private TextMeshProUGUI PartText;
    [SerializeField] private TextMeshProUGUI ChapterText;

    //public static variables
    public static string SceneName;
    public static string PartName;
    public static string ChapterName;
    public static string ChapterStoryFlag;

    public void Start()
    {
        StartCoroutine(StartChapter());
    }

    /// <summary>
    /// Sets up the information
    /// needed to display the chapter details
    /// in the ChapterScene.
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartChapter()
    {
        PartText.text = PartName;
        ChapterText.text = ChapterName;

        AudioManager.Instance.StopCurrentMusic(false);

        if (ChapterStoryFlag != null)
            Player.Instance().StoryFlagManager.UpdateFlag(ChapterStoryFlag, true);

        yield return _waitForSeconds3;
        SceneLoader.Instance.LoadScene(SceneName, TransitionType.FADE_TO_BLACK);
    }
}