using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

/// <summary>
/// BattleOverState is a class that extends the 
/// <c>BattleState</c> class. This class announces
/// that the battle is over and the winners of the 
/// battle.
/// 
/// Once this is done, it will then end the
/// <c>BattleStateMachine</c>, which will then end 
/// the <c>BattleSimulator</c>.
/// </summary>
public class BattleOverState : BattleState
{

    //private variables
    private PlayerHUD _playerHUD;
    private DialogueData _dialogueData;
    private TextBox _textBox;
    private List<string> _texts;
    private bool _startedDialogue;
    private string _winner;

    //Constructor
    public BattleOverState(PlayerHUD playerHUD, DialogueData dialogueData, TextBox textBox)
    {
        CurrentState = Units.BATTLE_OVER_STATE;
        _playerHUD = playerHUD;
        _dialogueData = dialogueData;
        _textBox = textBox;
        _texts = new List<string>();
    }

    public override void Enter()
    {
        InitBattleOverProcedure();
    }

    public override void Update()
    {
        if (_startedDialogue && DialogueManager.Instance.DialogueEnded)
        {
            TextBoxBattle.EndNarrationNow = true;
            NextState = Units.END_BATTLE;
        }
    }

    public override void Exit()
    {
        TextBoxBattle.KeepTextBoxOpened = false;
        if (Winner().Equals("ENEMY"))
            Player.Instance().BaseStats.SetHp((int)Mathf.Clamp(Player.Instance().BaseStats.FullHp * 0.2f, 1, Player.Instance().BaseStats.FullHp));
        NextState = null;
    }

    private void GetText()
    {
        _winner = Winner();
        List<Character> allies = new List<Character>();
        List<Character> enemies = new List<Character>();
        string text = "";

        foreach (Character c in BattleSimStatus.Allies)
            allies.Add(c);
        foreach (Character c in BattleSimStatus.Graveyard)
        {
            if (c.Type.Equals("ALLY"))
                allies.Add(c);
        }
        foreach (Character c in BattleSimStatus.Enemies)
            enemies.Add(c);
        foreach (Character c in BattleSimStatus.Graveyard)
        {
            if (c.Type.Equals("ENEMY"))
                enemies.Add(c);
        }

        if (_winner.Equals("PLAYER"))
        {
            text = Player.Instance().Name + " ";

            for (int i = 0; i < allies.Count; i++)
            {
                if (i == 0 && i + 1 == allies.Count)
                    text += "and " + (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower() : allies[i].Name);
                else if (i == 0)
                    text += ", " + (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower() : allies[i].Name);
                else if (i + 1 == allies.Count)
                    text += ", and " + (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower() : allies[i].Name);
            }

            text += " defeated ";

            for (int i = 0; i < enemies.Count; i++)
            {
                if (i == 0)
                    text += enemies[i].Name.Contains("Wild") ? "a " + enemies[i].Name.ToLower() : enemies[i].Name;
                else if (i + 1 == enemies.Count)
                    text += ", and " + (enemies[i].Name.Contains("Wild") ? "a " + enemies[i].Name.ToLower() : enemies[i].Name);
                else
                    text += ", " + (enemies[i].Name.Contains("Wild") ? "a " + enemies[i].Name.ToLower() : enemies[i].Name);
            }

            text += "!";

        }
        else if (_winner.Equals("ENEMY"))
        {
            text = Player.Instance().Name + " ";

            //text that dependent on the amount of allies
            if (allies.Count == 0)
                text += "was defeated by ";
            if (allies.Count == 1)
                text += "and " + allies[0].Name + " were defeated by ";
            else if (allies.Count == 2)
                text += ", " + allies[0].Name + ", and " + allies[1].Name + " were defeated by ";

            //texts that's dependent on the amount of enemies
            if (enemies.Count == 1)
                text += enemies[0].Name + "!";
            else if (enemies.Count == 2)
                text += enemies[0].Name + " and " + enemies[1].Name + "!";
            else if (enemies.Count == 3)
                text += enemies[0].Name + ", " + enemies[1].Name + ", and " + enemies[2].Name + "!";
        }

        text = text.Replace("Wild", "a wild");
        _texts.Add(text);
    }

