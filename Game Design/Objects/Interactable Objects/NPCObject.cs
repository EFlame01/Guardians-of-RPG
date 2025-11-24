using System.Collections;
using UnityEngine;

/// <summary>
/// NPCObject is a class that extends the
/// InteractableObject class. NPCObjects
/// allow you to interact with NPCs as they
/// talk to you.
/// 
/// Other features such as assigning quests,
/// and handing rewards will be added later.
/// </summary>
public class NPCObject : InteractableObject, IDialogue
{
    //Serialized variables
    [Header("NPCObject Properties")]
    [SerializeField] protected PlayerSprite _npcSprite;
    [SerializeField] private string _characterName;
    [SerializeField] private Sprite _characterSprite;
    [SerializeField] private GameObject _textBoxCharacterObject;
    [SerializeField] protected DialogueData _dialogueData;
    [SerializeField] private string npc_ID;
    [SerializeField] private string[] flags;
    [SerializeField] private bool[] flagValues;
    [SerializeField] private bool isSitting;

    //protected variables
    protected bool TalkedToPlayer;
    protected NpcData NpcData;

    public void OnEnable()
    {
        //_npcData = NpcDataContainer.GetNpcData(npcID) ?? new NpcData(npcID, transform.position, flags, flagValues);
        NpcData = NpcDataContainer.GetNpcData(npc_ID);
        if (NpcData == null)
            NpcData = new NpcData(npc_ID, transform.position, flags, flagValues);
        else
            transform.position = NpcData.Position;

        //Add NPC to NPC Container
        // NpcDataContainer.NpcDataList.Add(NpcData);
        NpcDataContainer.AddNpcData(NpcData);

        if (NpcData.direction != null)
        {
            switch (NpcData.direction)
            {
                case "UP":
                    _npcSprite.PerformIdleAnimation(PlayerDirection.UP);
                    break;
                case "LEFT":
                    _npcSprite.PerformIdleAnimation(PlayerDirection.LEFT);
                    break;
                case "DOWN":
                    _npcSprite.PerformIdleAnimation(PlayerDirection.DOWN);
                    break;
                case "RIGHT":
                    _npcSprite.PerformIdleAnimation(PlayerDirection.RIGHT);
                    break;
                default:
                    break;
            }
        }

        if (flags.Length <= 0)
            return;

        for (int i = 0; i < flags.Length; i++)
            if (!StoryFlagManager.FlagDictionary[flags[i]].Value == flagValues[i])
                Destroy(gameObject);
    }

    /// <summary>
    /// Display the light surrounding
    /// the character. This signals that
    /// you can interact with the character.
    /// </summary>
    public override void DisplayInputSymbol()
    {
        if (_myLight2D != null)
            _myLight2D.intensity = 0.5f;
    }

    /// <summary>
    /// If can interact, calls the method
    /// TalkToPlayer().
    /// </summary>
    public override void InteractWithObject()
    {
        if (CanInteract && !TalkedToPlayer)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            TalkedToPlayer = true;
            if (isSitting)
                StartDialogue();
            else
                StartCoroutine(TalkToPlayer());
        }
    }

    /// <summary>
    /// Turns to player and starts the dialogue.
    /// </summary>
    /// <returns></returns>
    public IEnumerator TalkToPlayer()
    {
        TurnToPlayer();
        yield return new WaitForSeconds(0.2f);
        StartDialogue();
        //TODO: Updating the PlayerState here is a temp fix.
        //      Find out where PlayerState is changing to NOT_MOVING
        //      Before the NPC talks to the player
        GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
        while (!DialogueManager.Instance.DialogueEnded)
            yield return null;
        GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
    }

    /// <summary>
    /// After determining which side the player
    /// is on, turns to the said player.
    /// </summary>
    public void TurnToPlayer()
    {
        _npcSprite.PerformIdleAnimation(GetCollisionSide());
    }

    /// <summary>
    /// Starts the dialogue for the
    /// NPC to talk and the player
    /// to interact with.
    /// </summary>
    public void StartDialogue()
    {
        TextBoxCharacter textBoxCharacter = Instantiate(_textBoxCharacterObject, transform).GetComponent<TextBoxCharacter>();
        textBoxCharacter.Sprite = _characterSprite;
        textBoxCharacter.CharacterName = _characterName;
        textBoxCharacter.SetUpCharacterTextBox();
        textBoxCharacter.OpenTextBox();
        textBoxCharacter.StartNarration(_dialogueData);
        CheckForInteraction = true;
    }

    public virtual void OnCollisionEnter2D(Collision2D collider2D)
    {
        if (ObjectDetected)
            return;

        if (collider2D.gameObject.CompareTag("Player"))
            RevealObjectIsInteractable(true);
    }

    public virtual void OnCollisionStay2D(Collision2D collider2D)
    {
        if (!ObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if (IsThisObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    public virtual void OnCollisionExit2D(Collision2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            TalkedToPlayer = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}