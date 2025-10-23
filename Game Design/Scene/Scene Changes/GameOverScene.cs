using UnityEngine;
using TMPro;

public class GameOverScene : Singleton<GameOverScene>
{
    [SerializeField] private Animator UIAnimator;
    [SerializeField] public TextMeshProUGUI GameOverText;
    [SerializeField] public string NextScene;
    [SerializeField] private Vector3 Position = new Vector3(0, 0, 0);

    public void Start()
    {
        UIAnimator.Play("game_over_fade");

        //TODO: Test. Delete later
        // SetScene("Game Over\n Loser", "Start Scene");
    }

    public void SetScene(string text, string sceneName, Vector3 position)
    {
        GameOverText.text = text;
        NextScene = sceneName;
        Position = position;
    }

    public void OnOKButtonPressed()
    {
        NextScene ??= BattleSimStatus.SceneName;
        SceneLoader.Instance.LoadScene(NextScene, TransitionType.FADE_TO_BLACK);
    }
}