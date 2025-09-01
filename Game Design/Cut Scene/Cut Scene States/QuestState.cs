using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class QuestState : CutSceneState, IDialogue
{

    [SerializeField] public string questID;
    [SerializeField] public bool assigned;
    [SerializeField] public bool completed;
    [SerializeField] protected TextBox TextBoxPrefab;
    [SerializeField] protected Transform Transform;
    [SerializeField] protected DialogueData QuestAssignDialogueData;
    [SerializeField] protected DialogueData QuestCompleteDialogueData;

    private DialogueData _dialogueData;

    public override void Enter()
    {
        base.Enter();
        if(completed)
            CompletedQuest();
        else if(assigned)
            AssignQuest();
    }

    public override void Update()
    {
        CheckDialogue();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void CheckDialogue()
    {
        if(IsActive && DialogueManager.Instance.DialogueEnded)
            Exit();
    }

    private void CompletedQuest()
    {
        Player player = Player.Instance();
        bool questCompleted = player.QuestManager.MarkQuestCompleted(questID);
        if (questCompleted)
        {
            _dialogueData = QuestCompleteDialogueData;
            DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);

            //TODO: determine more rewards for completing quests besides XP
            int xp = Level.DetermineXPForQuest();
            Level.GainXP(xp);
            if (xp > 0)
                DialogueManager.Instance.CurrentStory.variablesState["reward"] = xp + "XP";

            if (Level.CanLevelUp())
            {
                Level.LevelUpPlayer();
                DialogueManager.Instance.CurrentStory.variablesState["levelUpText"] = "You are now level " + Player.Instance().Level;
            }

            Move[] moves = Level.DetermineLearnedMoves();
            if (moves != null && moves.Length > 0)
            {
                string movesLearned = "You learned ";

                if (moves.Length == 1)
                {
                    movesLearned += moves[0].Name;
                    Player.Instance().MoveManager.AddMove(moves[0].Name);
                }
                else
                {
                    for (int i = 0; i < moves.Length; i++)
                    {
                        Player.Instance().MoveManager.AddMove(moves[i].Name);

                        if (i + 1 >= moves.Length)
                            movesLearned += "and " + moves[i].Name;

                        else
                            movesLearned += moves[i] + ", ";
                    }
                }

                DialogueManager.Instance.CurrentStory.variablesState["levelUp"] = true;
                DialogueManager.Instance.CurrentStory.variablesState["movesLearnedText"] = movesLearned;
                DialogueManager.Instance.CurrentStory.variablesState["whereToFindMovesText"] = "You can find the moves you learned in your Move Set list.";
            }
            else
                DialogueManager.Instance.CurrentStory.variablesState["levelUp"] = false;
        }
        else
            Debug.LogWarning("QuestID " + questID + " does not exist or was not assigned to player...");

        StartCoroutine(AudioManager.Instance.PlaySoundEffect("quest_completed"), true);
        StartDialogue();
    }

    private void AssignQuest()
    {
        Player player = Player.Instance();
        bool questAssigned = player.QuestManager.AddQuest(questID);
        if(questAssigned)
        {
            Quest quest = QuestManager.QuestDictionary[questID];
            _dialogueData = QuestAssignDialogueData;
            DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);
            DialogueManager.Instance.CurrentStory.variablesState["assignment"] = quest.Category.Equals("TODO") ? "objective" : "Quest Card";
            DialogueManager.Instance.CurrentStory.variablesState["quest"] = QuestManager.QuestDictionary[questID].Description;

        }
        else
            Debug.LogWarning("QuestID " + questID + " does not exist. Could not be assigned to player");

        StartCoroutine(AudioManager.Instance.PlaySoundEffect("quest_assigned", true));
        StartDialogue(); 
    }

    public virtual void StartDialogue()
    {
        try {
            if(Transform != null)
                TextBoxPrefab.gameObject.transform.SetParent(Transform);
            
            TextBoxPrefab.gameObject.SetActive(true);
            TextBoxPrefab.OpenTextBox();
            TextBoxPrefab.StartNarration(_dialogueData);
        } catch(Exception e){
            Debug.LogWarning("Error with TextBoxPrefab: " + e.Message);
            DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
        }
    }
}