    private void GetLevelUpText()
    {
        int oldLevel = Player.Instance().Level;
        int xp = 0;
        int bits = 0;
        int numEnemies = 0;
        List<Item> itemHaul = new List<Item>();

        foreach (Character c in BattleSimStatus.Graveyard)
        {
            if (c.Type.Equals("ENEMY"))
            {
                xp += Level.DetermineXPForBattle(c.Level);
                bits += c.Bits;
                numEnemies++;
                // if(Mathf.Random)
            }
        }
        xp += Level.BonusXPForBattle(numEnemies);
        Level.GainXP(xp);
        Level.LevelUpPlayer();
        int newLevel = Player.Instance().Level;
        Move[] newMoves = new Move[4];

        try
        {
            newMoves = MoveMaker.Instance.GetLevelUpMoves(newLevel, Player.Instance().Archetype.ArchetypeName, Player.Instance().Archetype.ClassName);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"WARNING: {e.Message}");
        }


        if (bits > 0)
            _texts.Add("You gained " + xp + " XP" + " and " + bits + " bits!");
        else
            _texts.Add("You gained " + xp + " XP!");

        if (newLevel != oldLevel)
        {
            _texts.Add("You are now level " + newLevel + "!");
            foreach (Move move in newMoves)
            {
                if (!MoveManager.MoveDictionary.ContainsKey(move.Name))
                {
                    _texts.Add("You learned " + move.Name + "!");
                    Player.Instance().MoveManager.AddMove(move);
                }
            }
        }

        _playerHUD.UpdateHUD(Player.Instance());
        _playerHUD.UpdateXPBar();
    }

    private void AnnounceBattleResult()
    {
        TextBoxBattle.KeepTextBoxOpened = true;
        TextBoxBattle.EndNarrationNow = false;

        if (_winner.Equals("PLAYER"))
            AudioManager.Instance.BlendMusic2(Units.Music.VICTORY_THEME);

        DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);
        for (int i = 0; i < _texts.Count; i++)
        {
            int textNum = i + 1;
            if (i == 0)
                DialogueManager.Instance.SetVariableState("text", _texts[0], "string");
            else
                DialogueManager.Instance.SetVariableState("text" + textNum, _texts[i], "string");
        }
        _textBox.OpenTextBox();
        _textBox.StartNarration(_dialogueData);
        _startedDialogue = true;
    }

    public void UpdateFlagForWin()
    {
        if (_winner.Equals("PLAYER"))
        {
            SetVictoryNPCFlag(false);
            if (BattleInformation.StoryFlagsIfWon != null && BattleInformation.StoryFlagsIfWon.Length > 0)
            {
                foreach (string flagID in BattleInformation.StoryFlagsIfWon)
                    Player.Instance().StoryFlagManager.UpdateFlag(flagID, true);
            }
        }
        else
            SetVictoryNPCFlag(true);
    }

    private void SetVictoryNPCFlag(bool flag)
    {
        List<Character> enemies = new List<Character>();
        enemies.AddRange(BattleSimStatus.Enemies);
        foreach (Character c in BattleSimStatus.Graveyard)
        {
            if (c.Type.Equals("ENEMY"))
                enemies.Add(c);
        }
        try
        {
            foreach (Character c in enemies)
            {
                NpcData data = NpcDataContainer.GetNpcData(c.Id);
                if (data != null)
                {
                    data.foughtPlayer = true;
                    data.wonAgainstPlayer = flag;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("NPCs were not documented in SetVictoryNPCFlag()... " + e.Message);
        }
    }

    public void InitBattleOverProcedure()
    {
        GetText();
        GetLevelUpText();
        UpdateFlagForWin();
        AnnounceBattleResult();
    }

}