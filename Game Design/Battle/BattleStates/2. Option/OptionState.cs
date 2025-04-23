using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OptionState is a class that extends the
/// <c>BattleState</c>. This class starts
/// the <c>BattleTimer</c> and opens the 
/// <c>Options</c> UI to help the <c>Player</c>
/// determine what option they wish to take.
/// This class also sets up the actions that
/// the NPCs in the battle will take.
/// 
/// Once that has been completed, or the timer
/// has ran out, it will determine if the next
/// state should be <c>RollInitiativeState</c>,
/// <c>RollRunState</c>, or <c>CharacterActionState</c>.
/// </summary>
public class OptionState : BattleState
{
    //serialized variables
    [SerializeField] public GameObject Timer;
    [SerializeField] public GameObject BattleOptions;

    //Constructor
    public OptionState(GameObject timer, GameObject battleOptions)
    {
        Timer = timer;
        BattleOptions = battleOptions;
    }

    public override void Enter()
    {
        BattleSimStatus.RoundStarted = false;
        InitializeBattleOptions();
    }

    public override void Update()
    {
        if(BattleSimStatus.EndPlayerOption)
        {
            // Player player = Player.Instance();

            // if(player.BattleStatus.RollInitiative)
            // {
            //     player.BattleStatus.SetRollInitiativeFalse();
            //     NextState = "ROLL INITIATIVE STATE";
            // }
            // else if(player.BattleStatus.RollRun)
            // {
            //     player.BattleStatus.SetRollRun(false);
            //     NextState = "ROLL RUN STATE";
            // }
            // else
            //    NextState = "CHARACTER ACTION STATE";
            
            NextState = "CHARACTER ACTION STATE";
        }
    }

    public override void Exit()
    {
        BattleTimer bt = Timer.GetComponent<BattleTimer>();
        bt.StopTimer();
        SetNPCOptions();
    }

    private void InitializeBattleOptions()
    {
        Player.Instance().BattleStatus.ResetRound();
        BattleSimStatus.EndPlayerOption = false;
        BattleOptions.SetActive(true);
        Timer.SetActive(true);
        BattleTimer bt = Timer.GetComponent<BattleTimer>();
        bt.StartTimer();
    }

    private void SetNPCOptions()
    {
        List<Character> list = new List<Character>();
        list.AddRange(BattleSimStatus.Allies.ToArray());
        list.AddRange(BattleSimStatus.Enemies.ToArray());
        foreach(Character c in list)
        {
            c.BattleStatus.ResetRound();
            NPCLogic logic = new NPCLogic(c);
            //TODO: Update NPC logic based on NPC logic level
            logic.SetRandomMove();
            logic.SetTargetList();
            logic.SetRandomTargets();
        }
    }
}
