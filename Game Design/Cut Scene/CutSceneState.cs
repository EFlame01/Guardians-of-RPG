using UnityEngine;
using System;

/// <summary>
/// CutSceneState is a class that acts as the base
/// that encapsulates every other action state.
/// </summary>
public class CutSceneState : MonoBehaviour
{
    [Serializable]
    public struct CutSceneObjectData
    {
        public string Type;
        public int Amount;
        public string Data;
    }

    //serialized variables
    [SerializeField] public float TimeStamp;
    [SerializeField] public bool IsDone;
    [SerializeField] public CutSceneState NextState;
    [SerializeField] public string StoryFlag;
    [SerializeField] public bool StoryFlagValue;
    [SerializeField] public CutSceneObjectData[] CutSceneObjects;

    //protected variables
    protected bool IsActive;

    /// <summary>
    /// Enter is what the <c>CutSceneState</c> will
    /// do once it starts the action state.
    /// </summary>
    public virtual void Enter()
    {
        IsActive = true;
        GiveCutSceneObject();
    }

    /// <summary>
    /// Udpate is what the <c>CutSceneState</c> will
    /// do during the action state. This method is 
    /// called every frame.
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// Exit is what the <c>CutSceneState</c> will
    /// right before it leaves the action state.
    /// </summary>
    public virtual void Exit()
    {
        IsActive = false;
        IsDone = true;
        SetStoryFlagsInCutScene();
    }

    protected void SetStoryFlagsInCutScene()
    {
        if(StoryFlag != null && StoryFlag.Length > 0)
            Player.Instance().StoryFlagManager.UpdateFlag(StoryFlag, StoryFlagValue);
    }

    private void GiveCutSceneObject()
    {
        foreach(CutSceneObjectData c in CutSceneObjects)
        {
            switch(c.Type)
            {
                case "QUEST":
                    Player.Instance().QuestManager.AddQuest(c.Data);
                    break;
                case "QUEST COMPLETE":
                    Player.Instance().QuestManager.MarkQuestCompleted(c.Data);
                    break;
                case "ITEM":
                    Player.Instance().Inventory.AddItem(c.Data, c.Amount);
                    StartCoroutine(AudioManager.Instance.PlaySoundEffect("quest_assigned", true));
                    break;
                case "BITS":
                    Player.Instance().SetBits(Player.Instance().Bits + c.Amount);
                    break;
                default:
                    break;
            }
        }
    }
}