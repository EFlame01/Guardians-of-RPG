using System.Collections;
using UnityEngine;
using Ink.Runtime;

/// <summary>
/// MedicalObject is a class that extends the
/// InteractableObject class. MedicalObject
/// provides the logic for and the ability to
/// use the Medical Center
/// in their inventory.
/// </summary>
public class MedicalObject : InteractableObject
{
    private static WaitForSeconds _waitForSeconds1_5 = new WaitForSeconds(1.5f);
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);

    //public 
    public string _medicalCenterID;
    public PlayerDirection _directionToUseMC;
    public int _numOfUses;
    public int _maxUses;
    public DialogueData _useMCDialogue;
    public DialogueData _cannotUseMCDialogue;
    public DialogueData _usedMCDialogue;
    public Animator animator;

    private bool _useMedicalCenter;
    private Story _story;
    private bool _stopCheckingStoryUpdate;
    private DialogueData _dialogueData;
    private MedicalCenterData _medicalCenterData;

    public void OnEnable()
    {
        _medicalCenterData = MedicalCenterDataContainer.GetMedicalCenterData(_medicalCenterID);
        if (_medicalCenterData == null)
        {
            _medicalCenterData = new MedicalCenterData(_medicalCenterID, _numOfUses, _maxUses);
            MedicalCenterDataContainer.MedicalCenterDataList.Add(_medicalCenterData);
        }
    }

    public override void Update()
    {
        base.Update();

        if (CheckToUpdateStory())
        {
            _stopCheckingStoryUpdate = true;
            StartCoroutine(UpdateStory());
        }
    }

    public override void InteractWithObject()
    {
        if (CanInteract && !_useMedicalCenter)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _useMedicalCenter = true;

            if (_medicalCenterData.NumOfTimesUsed < _medicalCenterData.Limit)
                StartCoroutine(UseMedicalCenter());
            else
                StartCoroutine(DontUseMedicalCenter());
        }
    }

    private IEnumerator UseMedicalCenter()
    {
        PlayDialogue(_useMCDialogue);

        yield return DialogueManager.Instance.WaitUntilDialogueIsOver();

        if (!_stopCheckingStoryUpdate)
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
        yield return _waitForSeconds0_5;

        animator.Play("heal");
        yield return _waitForSeconds1_5;

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
        if (ObjectDetected)
            return;

        if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMC) && GetObjectFacingSide().Equals(_directionToUseMC))
            RevealObjectIsInteractable(true);
    }

    private void OnTriggerStay2D(Collider2D collider2D)
    {
        if (!ObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMC) && GetObjectFacingSide().Equals(_directionToUseMC))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }

        if (IsThisObjectDetected)
        {
            //check if object should be detected
            if (collider2D.gameObject.CompareTag("Player") && PlayerSpawn.PlayerDirection.Equals(_directionToUseMC) && GetObjectFacingSide().Equals(_directionToUseMC))
                RevealObjectIsInteractable(true);
            else
                RevealObjectIsInteractable(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            _useMedicalCenter = false;
            CanInteract = false;
            IsThisObjectDetected = false;
            HideInputSymbol();
        }
    }
}