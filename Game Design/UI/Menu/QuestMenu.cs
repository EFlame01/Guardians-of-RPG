using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestMenu : MenuState
{
    [SerializeField] private GameObject questWidgetPrefab;
    [SerializeField] private Transform questListLayout;
    [SerializeField] private Sprite questCompleteSprite;
    [SerializeField] private Sprite questIncompleteSprite;

    private string questStatusTab;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        questStatusTab = "COMPLETED";
        SetUpQuestLayout();
    }

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
