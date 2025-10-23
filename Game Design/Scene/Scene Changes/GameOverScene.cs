using UnityEngine;
using TMPro;

public class GameOverScene : Singleton<GameOverScene>
{
    [SerializeField] private Animator UIAnimator;
    [SerializeField] public TextMeshProUGUI GameOverText;
    [SerializeField] public string NextScene;

    public void Start()
    {
        UIAnimator.Play("game_over_fade");

        //TODO: Test. Delete later
        SetScene("Game Over\n Loser", "Start Scene");
    }

    public void SetScene(string text, string sceneName)
    {
        GameOverText.text = text;
        NextScene = sceneName;
    }

    public void OnOKButtonPressed()
    {
        NextScene ??= BattleSimStatus.SceneName;
        SceneLoader.Instance.LoadScene(NextScene, TransitionType.FADE_TO_BLACK);
    }
}