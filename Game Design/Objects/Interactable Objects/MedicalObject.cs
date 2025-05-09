using System.Collections;
using UnityEngine;

public class MedicalObject : InteractableObject
{
    [SerializeField] public string _medicalCenterID;
    [SerializeField] public PlayerDirection _directionToUseMC;
    [SerializeField] public int _numOfUses;
    [SerializeField] public int _maxUses;
    [SerializeField] public DialogueData _useMCDialogue;
    [SerializeField] public DialogueData _cannotUseMCDialogue;

    private bool _useMedicalCenter;
    private Story _story;
    private DialogueData _dialogueData;
    private MedicalCenterData _medicalCenterData;
    private int _numHeals;

    public void OnEnable()
    {
        _medicalCenterData = MedicalDataContainer.GetMedicalCenterData(_medicalCenterID);
        //TEST - Delete later
        if(_medicalCenterData == null)
        {
            _medicalCenterData = new MedicalCenterData(_medicalCenterID, _numOfUses, _maxUses);
            MedicalCenterDataContainer.MedicalCenterDataList.Add(_medicalCenterData);
        }
        //end of TEST
        _numHeals = _medicalCenterData == null ? _maxUses : _medicalCenterData.Limit;
    }

    public override void InteractWithObject()
    {
        if(CanInteract && !_useMedicalCenter && _medicalCenterData.NumOfTimesUsed < _medicalCenterData.Limit)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _useMedicalCenter = true;
            StartCoroutine(UseMedicalCenter());
        }
        else if(CanInteract && !_useMedicalCenter && _medicalCenterData.NumOfTimesUsed >= _medicalCenterData.Limit)
        {
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            _useMedicalCenter = true;
            DontUseMedicalCenter();
        }
    }

    private void UseMedicalCenter()
    {
        Debug.Log("MEDICAL SHOP IN USE");
        //TODO: Ask if they want to use the medical center.
        //      If they want to use the medical center, heal
        //      display another message that they have been healed
        _dialogueData = _useMCDialogue;
        DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
    }

    private void DontUseMedicalCenter()
    {
        Debug.Log("MEDICAL SHOP NOT IN USE");
        //TODO: Tell them that they cannot use the medical center because they
        //      exceeded the amount of times they can use it.
        _dialogueData = _cannotUseMCDialogue;
        DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
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