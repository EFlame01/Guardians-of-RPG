using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// QuestMenu is a class that extends the
/// <c>MenuState</c> class. QuestMenu
/// allows you to look at the <c>Player</c>'s entire
/// quest divided into 3 sections:
/// <list type="bullet">
///     <item>INCOMPLETE</item>
///     <item>COMPLETE</item>
///     <item>ALL</item>
/// </list>
/// 
/// </summary>
public class QuestMenu : MenuState
{
    //Serialized variables
    [SerializeField] private GameObject questWidgetPrefab;
    [SerializeField] private Transform questListLayout;
    [SerializeField] private Sprite questCompleteSprite;
    [SerializeField] private Sprite questIncompleteSprite;
    [SerializeField] private Button incompleteButton;

    //private variable
    private string questStatusTab;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        incompleteButton.Select();
        questStatusTab = "INCOMPLETED";
        SetUpQuestLayout();
    }

    /// <summary>
    /// Displays a list of quests depending
    /// the Quest's <paramref name="status"/> (COMPLETE, INCOMPLETE, or both)
    /// </summary>
    /// <param name="status">The status of the <c>Quest</c> you wish to see displayed</param>
    public void OnQuestStatusButtonPressed(string status)
    {
        questStatusTab = status;
        SetUpQuestLayout();
    }

    private void SetUpQuestLayout()
    {
        ClearContents();
        Player player = Player.Instance();
        foreach(KeyValuePair<string, Quest> questInfo in QuestManager.QuestDictionary)
        {
            if(questStatusTab.Equals("ALL"))
                InstantiateQuestWidget(questInfo.Value);
            else if(questStatusTab.Equals("COMPLETED") && questInfo.Value.Completed)
                InstantiateQuestWidget(questInfo.Value);
            else if(questStatusTab.Equals("INCOMPLETED") && !questInfo.Value.Completed)
                InstantiateQuestWidget(questInfo.Value);
        }
    }

    private void InstantiateQuestWidget(Quest quest)
    {
        GameObject questUI = Instantiate(questWidgetPrefab, questListLayout);
        Image questStatusImage = questUI.transform.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI questType = questUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI questDescription = questUI.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        questStatusImage.sprite = quest.Completed ? questCompleteSprite : questIncompleteSprite;
        questType.text = quest.Type;
        questDescription.text = quest.Description;
    }

    private void ClearContents()
    {
        foreach(Transform child in questListLayout)
        {
            Destroy(child.gameObject);
        }
    }
}
