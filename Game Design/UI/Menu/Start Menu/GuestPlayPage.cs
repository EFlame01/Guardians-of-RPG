using UnityEngine;

public class GuestPlayPage : StartMenuPage
{

    public void OnYesButtonPressed()
    {
        OnBackButtonPressed();
        GameManager.Instance.StartGame();
    }

    public void OnNoButtonPressed()
    {
        OnBackButtonPressed();
    }
}