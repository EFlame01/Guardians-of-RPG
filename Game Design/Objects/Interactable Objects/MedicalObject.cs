using System.Collections;
using UnityEngine;
using Ink.Runtime;

public class MedicalObject : InteractableObject
{
    [SerializeField] public string _medicalCenterID;
    [SerializeField] public PlayerDirection _directionToUseMC;
    [SerializeField] public int _numOfUses;
    [SerializeField] public int _maxUses;
    [SerializeField] public DialogueData _useMCDialogue;
    [SerializeField] public DialogueData _cannotUseMCDialogue;
    [SerializeField] public DialogueData _usedMCDialogue;
    [SerializeField] public Animator animator;

    private bool _useMedicalCenter;
    private Story _story;
    private bool _stopCheckingStoryUpdate;
    private DialogueData _dialogueData;
    private MedicalCenterData _medicalCenterData;
    private int _numHeals;

    public void OnEnable()
    {
        _medicalCenterData = MedicalCenterDataContainer.GetMedicalCenterData(_medicalCenterID);
        if(_medicalCenterData == null)
        {
            _medicalCenterData = new MedicalCenterData(_medicalCenterID, _numOfUses, _maxUses);
            MedicalCenterDataContainer.MedicalCenterDataList.Add(_medicalCenterData);
        }
        _numHeals = _medicalCenterData == null ? _maxUses : _medicalCenterData.Limit;
    }

    public override void Update()
    {
        base.Update();

        if(CheckToUpdateStory())
        {
            _stopCheckingStoryUpdate = true;
            StartCoroutine(UpdateStory());
        }
    }

    public override void InteractWithObject()
    {
        if(CanInteract && !_useMedicalCenter)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _useMedicalCenter = true;

            if(_medicalCenterData.NumOfTimesUsed < _medicalCenterData.Limit)
                StartCoroutine(UseMedicalCenter());
            else
                StartCoroutine(DontUseMedicalCenter());
        }
    }

    private IEnumerator UseMedicalCenter()
    {
        PlayDialogue(_useMCDialogue);
        
        yield return DialogueManager.Instance.WaitUntilDialogueIsOver();

        if(!_stopCheckingStoryUpdate)
            EndMedicalCare();
    }

    private IEnumerator DontUseMedicalCenter()
    {
        PlayDialogue(_cannotUseMCDialogue);
        
        yield return DialogueManager.Instance.WaitUntilDialogueIsOver();
        
        EndMedicalCare();
    }

    private IEnumerator UpdateStory()
    {
        yield return DialogueManager.Instance.WaitUntilDialogueIsOver();
            
        yield return HealPlayer();
    }

    private IEnumerator HealPlayer()
    {
        RestorePlayerHealth();
        yield return new WaitForSeconds(0.5f);

        animator.Play("heal");
        yield return new WaitForSeconds(1.5f);

        PlayDialogue(_usedMCDialogue);

        yield return DialogueManager.Instance.WaitUntilDialogueIsOver();
        
        yield return new WaitForSeconds(0.5f);

        EndMedicalCare();
    }

    private void EndMedicalCare()
    {
        GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
        _stopCheckingStoryUpdate = false;
    }

    private void PlayDialogue(DialogueData dialogueData)
    {
        _dialogueData = dialogueData;
        DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
    }

    private bool CheckToUpdateStory()
    {
        _story = DialogueManager.Instance.CurrentStory;

        return (
            _story != null && 
            _story.variablesState["acceptsMedicalHelp"] != null && 
            (bool)_story.variablesState["acceptsMedicalHelp"] && 
            !_stopCheckingStoryUpdate
        );
    }

    private void RestorePlayerHealth()
    {
        Player player = Player.Instance();
        player.BaseStats.ResetStats();
        player.BaseStats.SetHp(player.BaseStats.FullHp);
        _medicalCenterData.NumOfTimesUsed++;
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(ObjectDetected)
            return;

        if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMC) && GetObjectFacingSide().Equals(_directionToUseMC))
            RevealObjectIsInteractable(true);
    }
    
    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if(!ObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMC) && GetObjectFacingSide().Equals(_directionToUseMC))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if(IsThisObjectDetected)
        {
            //check if object should be detected
            if(collider2D.gameObject.tag.Equals("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMC) && GetObjectFacingSide().Equals(_directionToUseMC))
                RevealObjectIsInteractable(true);  
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.tag.Equals("Player"))
        {
            _useMedicalCenter = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}