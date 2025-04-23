using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{

    [SerializeField] private string _soundEffect;
    private Button _button;

    public void Start()
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
        if(_button != null)
            _button.interactable = GameManager.Instance.EnableNarrationInputs;
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