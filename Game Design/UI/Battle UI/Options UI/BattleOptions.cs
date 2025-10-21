using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BattleOptions is a class that is responsible
/// for mapping and listing the options that the
/// <c>Player</c> can choose during battle.
/// </summary>
public class BattleOptions : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_3 = new WaitForSeconds(0.3f);

    //serialized variables
    public Button RunButton;
    public Options Options;
    public Animator Animator;

    void Start()
    {
        RunButton.interactable = BattleSimStatus.CanRun;
    }

    /// <summary>
    /// Closes the BattleOptions UI and
    /// opens the Items selection UI.
    /// </summary>
    public void OnItemButtonPressed()
    {
        StartCoroutine(CloseBattleOptions());
        Options.SetOption("ITEM");
        Options.gameObject.SetActive(true);
    }

    /// <summary>
    /// Closes the BattleOptions UI and
    /// opens the Moves selection UI.
    /// </summary>
    public void OnMoveButtonPressed()
    {
        StartCoroutine(CloseBattleOptions());
        Options.SetOption("MOVE");
        Options.gameObject.SetActive(true);
    }

    /// <summary>
    /// Sets the <c>Player</c>'s option to RUN.
    /// </summary>
    public void OnRunButtonPressed()
    {
        StartCoroutine(CloseBattleOptions());
        Player.Instance().BattleStatus.SetRollRun(true);
        Player.Instance().BattleStatus.SetTurnStatus(TurnStatus.RUN);
        BattleSimStatus.EndPlayerOption = true;
    }

    /// <summary>
    /// Sets the <c>Player</c>'s option to SKIP.
    /// </summary>
    public void OnSkipButtonPressed()
    {
        StartCoroutine(CloseBattleOptions());
        Player.Instance().BattleStatus.SetTurnStatus(TurnStatus.SKIP);
        BattleSimStatus.EndPlayerOption = true;
    }

    /// <summary>
    /// Closes battle option UI
    /// </summary>
    public IEnumerator CloseBattleOptions()
    {
        Animator.Play("text_box_battle_options_close");
        yield return _waitForSeconds0_3;
        gameObject.SetActive(false);
    }
}