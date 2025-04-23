using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MenuState
{
    [SerializeField] private SliderBar volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValueText;
    [SerializeField] private GameObject sfxOn;
    [SerializeField] private GameObject sfxOff;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetUpOptions();
    }

    public void OnVolumeSliderChange()
    {
        int volumeVal = (int) (volumeSlider.slider.value);
        volumeValueText.text = volumeVal.ToString();
        GameManager.Instance.GameVolume = (float)(volumeVal / 100f);
        AudioManager.Instance.AdjustVolume();
    }

    public void OnSFXButtonPressed(bool sfx)
    {
        GameManager.Instance.GameSFX = sfx;
    }

    public void OnSaveButtonPressed()
    {
        //TODO: optional - ask if they are sure they want to save progress
        GameManager.SaveGame();
    }

    public void OnExitButtonPressed()
    {
        //TODO: optional - ask if they are sure they want to exit game
        Application.Quit();
    }

    private void SetUpOptions()
    {
        int volumeVal = (int) (GameManager.Instance.GameVolume * 100);
        
        sfxOn.SetActive(GameManager.Instance.GameSFX);
        sfxOff.SetActive(!GameManager.Instance.GameSFX);

        volumeSlider.SetValue(volumeVal, 100);
        volumeValueText.text = volumeVal.ToString();
    }
}
