
using UnityEngine.UI;

public class DeleteAccount : StartMenuPage
{
    public Button yesButton;
    public Button noButton;

    public void OnYesButtonPressed()
    {
        OnBackButtonPressed();
    }

    public void OnNoButtonPressed()
    {
        OnBackButtonPressed();
    }
}