using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AfterRoundState will take the characters
/// that survived the round and update any 
/// battle status effects for the next round.
/// 
/// After this it will determine if there
/// </summary>
public class AfterRoundState : BattleState
{
    DialogueData dialogueData;
    TextBox textBox;
    BattleCharacter battlePlayer;
    BattleCharacter[] battleAllies;
    BattleCharacter[] battleEnemies;
    BattleActionEffect battleActionEffect;

    //Constructor
    public AfterRoundState()
    {

    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        
    }
}
