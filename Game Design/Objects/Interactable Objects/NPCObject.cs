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
    [SerializeField] protected PlayerSprite _npcSprite;
    [SerializeField] private string _characterName;
    [SerializeField] private Sprite _characterSprite;
    [SerializeField] private GameObject _textBoxCharacterObject;
    [SerializeField] private DialogueData _dialogueData;
    [SerializeField] public string npcID;
    [SerializeField] public string[] flags;
    [SerializeField] public bool[] flagValues;
    [SerializeField] public bool isSitting = false;

    private bool _talkedToPlayer;
    private NpcData _npcData;

    public void OnEnable()
    {
        _npcData = NpcDataContainer.GetNpcData(npcID) ?? new NpcData(npcID, transform.position, flags, flagValues);
        
        if(_npcData == null)
            return;
        
        transform.position = _npcData.Position;

        if(flags.Length <= 0 )
            return;

        for(int i = 0; i < flags.Length; i++)
            if(!StoryFlagManager.FlagDictionary[flags[i]].Value == flagValues[i])
                Destroy(gameObject);
    }

    public override void DisplayInputSymbol()
    {
        if(_myLight2D != null)
            _myLight2D.intensity = 0.5f;
    }

    /// <summary>
    /// If can interact, calls the method
    /// TalkToPlayer().
    /// </summary>
    public override void InteractWithObject()
    {
        if(CanInteract && !_talkedToPlayer)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _talkedToPlayer = true;
            if(isSitting)
                StartDialogue();
            else
                StartCoroutine(TalkToPlayer());
        }
    }

    /// <summary>
    /// Turns to player and starts the dialogue.
    /// </summary>
    /// <returns></returns>
    private IEnumerator TalkToPlayer()
    {
        TurnToPlayer();
        yield return new WaitForSeconds(0.2f);
        StartDialogue();
        while(!DialogueManager.Instance.DialogueEnded)
            yield return null;
        
        GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
    }

    /// <summary>
    /// After determining which side the player
    /// is on, turns to the said player.
    /// </summary>
    private void TurnToPlayer()
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
        if(ObjectDetected)
            return;

        if(collider2D.gameObject.tag.Equals("Player"))
            RevealObjectIsInteractable(true);
    }

    public virtual void OnCollisionStay2D(Collision2D collider2D)
    {
        if(!ObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);  
        }

        if(IsThisObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player"))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    public virtual void OnCollisionExit2D(Collision2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            _talkedToPlayer = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}