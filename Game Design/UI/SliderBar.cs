using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;

public class SliderBar : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI maxValueText;
    public Slider slider;
    [SerializeField] protected Gradient gradient;
    [SerializeField] protected Image fill;

    public void SetValue(int value, int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = value;

        if (valueText != null)
            valueText.text = value.ToString();
        if (maxValueText != null)
            maxValueText.text = maxValue.ToString();
        if (fill != null)
            fill.color = gradient.Evaluate(slider.value / slider.maxValue);
    }

    public IEnumerator ChangeValue(int newValue, int newMaxValue, bool updateCharacterHUD)
    {
        valueText.text = slider.value.ToString();
        maxValueText.text = slider.maxValue.ToString();
        float duration = 1f;
        float startTime = 0f;
        while (startTime < duration)
        {
            float t = startTime / duration;
            slider.value = Mathf.Lerp(slider.value, newValue, t);
            slider.maxValue = Mathf.Lerp(slider.maxValue, newMaxValue, t);

            valueText.text = ((int)slider.value).ToString();
            maxValueText.text = ((int)slider.maxValue).ToString();

            fill.color = gradient.Evaluate(slider.value / slider.maxValue);

            startTime += Time.fixedDeltaTime;
            yield return null;
        }

        slider.value = newValue;
        slider.maxValue = newMaxValue;

        valueText.text = ((int)slider.value).ToString();
        maxValueText.text = ((int)slider.maxValue).ToString();

        fill.color = gradient.Evaluate(slider.value / slider.maxValue);

        if (updateCharacterHUD)
            BattleSimStatus.UpdatedCharacterHUD = true;
    }
}