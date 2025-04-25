using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{

    [SerializeField] private string _soundEffect;
    [SerializeField] private bool _narrationButton = true;
    protected Button _button;

    public virtual void Start()
    {
        _button = gameObject.GetComponent<Button>();
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
        if(_button == null)
            return;
        if(_narrationButton)
            _button.interactable = GameManager.Instance.EnableNarrationInputs;
        else
            _button.interactable = GameManager.Instance.EnableButtons;
    }

    /// <summary>
    /// Adds sound effect to button
    /// once it is clicked.
    /// </summary>
    private void AddButtonSound()
    {
        if(_button != null)
            _button.onClick.AddListener(() => AudioManager.Instance.PlaySoundEffect(_soundEffect));
    }
}