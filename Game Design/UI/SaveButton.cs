using UnityEngine;
using UnityEngine.UI;

public class SaveButton : ButtonUI
{
    public override void Start()
    {
        UIButton = gameObject.GetComponent<Button>();
        AddButtonSound();
        EnableSaveButton();
    }

    public override void Update()
    {
        // EnableSaveButton();
    }

    private void EnableSaveButton()
    {
        UIButton.interactable = GameManager.Instance.IsPlayingAsUser();
    }
}