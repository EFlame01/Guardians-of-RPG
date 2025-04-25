using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ButtonUI is a class that is the 
/// foundation for how all other buttons
/// that extend this class will act.
/// ButtonUI determines whether the button
/// should be interactable in real time, and
/// it determines what sound effect the
/// button should make when it is clicked.
/// </summary>
public class ButtonUI : MonoBehaviour
{

    //Serialize variables
    [SerializeField] private string _soundEffect;
    [SerializeField] private bool _narrationButton = true;

    //protected variables
    protected Button UIButton;

    public virtual void Start()
    {
        UIButton = gameObject.GetComponent<Button>();
        AddButtonSound();
        EnableButton();
    }

    public void Update()
    {
        EnableButton();
    }

    /// <summary>
    /// Checks/sets the button to be enabled
    /// or disabled.
    /// </summary>
    private void EnableButton()
    {
        if(UIButton == null)
            return;
        if(_narrationButton)
            UIButton.interactable = GameManager.Instance.EnableNarrationInputs;
        else
            UIButton.interactable = GameManager.Instance.EnableButtons;
    }

    /// <summary>
    /// Adds sound effect to button
    /// once it is clicked.
    /// </summary>
    private void AddButtonSound()
    {
        UIButton?.onClick.AddListener(() => AudioManager.Instance.PlaySoundEffect(_soundEffect));
    }
}