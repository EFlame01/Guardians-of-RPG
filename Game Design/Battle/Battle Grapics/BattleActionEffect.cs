using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using Ink.Runtime;

/// <summary>
/// BattleActionEffect is a helper class to
/// the <c>ActionEffectState</c> class. This
/// class is responsible for displaying the
/// animations of the actions, as well as
/// updating the <c>CharacterHUD</c> for a
/// <c>BattleCharacter</c> after said action
/// is completed.
/// </summary>
public class BattleActionEffect : MonoBehaviour
{
    //public variables
    public Queue<Character> TargetQueue = new Queue<Character>();
    public bool StartedDialogue;
    public bool FinishedAction;
    public Character Target {get; private set;}

    //private variables
    private BattleCharacter _battlePlayer;
    private BattleCharacter[] _battleAllies;
    private BattleCharacter[] _battleEnemies;
    private Character _user;
    private Camera _camera;
    private DialogueData _dialogueData;
    private List<string> _effectText;
    private MoveEffects moveEffects;
    private TextBox _textBox;
    private bool _firstTimeMove;
    private string _effect;

    /// <summary>
    /// Sets the public and private variables to the
    /// ones found in the <c>ActionEffectState</c>.
    /// 
    /// This allows the <c>BattleActionEffect</c> class
    /// to perform the animations needed for the action.
    /// </summary>
    /// <param name="user">The user of the action</param>
    /// <param name="battlePlayer">The <c>Player</c> battle information</param>
    /// <param name="allies">An array of the allies' battle information</param>
    /// <param name="enemies">An array of the enemies' battle information</param>
    /// <param name="camera">The main camera</param>
    /// <param name="textBox">The textbox used to display the results of action</param>
    /// <param name="dialogueData">The dialogue data used to store the dialogue</param>
    public void SetUpBattleActionEffect(Character user, BattleCharacter battlePlayer, BattleCharacter[] allies, BattleCharacter[] enemies, Camera camera, TextBox textBox, DialogueData dialogueData)
    {
        _user = user;
        _battlePlayer = battlePlayer;
        _battleAllies = allies;
        _battleEnemies = enemies;
        _camera = camera;
        _textBox = textBox;
        _dialogueData = dialogueData;
        _effectText = new List<string>();
        _firstTimeMove = true;
        AddTargetsToQueue();
        GetEffect();
    }

    /// <summary>
    /// Public method that is used to start the coroutine to
    /// start the action effect from a non MonoBehaviour class.
    /// </summary>
    public void StartActionEffect()
    {
        StartCoroutine(PerformAction());
    }

    private void AddTargetsToQueue()
    {
        Character[] characters = _user.BattleStatus.ChosenTargets.ToArray();
        foreach(Character c in characters)
            TargetQueue.Enqueue(c);
    }

    private void GetEffect()
    {
        if(_user.BattleStatus.ChosenMove != null)
        {
            _effect = "MOVE";
            return;
        }
        else if(_user.BattleStatus.ChosenItem != null)
        {
            _effect = "ITEM";
            return;
        }
        else if(_user.BattleStatus.TurnStatus.Equals(TurnStatus.SKIP))
            _effect = "SKIP";
    }

    private IEnumerator PerformAction()
    {
        if(TargetQueue.Count == 0)
        {
            Target = null;
            FinishedAction = true;
            _firstTimeMove = true;
            yield return null;
        }
        else
        {
            FinishedAction = false;
            Target = TargetQueue.Dequeue();
            moveEffects = GetMoveEffects(Target);
            if(ActionSuccessful())
            {
                switch(_effect)
                {
                    case "MOVE":
                        if(_firstTimeMove)
                        {
                            yield return ActionAnimation();
                            _firstTimeMove = false;
                        }
                        yield return EffectAnimation("FLASH");
                        yield return MoveEffect();
                        break;
                    case "ITEM":
                        yield return EffectAnimation("");
                        yield return ItemEffect();
                        break;
                    default:
                        break;
                }
            }
            yield return new WaitForSeconds(0.25f);
            DisplayText();
        }
    }

    private List<Character> SetTargetBasedOnEffectTarget(MoveTarget target)
    {
        List<Character> effectTargets = new List<Character>();
        return effectTargets;
    }

