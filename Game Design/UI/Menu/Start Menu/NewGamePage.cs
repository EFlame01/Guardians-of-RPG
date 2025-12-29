
public class NewGamePage : StartMenuPage
{

    public void OnYesButtonPressed()
    {
        OnBackButtonPressed();
        GameManager.Instance.ResetGameData();
        GameManager.Instance.StartGame();
    }

    public void OnNoButtonPressed()
    {
        OnBackButtonPressed();
    }
}