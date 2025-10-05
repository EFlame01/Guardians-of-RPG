using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// TargetOption is a class that handles
/// the UI responsible for selecting the
/// <c>Character</c> the <c>Player</c> will
/// target in the next round.
/// </summary>
public class TargetOption : MonoBehaviour
{
    //serialized variables
    public Transform TargetLayout;
    public GameObject TargetButton;
    public Options Options;
    public Button NextButton;

    public void Start()
    {
        InitializeTargetOption();
    }

    public void Update()
    {
        NextButton.interactable = Player.Instance().BattleStatus.ChosenTargets.Count > 0;
    }

    private void InitializeTargetOption()
    {
        Player player = Player.Instance();

        if (player.BattleStatus.ChosenMove != null)
            DetermineTargetForMove();
        else if (player.BattleStatus.ChosenItem != null)
            DetermineTargetForItem();
        // else
        //     Options.OnNextButtonPressed();
    }

    private void DetermineTargetForItem()
    {
        Item item = Player.Instance().BattleStatus.ChosenItem;
        switch (item.Type)
        {
            case ItemType.FOOD:
            case ItemType.MEDICAL:
                MakeOneButton(Player.Instance(), BattleInformation.BattlePlayerData);
                List<Character> allySide = new List<Character>();
                allySide.AddRange(BattleSimStatus.Allies.ToArray());
                foreach (Character c in BattleSimStatus.Graveyard)
                {
                    if (c.Type.Equals("ALLY"))
                        allySide.Add(c);
                }
                foreach (Character character in allySide)
                {
                    foreach (BattleCharacterData data in BattleInformation.BattleAlliesData)
                    {
                        if (data != null && data.CharacterData.Equals(character.Id))
                            MakeOneButton(character, data);
                    }
                }
                break;
            default:
                MakeOneButton(Player.Instance(), BattleInformation.BattlePlayerData);
                break;
        }
    }

    private void DetermineTargetForMove()
    {
        Move move = Player.Instance().BattleStatus.ChosenMove;
        switch (move.Target)
        {
            case MoveTarget.ENEMY:
                foreach (Character enemy in BattleSimStatus.Enemies)
                {
                    foreach (BattleCharacterData data in BattleInformation.BattleEnemiesData)
                    {
                        if (data != null && data.CharacterData.Equals(enemy.Id))
                            MakeOneButton(enemy, data);
                    }
                }
                break;
            case MoveTarget.ALL_ENEMIES:
                MakeAllButton(BattleSimStatus.Enemies.ToArray(), null);
                break;
            case MoveTarget.ALLY:
                foreach (Character ally in BattleSimStatus.Allies)
                {
                    foreach (BattleCharacterData data in BattleInformation.BattleAlliesData)
                    {
                        if (data != null && data.CharacterData.Equals(ally.Id))
                            MakeOneButton(ally, data);
                    }
                }
                break;
            case MoveTarget.ALL_ALLIES:
                MakeAllButton(BattleSimStatus.Allies.ToArray(), null);
                break;
            case MoveTarget.ALLY_SIDE:
                //TODO: create one button for ally's side
                List<Character> allySide = new List<Character>
                {
                    Player.Instance()
                };
                allySide.AddRange(BattleSimStatus.Allies.ToArray());
                MakeAllButton(allySide.ToArray(), null);
                break;
            case MoveTarget.EVERYONE:
                //TODO: create one button for everyone
                List<Character> everyone = new List<Character>();
                everyone.AddRange(BattleSimStatus.Allies.ToArray());
                everyone.AddRange(BattleSimStatus.Enemies.ToArray());
                MakeAllButton(everyone.ToArray(), null);
                break;
        }//End of switch()...
    }

    private void MakeOneButton(Character character, BattleCharacterData battleCharacterData)
    {
        TargetButton targetButton = Instantiate(TargetButton, TargetLayout).GetComponent<TargetButton>();
        targetButton.textComponent.text = character.Name;
        targetButton.image.sprite = battleCharacterData.IsPlayer ? battleCharacterData.GetPlayerSprite() : battleCharacterData.CharacterSprite;
        targetButton.button.onClick.AddListener(() =>
        {
            //TODO: add target
            Character[] targets = { character };
            AddTargets(targets);
        });
    }

    private void MakeAllButton(Character[] targets, Sprite image)
    {
        TargetButton targetButton = Instantiate(TargetButton, TargetLayout).GetComponent<TargetButton>();
        targetButton.textComponent.text = MoveTarget.ALL_ALLIES.ToString();
        targetButton.image.sprite = image;

        targetButton.button.onClick.AddListener(() =>
        {
            //TODO: add target
            AddTargets(targets);
        });
    }

    private void AddTargets(Character[] targets)
    {
        Player player = Player.Instance();
        player.BattleStatus.ChosenTargets.AddRange(targets);
        Options.OnNextButtonPressed();
    }

}