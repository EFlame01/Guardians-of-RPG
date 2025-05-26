using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffectState2 : BattleState
{
    //private variables
    private Camera _camera;
    private TextBox _narrationTextBox;
    private DialogueData _dialogueData;
    private BattleCharacter _battlePlayer;
    private BattleCharacter[] _battleAllies;
    private BattleCharacter[] _battleEnemies;
    private BattleActionEffect _battleActionEffect;

    //Constructor
    public ActionEffectState2(BattleCharacter battlePlayer, BattleCharacter[] battleAllies, BattleCharacter[] battleEnemies, Camera camera, DialogueData dialogueData, TextBox textBox, BattleActionEffect battleActionEffect)
    {
        _battlePlayer = battlePlayer;
        _battleAllies = battleAllies;
        _battleEnemies = battleEnemies;
        _camera = camera;
        _dialogueData = dialogueData;
        _narrationTextBox = textBox;
        _battleActionEffect = battleActionEffect;
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
