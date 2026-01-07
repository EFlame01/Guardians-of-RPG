using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// OptionsMenu is a class that extends the
/// <c>MenuState</c> class. OptionsMenu
/// allows you to change certain aspects of,
/// as well as save and exit the game.
/// </summary>
public class OptionsMenu : MenuState
{
    //Serialize variables
    [SerializeField] private SliderBar volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValueText;
    [SerializeField] private GameObject sfxOn;
    [SerializeField] private GameObject sfxOff;
    [SerializeField] private GameObject touchPadOn;
    [SerializeField] private GameObject touchPadOff;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetUpOptions();
    }

    /// <summary>
    /// Changes the volume dynamically
    /// in game.
    /// </summary>
    public void OnVolumeSliderChange()
    {
        int volumeVal = (int)(volumeSlider.slider.value);
        volumeValueText.text = volumeVal.ToString();
        GameManager.Instance.GameVolume = (float)(volumeVal / 100f);
        AudioManager.Instance.AdjustVolume();
    }

    /// <summary>
    /// Turns sfx on or off depending on
    /// the <paramref name="sfx"/> variable.
    /// </summary>
    /// <param name="sfx">boolean value that determines if sfx should be on or off.</param>
    public void OnSFXButtonPressed(bool sfx)
    {
        GameManager.Instance.GameSFX = sfx;
    }

    public void OnEnableTouchPadPressed(bool enableTouchPad)
    {
        GameManager.Instance.EnableTouchPad = enableTouchPad;
    }

    /// <summary>
    /// Saves the game.
    /// </summary>
    public void OnSaveButtonPressed()
    {
        //TODO: optional - ask if they are sure they want to save progress
        // GameManager.SaveGame();
        GameManager.Instance.SaveGameData();
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void OnExitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_WEBGL
        AudioManager.Instance.StopCurrentMusic(false);
        SceneLoader.Instance.LoadScene("Start Scene", TransitionType.FADE_TO_BLACK);
        // Application.ExternalEval("window.location.href = 'https://eo-comics.itch.io/guardians-of-rpg-v103-beta';");

#else
        Application.Quit();

#endif
    }

    private void SetUpOptions()
    {
        int volumeVal = (int)(GameManager.Instance.GameVolume * 100);

        sfxOn.SetActive(GameManager.Instance.GameSFX);
        sfxOff.SetActive(!GameManager.Instance.GameSFX);

        touchPadOn.SetActive(GameManager.Instance.EnableTouchPad);
        touchPadOff.SetActive(!GameManager.Instance.EnableTouchPad);

        volumeSlider.SetValue(volumeVal, 100);
        volumeValueText.text = volumeVal.ToString();
    }
}
