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
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    private static WaitForSeconds _waitForSeconds0_25 = new WaitForSeconds(0.25f);
    private static WaitForSeconds _waitForSeconds0_01 = new WaitForSeconds(0.01f);
    [SerializeField] protected string TextBoxName;
    public TextMeshProUGUI TextMeshComponent;
    public Scrollbar ScrollBar;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected bool PlayOnStart;
    [SerializeField] protected bool DestroyTextBox = true;
    [SerializeField] private InputActionReference Select;

    public bool IsClosed { get; private set; }

    private DialogueData _dialogueData;
    private bool _textBoxOpened;

    public void OnEnable()
    {
        ResetScrollBar();
    }

    public virtual void Start()
    {
        IsClosed = true;
        if (PlayOnStart)
            OpenTextBox();
    }

    public void Update()
    {
        if (Select.action.ReadValue<float>() <= 0f)
            return;
        if (!_textBoxOpened)
            return;
        if (GameManager.Instance.EnableNarrationInputs)
            OnContinueButtonPressed();
    }

    /// <summary>
    /// Sets the game state to the start state,
    /// animate the text box start up, and begins
    /// the dialogue sequence.
    /// </summary>
    public virtual void OpenTextBox()
    {
        //Method placed here to always reset scroll bar before text box starts dialogue
        ResetScrollBar();

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
        try
        {
            _textBoxOpened = false;
            StartCoroutine(EndTextBoxAnimation());
            IsClosed = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    /// <summary>
    /// Starts the animation to open the text box
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator StartTextBoxAnimation()
    {
        //TODO: testing if placing ResetScrollBar in a different spot is better.
        //Method placed here to always reset scroll bar after opening text box
        // ResetScrollBar();

        if (TextMeshComponent != null)
            TextMeshComponent.text = "";

        yield return _waitForSeconds0_01;

        AudioManager.Instance.PlaySoundEffect("open_01");
        Animator.Play(TextBoxName + "_open");
        yield return _waitForSeconds0_25;
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
        yield return _waitForSeconds0_5;
        if (DestroyTextBox)
            Destroy(gameObject);
    }

    public void ResetScrollBar()
    {
        try
        {
            ScrollBar.value = 1;
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARNING from ResetScrollBar(): " + e.Message);
        }
    }

    protected void ContinueNarration()
    {
        if (!DialogueManager.Instance.DialogueContinued)
        {
            DialogueManager.Instance.DisplayNextDialogue(_dialogueData);
        }
    }
}