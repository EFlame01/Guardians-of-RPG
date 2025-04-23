using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] Button menuButton;
    [SerializeField] GameObject menuObject;
    private bool _canBePressed;

    public void Update()
    {
        _canBePressed = CanBePressed();
        menuButton.interactable = _canBePressed;
    }

    public void OnPressed()
    {
        if(_canBePressed)
        {
            Instantiate(menuObject, null);
            GameManager.Instance.PlayerState = PlayerState.PAUSED;
        }
    }

    public bool CanBePressed()
    {
        return GameManager.Instance.PlayerState switch
        {
            PlayerState.MOVING => true,
            PlayerState.NOT_MOVING => true,
            PlayerState.PAUSED => false,
            PlayerState.CANNOT_MOVE => false,
            PlayerState.CUT_SCENE => false,
            PlayerState.TRANSITION => false,
            PlayerState.INTERACTING_WITH_OBJECT => false,
            _ => false,
        };
    }
}