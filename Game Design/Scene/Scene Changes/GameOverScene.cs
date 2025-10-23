using UnityEngine;
using TMPro;

public class GameOverScene : Singleton<GameOverScene>
{
    [SerializeField] private Animator UIAnimator;
    [SerializeField] public TextMeshProUGUI GameOverText;
    [SerializeField] public string NextScene;
    [SerializeField] private Vector3 Position = new Vector3(0, 0, 0);

    public static string gameOverText;
    public static string nextScene;
    public static Vector3 playerPosition;

    public void Start()
    {
        UIAnimator.Play("game_over_fade");
        GameOverText.text = gameOverText;
        NextScene = nextScene;
        Position = playerPosition;

        PlayerSpawn.PlayerPosition = Position;
        PlayerSpawn.PlayerDirection = PlayerDirection.DOWN;
    }

    public static void SetScene(string text, string sceneName, Vector3 position)
    {
        gameOverText = text;
        nextScene = sceneName;
        playerPosition = position;
    }

    public void OnOKButtonPressed()
    {
        NextScene ??= BattleSimStatus.SceneName;
        SceneLoader.Instance.LoadScene(NextScene, TransitionType.FADE_TO_BLACK);
    }
}