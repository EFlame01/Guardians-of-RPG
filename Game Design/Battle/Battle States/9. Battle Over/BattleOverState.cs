using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

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
    private string _winner;

    //private variables
    private PlayerHUD playerHUD;
    private DialogueData dialogueData;
    private TextBox textBox;
    private List<string> texts;
    private bool startedDialogue;

    //Constructor
    public BattleOverState(PlayerHUD playerHUD, DialogueData dialogueData, TextBox textBox)
    {
        this.playerHUD = playerHUD;
        this.dialogueData = dialogueData;
        this.textBox = textBox;
        texts = new List<string>();
    }

    public override void Enter()
    {
        GetText();
        GetLevelUpText();
        UpdateFlagForWin();
        AnnounceBattleResult();
    }

    public override void Update()
    {
        if(startedDialogue && DialogueManager.Instance.DialogueEnded)
            NextState = "END BATTLE";
    }

    public override void Exit()
    {
        if(Winner().Equals("ENEMY"))
            Player.Instance().BaseStats.SetHp((int)Mathf.Clamp((float)Player.Instance().BaseStats.FullHp * 0.2f, 1, Player.Instance().BaseStats.FullHp));
        NextState = null;
    }

    private void GetText()
    {
        _winner = Winner();
        List<Character> allies = new List<Character>();
        List<Character> enemies = new List<Character>();
        string text = "";

        foreach(Character c in BattleSimStatus.Allies)
            allies.Add(c);
        foreach(Character c in BattleSimStatus.Graveyard)
        {
            if(c.Type.Equals("ALLY"))
                allies.Add(c);
        }
        foreach(Character c in BattleSimStatus.Enemies)
            enemies.Add(c);
        foreach(Character c in BattleSimStatus.Graveyard)
        {
            if(c.Type.Equals("ENEMY"))
                enemies.Add(c);
        }

        if(_winner.Equals("PLAYER"))
        {
            text = Player.Instance().Name + " ";
            
            for(int i = 0; i < allies.Count; i++)
            {
                if(i == 0 && i + 1 == allies.Count)
                    text += "and " +  (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower() : allies[i].Name);
                else if(i == 0)
                    text += ", " + (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower() : allies[i].Name);
                else if(i + 1 == allies.Count)
                    text += ", and " + (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower() : allies[i].Name);
            }
            
            text += " defeated ";

            for(int i = 0; i < enemies.Count; i++)
            {
                if(i == 0)
                    text += enemies[i].Name.Contains("Wild") ? "a " + enemies[i].Name.ToLower()  : enemies[i].Name;
                else if(i + 1 == enemies.Count)
                    text += ", and " + (enemies[i].Name.Contains("Wild") ? "a " + enemies[i].Name.ToLower()  : enemies[i].Name);
                else
                    text += ", " + (enemies[i].Name.Contains("Wild") ? "a " + enemies[i].Name.ToLower()  : enemies[i].Name);
            }

            text += "!";

        }
        else if (_winner.Equals("ENEMY"))
        {
            text = Player.Instance().Name + " ";
            
            for(int i = 0; i < allies.Count; i++)
            {
                if(i == 0 && i + 1 == allies.Count)
                    text += "and " +  (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower()  : allies[i].Name);
                else if(i == 0)
                    text += ", " + (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower()  : allies[i].Name);
                else if(i + 1 == allies.Count)
                    text += ", and " + (allies[i].Name.Contains("Wild") ? "a " + allies[i].Name.ToLower()  : allies[i].Name);
            }
            
            text += " was defeated by ";

            for(int i = 0; i < enemies.Count; i++)
            {
                if(i == 0)
                    text += enemies[i].Name;
                else if(i + 1 == enemies.Count)
                    text += ", and " + (enemies[i].Name.Contains("Wild") ? "a " + enemies[i].Name.ToLower()  : enemies[i].Name);
                else
                    text += ", " + (enemies[i].Name.Contains("Wild") ? "a " + enemies[i].Name.ToLower()  : enemies[i].Name);
            }

            text += "!";
        }

        texts.Add(text);
    }

    private void GetLevelUpText()
    {
        int oldLevel = Player.Instance().Level;
        int xp = 0;
        int bits = 0;
        List<Item> itemHaul = new List<Item>();

        foreach(Character c in BattleSimStatus.Graveyard)
        {
            if(c.Type.Equals("ENEMY"))
            {
                xp += Level.DetermineXPForBattle(c.Level);
                bits += c.Bits;
                // if(Mathf.Random)
            }
        }
        Level.GainXP(xp);
        Level.LevelUpPlayer();
        int newLevel = Player.Instance().Level;
        Move[] newMoves = MoveMaker.Instance.GetLevelUpMoves(newLevel, Player.Instance().Archetype.ArchetypeName, Player.Instance().Archetype.ClassName);

        if(bits > 0)
            texts.Add("You gained " + xp + " XP" + " and " + bits + " bits!");
        else
            texts.Add("You gained " + xp + " XP!");

        if(newLevel != oldLevel)
        {
            texts.Add("You are now level " + newLevel + "!");
            foreach(Move move in newMoves)
            {
                if(!MoveManager.MoveDictionary.ContainsKey(move.Name))
                {
                    texts.Add("You learned " + move.Name + "!");
                    Player.Instance().MoveManager.AddMove(move.Name);
                }
            }
        }

        playerHUD.UpdateHUD(Player.Instance());
        playerHUD.UpdateXPBar();
    }

    private void AnnounceBattleResult()
    {
        //TODO: find victory song to play
        if (_winner.Equals("PLAYER"))
            AudioManager.Instance.BlendMusic2(Units.Music.VICTORY_THEME);
        
        DialogueManager.Instance.CurrentStory = new Story(dialogueData.InkJSON.text);
        for (int i = 0; i < texts.Count; i++)
        {
            int textNum = i + 1;
            if (i == 0)
                DialogueManager.Instance.CurrentStory.variablesState["text"] = texts[0];
            else
                DialogueManager.Instance.CurrentStory.variablesState["text" + textNum] = texts[i];
        }
        textBox.OpenTextBox();
        textBox.StartNarration(dialogueData);
        startedDialogue = true;
    }

    public void UpdateFlagForWin()
    {
        if(_winner.Equals("PLAYER"))
        {
            if(BattleInformation.StoryFlagsIfWon != null && BattleInformation.StoryFlagsIfWon.Length > 0)
            {
                foreach(string flagID in BattleInformation.StoryFlagsIfWon)
                    Player.Instance().StoryFlagManager.UpdateFlag(flagID, true);
            }
        }
    }

}