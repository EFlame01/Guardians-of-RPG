using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// BattleOptions is a class that is responsible
/// for mapping and listing the options that the
/// <c>Player</c> can choose during battle.
/// </summary>
public class BattleOptions : MonoBehaviour
{
    //serialized variables
    [SerializeField] public Button RunButton;
    [SerializeField] public Options Options;
    [SerializeField] public Animator Animator;

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
        //TODO: player's option is to run
        Debug.Log("Run button pressed...");
        Player.Instance().BattleStatus.SetTurnStatus(TurnStatus.RUN);
        BattleSimStatus.EndPlayerOption = true;
    }

    /// <summary>
    /// Sets the <c>Player</c>'s option to SKIP.
    /// </summary>
    public void OnSkipButtonPressed()
    {
        //TODO: player's option is to skip turn
        Debug.Log("Skip button pressed...");
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
        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
    }
}