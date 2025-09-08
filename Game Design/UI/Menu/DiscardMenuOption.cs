using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DiscardMenuOption : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] TextMeshProUGUI itemAmountText;
    [SerializeField] Button incrementButton;
    [SerializeField] Button decrementButton;

    [SerializeField] public Button confirmButton;
    [SerializeField] Button cancelButton;

    public int maxAmount;

    public void SetUpWindow()
    {
        itemAmountText.text = "1";
        incrementButton.onClick.AddListener(() => 
        {
            int amount = int.Parse(itemAmountText.text) + 1;
            amount = amount > maxAmount ? maxAmount : amount;
            itemAmountText.text = amount.ToString();
        });
        decrementButton.onClick.AddListener(() => 
        {
            int amount = int.Parse(itemAmountText.text) - 1;
            amount = amount < 1 ? 1 : amount;
            itemAmountText.text = amount.ToString();
        });
        cancelButton.onClick.AddListener(() =>
        {
            StartCoroutine(ResetWindow());
        });
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.OPEN_UI_1);
        animator.Play("discard_settings_open");
    }

    public int GetItemAmount()
    {
        return int.Parse(itemAmountText.text);
    }
    public IEnumerator ResetWindow()
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.CLOSE_UI_4);
        animator.Play("discard_settings_close");
        yield return new WaitForSeconds(0.25f);
        itemAmountText.text = "1";
        this.gameObject.SetActive(false);
    }
}