    private IEnumerator ActionAnimation()
    {
        string offset = Target.Type.Equals("ENEMY") ? "enemy" : "ally";
        EnableAllCharacterHUD(false);
        yield return moveEffects.ActionAnimationRoutine(_user.BattleStatus.ChosenMove.Name, offset);
    }

    private IEnumerator EffectAnimation(string afterEffect)
    {
        //TODO: Create animation for effect
        yield return new WaitForSeconds(0.25f);
        switch(afterEffect)
        {
            case "FLASH":
                yield return moveEffects.FlashRoutine();
                break;
            case "HEAL":
                yield return moveEffects.HealRoutine();
                break;
            case "STAT_CHANGE":
                yield return moveEffects.StatRoutine();
                break;
            default:
                yield return moveEffects.FlashRoutine();
                break;
        }
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator MoveEffect()
    {
        _user.BattleStatus.ChosenMove.UseMove(_user, Target, 1);
        EnableAllCharacterHUD(true);
        UpdateBattleCharacter(_user);
        UpdateBattleCharacter(Target);
        SetMoveEffectString();
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator ItemEffect()
    {
        Item item = _user.BattleStatus.ChosenItem;
        item.UseItem(Target);
        EnableAllCharacterHUD(true);
        UpdateInventory(item);
        UpdateBattleCharacter(Target);
        SetItemEffectString();
        yield return new WaitForSeconds(1f);
    }

    private void UpdateInventory(Item item)
    {
        if(!_user.Equals(Player.Instance()))
            return;
        Player player = Player.Instance();
        switch(item.Type)
        {
            case ItemType.FOOD:
            case ItemType.MEDICAL:
                player.Inventory.ChangeItemAmount(item.Name, -1);
                break;
            case ItemType.HEALING:
            case ItemType.PRIORITY:
            case ItemType.STAT_CHANGING:
                player.Inventory.EquipItem(item.Name);
                break;
        }
    }

    private void DisplayText()
    {
        DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);
        
        for(int i = 0; i < _effectText.Count; i++)
        {
            int val = i + 1;
            string variable = val >= 2 ? "text" + val.ToString() : "text";
            DialogueManager.Instance.CurrentStory.variablesState[variable] = _effectText[i];
        }

        _textBox.OpenTextBox();
        _textBox.StartNarration(_dialogueData);
        StartedDialogue = true;
    }

    private void UpdateBattleCharacter(Character character)
    {
        if(_battlePlayer.Character.Id.Equals(character.Id))
        {
            _battlePlayer.UpdateHUD();
            return;
        }

        foreach(BattleCharacter ally in _battleAllies)
        {
            if(ally.Character != null && ally.Character.Id.Equals(character.Id))
            {
                ally.UpdateHUD();
                return;
            }
        }

        foreach(BattleCharacter enemy in _battleEnemies)
        {
            if(enemy.Character != null && enemy.Character.Id.Equals(character.Id))
            {
                enemy.UpdateHUD();
                return;
            }
        }
    }

    private void SetMoveEffectString()
    {
        if(_effect.Equals("MOVE"))
        {
            Move move = _user.BattleStatus.ChosenMove;
            _effectText.Clear();
            switch(move.Type)
            {
                case MoveType.REGULAR:
                case MoveType.PRIORITY:
                    _effectText.Add("It hit " + Target.Name + "!");
                    break;
                case MoveType.HEALING:
                    _effectText.Add(_user.Name + " healed " + Target.Name + "!");
                    break;
                case MoveType.STAT_CHANGING:
                    StatChangingMove statChangingMove = (StatChangingMove) move;
                    for(int i = 0; i < statChangingMove._stats.Length; i++)
                    {
                        string changed = statChangingMove._stages[i] > 0 ? " increased " : " decreased ";
                        int stageLevel = Mathf.Abs(statChangingMove._stages[i]);
                        _effectText.Add(Target.Name + "'s " + statChangingMove._stats[i] + changed + stageLevel + " stages!");
                    }
                    break;
                case MoveType.STATUS_CHANGING:
                    StatusChangingMove statusChangingMove = (StatusChangingMove) move;
                    if(Target.BattleStatus.StatusConditions[statusChangingMove._statusCondition.Name].Equals(statusChangingMove._statusCondition))
                        _effectText.Add(Target.Name + " is " + statusChangingMove._statusCondition.AfflictionText + "!");
                    else
                        _effectText.Add("The move failed!");
                    break;
                case MoveType.COUNTER:
                    string targetSex = "themselves";
                    if(Target.Sex.Equals("MALE"))
                        targetSex = "himself";
                    else if(Target.Sex.Equals("FEMALE"))
                        targetSex = "herslef";
                    _effectText.Add(Target.Name + " prepared " + targetSex + " for an attack!");
                    break;
                case MoveType.PROTECT:
                    _effectText.Add(Target.Name + " was protected!");
                    break;
                case MoveType.KNOCK_OUT:
                    _effectText.Add("It's a one hit knock out!");
                    break;
                default:
                    break; 
            }
        }
    }

    private void SetItemEffectString()
    {
        if(!_effect.Equals("ITEM"))
            return;

        Item item = Player.Instance().BattleStatus.ChosenItem;
        switch(item.Type)
        {
            case ItemType.FOOD:
            case ItemType.MEDICAL:
                _effectText.Add(Target.Name + " was healed!");
                break;
            case ItemType.HEALING:
            case ItemType.PRIORITY:
            case ItemType.STAT_CHANGING:
                _effectText.Add(Target.Name + " equipped the " + item.Name + "!");
                break;
        }
    }

    private bool ActionSuccessful()
    {
        if(_effect == "ITEM")
        {
            Item item = _user.BattleStatus.ChosenItem;
            switch(item.Type)
            {
                case ItemType.FOOD:
                    if(Target.BaseStats.Hp == Target.BaseStats.FullHp)
                    {
                        _effectText.Add("It had no effect!");
                        return false;
                    }
                    return true;
                case ItemType.MEDICAL:
                    MedicalItem medicalItem = (MedicalItem) item;
                    if(medicalItem._healAmount == 0)
                    {
                        foreach(string status in medicalItem._statusConditions)
                        {
                            if(status.Equals("ALL") || Target.BattleStatus.StatusConditions.ContainsKey(status))
                                return true;
                        }
                        _effectText.Add("It had no effect!");
                        return false;
                    }
                    else
                    {
                        if(Target.BaseStats.Hp == Target.BaseStats.FullHp)
                        {
                            _effectText.Add("It had no effect!");
                            return false;
                        }
                    }
                    break;
                case ItemType.KEY:
                    _effectText.Add("Cannot use that here!");
                    return false;
                default:
                    return false;
            }
        }
        else if(_effect == "MOVE")
        {
            Debug.Log("effect is: " + _effect);
            Move move = _user.BattleStatus.ChosenMove;
            if(move.DidMoveHit(_user, Target))
            {
                Debug.Log("move hit");
                if(Target.BaseStats.Hp == 0)
                {
                    _effectText.Add(Target.Name + " is already knocked out!");
                    Debug.Log(Target.Name + " is already knocked out!");
                    return false;
                }
                if(Target.BattleStatus.ProtectionStatus != "NONE")
                {
                    _effectText.Add(Target.Name + " protected themselves!");
                    Debug.Log(Target.Name + " protected themselves!");
                    return false;
                }
            }
            else
            {
               _effectText.Add("But the move missed!");
                Debug.Log("But the move missed!");
                return false; 
            }
        }
        Debug.Log(_effect + " was successful...");
        return true;
    }

    private MoveEffects GetMoveEffects(Character character)
    {
        if(_battlePlayer.Character.Equals(character))
            return _battlePlayer.MoveEffects;

        foreach(BattleCharacter ally in _battleAllies)
        {
            if(ally.Character != null && ally.Character.Equals(character))
                return ally.MoveEffects;
        }

        foreach(BattleCharacter enemy in _battleEnemies)
        {
            if(enemy.Character != null && enemy.Character.Equals(character))
                return enemy.MoveEffects;
        }

        return null;
    }

    private void EnableAllCharacterHUD(bool enable)
    {
        _battlePlayer.EnableHUD(enable);
        foreach(BattleCharacter ally in _battleAllies)
            ally.EnableHUD(enable);
        foreach(BattleCharacter enemy in _battleEnemies)
            enemy.EnableHUD(enable);
    }

}