using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Options is the class that holds all of
/// the other battle options UI that the
/// <c>Player</c> can choose for a round.
/// </summary>
public class Options : MonoBehaviour
{
    //serialize variables
    [SerializeField] public GameObject MoveOption;
    [SerializeField] public GameObject ItemOption;
    [SerializeField] public GameObject TargetOption;
    [SerializeField] public GameObject InitiativeOption;
    [SerializeField] public BattleOptions BattleOptions;
    [SerializeField] public Animator Animator;

    //public variable
    public string Option;

    /// <summary>
    /// Based on the option the player is in, 
    /// it will go to the previous appropriate
    /// option screen.
    /// <list type="bullet">
    ///     <listheader>
    ///         <term>Options</term>
    ///     </listheader>
    ///     <item>MOVE</item>
    ///     <item>ITEM</item>
    ///     <item>TARGET</item>
    ///     <item>INITIATIVE</item>
    /// </list>
    /// </summary>
    public void OnBackButtonPressed()
    {
        if(Option == null)
        {
            //TODO: close option tab and return to Battle Options
            StartCoroutine(CloseOptions());
            BattleOptions.gameObject.SetActive(true);
        }

        switch(Option)
        {
            case "MOVE":
            case "ITEM":
                Player.Instance().BattleStatus.ChosenMove = null;
                Player.Instance().BattleStatus.ChosenItem = null;
                StartCoroutine(CloseOptions());
                BattleOptions.gameObject.SetActive(true);
                break;
            case "TARGET":
                if(Player.Instance().BattleStatus.ChosenMove != null)
                    SetOption("MOVE");
                else if(Player.Instance().BattleStatus.ChosenItem != null)
                    SetOption("ITEM");
                break;
            case "INITIATIVE":
                SetOption("MOVE");
                break;
            default:
                StartCoroutine(CloseOptions());
                BattleOptions.gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Based on the option the player is in, 
    /// it will go to the next appropriate option
    /// screen, or it will lock in the <c>Player</c>'s
    /// selection and end the turn.
    /// <list type="bullet">
    ///     <listheader>
    ///         <term>Options</term>
    ///     </listheader>
    ///     <item>MOVE</item>
    ///     <item>ITEM</item>
    ///     <item>TARGET</item>
    ///     <item>INITIATIVE</item>
    /// </list>
    /// </summary>
    public void OnNextButtonPressed()
    {
        Player player = Player.Instance();
        if(Option == null)
            return;

        switch(Option)
        {
            case "MOVE":
                //if target is enemy and there is one enemy, skip target.
                //if skipping target and out of initiatives, end turn
                if(BattleSimStatus.Enemies.Count == 1 && (player.BattleStatus.ChosenMove.Target.Equals(MoveTarget.ENEMY) || player.BattleStatus.ChosenMove.Target.Equals(MoveTarget.ALL_ENEMIES)))
                {
                    foreach(Character enemy in BattleSimStatus.Enemies)
                    {
                        if(enemy != null)
                        {
                            player.BattleStatus.ChosenTargets.Clear();
                            player.BattleStatus.ChosenTargets.Add(enemy);
                            break;
                        }
                    }
                    // if(player.BattleStatus.InitiativeCount > 0)
                    //     SetOption("INITIATIVE");
                    // else
                    //     EndTurn();
                    EndTurn();
                }
                else
                    SetOption("TARGET");
                break;
            case "ITEM":
                //if player is the only one present, skip target and end turn
                if(BattleSimStatus.CharacterTypePresent("ALLY"))
                    SetOption("TARGET");
                else
                {
                    player.BattleStatus.ChosenTargets.Clear();
                    player.BattleStatus.ChosenTargets.Add(player);
                    EndTurn();
                }
                break;
            case "TARGET":
                // //if player is out of initiatives, end turn
                // if(player.BattleStatus.ChosenMove != null && player.BattleStatus.InitiativeCount > 0)
                //     SetOption("INITIATIVE");
                // else
                //     EndTurn();
                EndTurn();
                break;
            case "INITIATIVE":
                //end turn
                EndTurn();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Closes any other options and opens
    /// the correct UI based on the 
    /// <paramref name="option"/> variable.
    /// </summary>
    /// <param name="option">the name of the option to be opened</param>
    public void SetOption(string option)
    {
        MoveOption.SetActive(false);
        ItemOption.SetActive(false);
        TargetOption.SetActive(false);
        InitiativeOption.SetActive(false);

        Option = option;

        switch(option)
        {
            case "MOVE":
                MoveOption.SetActive(true);
                break;
            case "ITEM":
                ItemOption.SetActive(true);
                break;
            case "TARGET":
                TargetOption.SetActive(true);
                break;
            case "INITIATIVE":
                InitiativeOption.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Closes all of the option UI.
    /// </summary>
    public IEnumerator CloseOptions()
    {
        Animator.Play("ui_to_fade");
        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
    }
    
    private void EndTurn()
    {
        StartCoroutine(CloseOptions());
        BattleSimStatus.EndPlayerOption = true;
    }
}