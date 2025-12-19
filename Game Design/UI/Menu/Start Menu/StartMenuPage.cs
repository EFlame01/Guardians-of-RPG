using UnityEngine;
using UnityEngine.UI;

public class StartMenuPage : MonoBehaviour
{
    [SerializeField] Animator animator;
    private string menuName = "options_settings";

    public void OnBackButtonPressed()
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.CLOSE_UI_4);
        animator.Play(menuName + "_close");
        Destroy(gameObject, 0.5f);
    }

    public void SetUsernameText(string username)
    {
        StartScene.Username = username;
        StartScene.UpdatedUsername = true;
    }
}
