using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

/// <summary>
/// TextBox is a class that extends the Dialogue
/// class. TextBox is the class that holds the
/// UI information needed to interface with the
/// game for narration and dialogue
/// </summary>
public class TextBox : MonoBehaviour
{
    [SerializeField] protected string TextBoxName;
    [SerializeField] public TextMeshProUGUI TextMeshComponent;
    [SerializeField] public Scrollbar ScrollBar;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected bool PlayOnStart;
    [SerializeField] protected bool DestroyTextBox = true;
    [SerializeField] private InputActionReference Select;

    public bool IsClosed {get; private set;}

    private DialogueData _dialogueData;
    private bool _textBoxOpened;

    public virtual void Start()
    {
        IsClosed = true;
        if(PlayOnStart)
            OpenTextBox();
    }

    public void Update()
    {
        if(Select.action.ReadValue<float>() <= 0f)
            return;
        if(!_textBoxOpened)
            return;
        if(GameManager.Instance.EnableNarrationInputs)
            OnContinueButtonPressed();
    }
    
    /// <summary>
    /// Sets the game state to the start state,
    /// animate the text box start up, and begins
    /// the dialogue sequence.
    /// </summary>
    public virtual void OpenTextBox()
    {
        //start narraiton naimation and dialogue
        StartCoroutine(StartTextBoxAnimation());
        IsClosed = false;
    }

    /// <summary>
    /// Calls the ContinueDialogue method.
    /// </summary>
    public void OnContinueButtonPressed()
    {
        ContinueNarration();
    }

    public void StartNarration(DialogueData dialogueData)
    {
        _dialogueData = dialogueData;
        DialogueManager.Instance.SetTextBox(this);
        DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
    }

    /// <summary>
    /// Animates the text box to disappear, and 
    /// sets the game state to the end state
    /// </summary>
    public void EndNarration()
    {
        _textBoxOpened = false;
        StartCoroutine(EndTextBoxAnimation());
        IsClosed = true;
    }

    /// <summary>
    /// Starts the animation to open the text box
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator StartTextBoxAnimation()
    {
        try{
            TextMeshComponent.text = "";
        } catch(Exception e){
            Debug.LogWarning("TextBox does not have a TextMeshPro component: " + e.Message);
        }
        yield return new WaitForSeconds(0.01f);

        //Method placed here to always reset scroll bar after opening text box
        ResetScrollBar();

        AudioManager.Instance.PlaySoundEffect("open_01");
        Animator.Play(TextBoxName + "_open");
        yield return new WaitForSeconds(0.25f);
        _textBoxOpened = true;
    }

    /// <summary>
    /// Starts the animation to close the text box
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator EndTextBoxAnimation()
    {
        AudioManager.Instance.PlaySoundEffect("close_04");
        Animator.Play(TextBoxName + "_close");
        yield return new WaitForSeconds(0.5f);
        if(DestroyTextBox)
            Destroy(gameObject);
    }

    public void ResetScrollBar()
    {
        try{
            ScrollBar.value = 1;
        }catch(Exception e)
        {
            Debug.LogWarning("Error from ResetScrollBar(): " + e.Message);
        }
    }

    protected void ContinueNarration()
    {
        if(!DialogueManager.Instance.DialogueContinued)
        {
            DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
        }
    }
}