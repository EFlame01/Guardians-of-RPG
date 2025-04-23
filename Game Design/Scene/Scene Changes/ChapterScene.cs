using System.Collections;
using UnityEngine;
using TMPro;

public class ChapterScene : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI PartText;
    [SerializeField] public TextMeshProUGUI ChapterText;
    public static string SceneName;
    public static string PartName;
    public static string ChapterName;
    public static string ChapterStoryFlag;
    
    // public void Awake()
    // {
    //     PartText.text = PartName;
    //     ChapterText.text = ChapterName;
    // }

    public void Start()
    {
        StartCoroutine(StartChapter());
    }

    public IEnumerator StartChapter()
    {
        PartText.text = PartName;
        ChapterText.text = ChapterName;

        if(ChapterStoryFlag != null)
            Player.Instance().StoryFlagManager.UpdateFlag(ChapterStoryFlag, true);

        yield return new WaitForSeconds(3f);
        SceneLoader.Instance.LoadScene(SceneName, TransitionType.FADE_TO_BLACK);
    }
}