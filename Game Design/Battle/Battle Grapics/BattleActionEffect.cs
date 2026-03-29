using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

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
    //serialize variables
    public RollMechanic RollMechanic;

    //public variables
    public Queue<Character> TargetQueue = new Queue<Character>();
    public bool StartedDialogue;
    public bool FinishedAction;
    public Character Target { get; private set; }
    public bool DoneWithSecondaryEffects { get; private set; }
    public bool FinishedAfterRound;
    public bool FinishedBeforeRound;

    //private variables
    private BattleCharacter _battlePlayer;
    private BattleCharacter[] _battleAllies;
    private BattleCharacter[] _battleEnemies;
    private Character _user;
    private DialogueData _dialogueData;
    private List<string> _effectText;
    private MoveEffects moveEffects;
    private TextBox _textBox;
    private bool _firstTimeMove;
    private string _effect;
    private string _prevState;
    private string _currentState;

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
    public void SetUpBattleActionEffect(Character user, BattleCharacter battlePlayer, BattleCharacter[] allies, BattleCharacter[] enemies, Camera camera, TextBox textBox, DialogueData dialogueData, string prevState, string currentState)
    {
        _user = user;
        _battlePlayer = battlePlayer;
        _battleAllies = allies;
        _battleEnemies = enemies;
        _textBox = textBox;
        _dialogueData = dialogueData;
        _effectText = new List<string>();
        _firstTimeMove = true;
        _prevState = prevState;
        _currentState = currentState;
        GetEffect();
        AddTargetsToQueue();
    }

    public void SetUpSecondaryActionEffect(Character user, BattleCharacter battlePlayer, BattleCharacter[] allies, BattleCharacter[] enemies, Camera camera, TextBox textBox, DialogueData dialogueData, string prevState, string currentState)
    {
        _user = user;
        _battlePlayer = battlePlayer;
        _battleAllies = allies;
        _battleEnemies = enemies;
        _textBox = textBox;
        _dialogueData = dialogueData;
        _effectText = new List<string>();
        _prevState = prevState;
        _currentState = currentState;
        GetEffect();
    }

    public void SetUpBeforeRoundEffect(BattleCharacter battlePlayer, BattleCharacter[] allies, BattleCharacter[] enemies, TextBox textBox, DialogueData dialogueData, string prevState, string currentState)
    {
        _battlePlayer = battlePlayer;
        _battleAllies = allies;
        _battleEnemies = enemies;
        _textBox = textBox;
        _dialogueData = dialogueData;
        _effectText = new List<string>();
        _prevState = prevState;
        _currentState = currentState;
        GetEffect();
        AddTargetsToQueue();
    }

    /// <summary>
    /// Public method that is used to start the coroutine to
    /// start the action effect from a non MonoBehaviour class.
    /// </summary>
    public void StartActionEffect()
    {
        StartCoroutine(PerformAction());
    }

    public void StartSecondaryEffect()
    {
        StartCoroutine(PerformSecondaryAction());
    }

    public void StartAfterRoundEffect()
    {
        StartCoroutine(PerformAfterRoundAction());
    }

    public void StartBeforeRoundEffect()
    {
        StartCoroutine(PerformBeforeRoundAction());
    }

    private void AddTargetsToQueue()
    {
        TargetQueue.Clear();
        switch (_effect)
        {
            case "MOVE":
            case "ITEM":
                Character[] characters = _user.BattleStatus.ChosenTargets.ToArray();
                foreach (Character c in characters)
                    TargetQueue.Enqueue(c);
                break;
            case "RUN":
                TargetQueue.Enqueue(_user);
                break;
            case "BEFORE ROUND":
                TargetQueue.Enqueue(_battlePlayer.Character);
                foreach (BattleCharacter bc in _battleAllies)
                {
                    if (bc.Character != null)
                        TargetQueue.Enqueue(bc.Character);
                }
                foreach (BattleCharacter bc in _battleEnemies)
                {
                    if (bc.Character != null)
                        TargetQueue.Enqueue(bc.Character);
                }
                break;
            default:
                break;
        }
    }

    private void GetEffect()
    {
        if (_prevState.Equals(Units.CHARACTER_ACTION_STATE))
        {
            if (_user.BattleStatus.ChosenMove != null)
            {
                if (_currentState.Equals(Units.ACTION_EFFECT_STATE))
                    _effect = "MOVE";
                else if (_currentState.Equals(Units.ACTION_EFFECT_STATE_2))
                    _effect = "SECONDARY MOVE EFFECT";
            }
            else if (_user.BattleStatus.ChosenItem != null)
                _effect = "ITEM";
            else if (_user.BattleStatus.TurnStatus.Equals(TurnStatus.SKIP))
                _effect = "SKIP";
            else if (_user.BattleStatus.TurnStatus.Equals(TurnStatus.RUN))
                _effect = "RUN";
        }
        else if (_prevState.Equals(Units.AFTER_ROUND_STATE))
            _effect = "STATUS";
        else if (_prevState.Equals(Units.INITIALIZE_STATE))
            _effect = "BEFORE ROUND";
    }

    private IEnumerator PerformAction()
    {
        _effectText.Clear();
        if (TargetQueue.Count == 0)
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
            if (ActionSuccessful())
            {
                TextBoxBattle.KeepTextBoxOpened = false;
                TextBoxBattle.EndNarrationNow = true;
                switch (_effect)
                {
                    case "MOVE":
                        if (_firstTimeMove)
                        {
                            yield return StartCoroutine(ActionAnimation());
                        }
                        yield return StartCoroutine(EffectAnimation("FLASH"));
                        yield return StartCoroutine(MoveEffect());
                        break;
                    case "ITEM":
                        yield return StartCoroutine(EffectAnimation(""));
                        yield return StartCoroutine(ItemEffect());
                        break;
                    case "RUN":
                        yield return StartCoroutine(RollToRunAnimation());
                        break;
                    default:
                        break;
                }
            }
            yield return new WaitForSeconds(1f);
            DisplayText();
            while (!DialogueManager.Instance.DialogueEnded)
                yield return null;
            FinishedAction = true;
        }
    }

    private IEnumerator PerformSecondaryAction()
    {
        _effectText.Clear();
        Move move = _user.BattleStatus.ChosenMove;
        DoneWithSecondaryEffects = false;
        foreach (Effect effect in move.SecondaryEffects)
        {
            // Debug.Log(" - testing effect for - " + effect.Name);
            SetTargetBasedOnEffectTarget(effect.Target, TargetQueue);
            while (TargetQueue.Count > 0)
            {
                DoneWithSecondaryEffects = false;
                Target = TargetQueue.Dequeue();
                moveEffects = GetMoveEffects(Target);
                if (SecondaryEffectSuccessful(move, effect))
                {
                    // Debug.Log(" - implementing effect on - " + Target.Name);
                    yield return StartCoroutine(EffectAnimation(effect.Type.ToString()));
                    yield return StartCoroutine(SecondaryEffect(effect));
                    yield return new WaitForSeconds(0.25f);
                    DisplayText();
                    while (!DialogueManager.Instance.DialogueEnded)
                        yield return null;
                    _effectText.Clear();
                }
                // else
                // Debug.Log(" - effect did not activate");
            }
            // Debug.Log(" - no more targets for effects");
            Target = null;
            yield return null;
        }

        Debug.Log(" - done with secondary effects");
        DoneWithSecondaryEffects = true;
    }

    private IEnumerator PerformAfterRoundAction()
    {
        _effectText.Clear();
        FinishedAfterRound = false;
        if (TargetQueue.Count == 0)
        {
            Target = null;
            FinishedAfterRound = true;
            yield return null;
        }
        else
        {
            Target = TargetQueue.Dequeue();
            //TODO: check for ability
            //TODO: check for status condition that needs to be implemented
            foreach (StatusCondition statusCondition in Target.BattleStatus.StatusConditions.Values)
            {
                if (Target.BaseStats.Hp <= 0)
                    break;
                if (statusCondition.Condition.Equals("AFTER ROUND"))
                {
                    moveEffects = GetMoveEffects(Target);
                    yield return StartCoroutine(EffectAnimation(statusCondition.Name));
                    statusCondition.ImplementStatusCondition(Target);
                    UpdateBattleCharacter(Target, null, statusCondition.Name);
                    _effectText.Add(Target.Name + " was effected by the " + statusCondition.Name + "!");
                    DisplayText();
                    _effectText.Clear();
                    while (!DialogueManager.Instance.DialogueEnded)
                        yield return null;

                }
            }
            FinishedAfterRound = true;
        }
    }

    private IEnumerator PerformBeforeRoundAction()
    {
        _effectText.Clear();
        FinishedBeforeRound = false;
        if (TargetQueue.Count == 0)
        {
            Target = null;
            FinishedBeforeRound = true;
            yield return null;
        }
        else
        {
            while (TargetQueue.Count > 0)
            {
                Target = TargetQueue.Dequeue();
                //TODO: check for ability
                //TODO: check for items that the character is holding that may boost stats
                if (Target.Item != null && Target.Item.Type.Equals(ItemType.STAT_CHANGING))
                {
                    StatChangingItem item = (StatChangingItem)Target.Item;
                    moveEffects = GetMoveEffects(Target);
                    item.UseItem(Target);
                    _effectText.Add(Target.Name + " is using the " + item.Name + "!");
                    DisplayText();
                    while (!DialogueManager.Instance.DialogueEnded)
                        yield return null;
                    _effectText.Clear();
                    yield return StartCoroutine(EffectAnimation("STAT_CHANGE"));
                    for (int i = 0; i < item._stats.Length; i++)
                    {
                        string stat = item._stats[i];
                        int val = item._values[i];
                        string change = val > 0 ? "increased" : "decreased";
                        string stage = val > 1 || val < -1 ? "STAGES" : "STAGE";

                        _effectText.Add(Target.Name + " " + stat + " " + change + " " + val + " " + stage + "!");
                    }
                    DisplayText();
                    while (!DialogueManager.Instance.DialogueEnded)
                        yield return null;
                }
            }

            Target = null;
            FinishedBeforeRound = true;
            yield return null;
        }
    }

    private void SetTargetBasedOnEffectTarget(MoveTarget moveTarget, Queue<Character> queue)
    {
        TargetQueue.Clear();
        switch (moveTarget)
        {
            case MoveTarget.USER:
                TargetQueue.Enqueue(_user);
                break;
            default:
                foreach (BattleCharacter battleCharacter in _battleAllies)
                {
                    if (!_user.BattleStatus.ChosenMove.Name.Equals(battleCharacter.MoveHitWith))
                        continue;
                    if (battleCharacter.Character.BaseStats.Hp <= 0)
                        continue;

                    TargetQueue.Enqueue(battleCharacter.Character);
                }
                foreach (BattleCharacter battleCharacter in _battleEnemies)
                {
                    if (!_user.BattleStatus.ChosenMove.Name.Equals(battleCharacter.MoveHitWith))
                        continue;
                    if (battleCharacter.Character.BaseStats.Hp <= 0)
                        continue;

                    TargetQueue.Enqueue(battleCharacter.Character);
                }
                break;
        }
    }

    private IEnumerator ActionAnimation()
    {
        string offset = Target.Type.Equals("ENEMY") ? "enemy" : "ally";
        EnableAllCharacterHUD(false);
        yield return StartCoroutine(moveEffects.ActionAnimationRoutine(_user.BattleStatus.ChosenMove.Name, offset));
    }

    private IEnumerator EffectAnimation(string afterEffect)
    {
        //TODO: Create animation for effect        Debug.Log(" - " + afterEffect + " animation");
        yield return new WaitForSeconds(0.25f);
        switch (afterEffect)
        {
            case "FLASH":
                yield return StartCoroutine(moveEffects.FlashRoutine());
                break;
            case "HEAL":
                yield return StartCoroutine(moveEffects.HealRoutine());
                break;
            case "STAT_CHANGE":
                yield return StartCoroutine(moveEffects.StatRoutine());
                break;
            default:
                yield return StartCoroutine(moveEffects.FlashRoutine());
                break;
        }
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator RollToRunAnimation()
    {
        RollMechanic.gameObject.SetActive(true);
        yield return StartCoroutine(RollMechanic.RollRun());

        if (RollMechanic.RollNumber > 10)
        {
            _effectText.Add("You got away safely!");
            BattleSimStatus.RunSuccessful = true;
        }
        else
            _effectText.Add("You were unable to escape!");

        Target.BattleStatus.SetRollRun(false);
        RollMechanic.gameObject.SetActive(false);
    }

    private IEnumerator MoveEffect()
    {
        _user.BattleStatus.ChosenMove.UseMove(_user, Target, 1);
        if (_firstTimeMove)
        {
            _user.BattleStatus.ChosenMove.UpdateElixir(_user, 1);
            _firstTimeMove = false;
        }
        EnableAllCharacterHUD(true);
        BattleSimStatus.CheckGraveyardStatus(Target);
        if (Target.BaseStats.Hp <= 0)
            BattleSimStatus.AddToGraveYard(Target);
        UpdateBattleCharacter(_user, null, null);
        UpdateBattleCharacter(Target, _user.BattleStatus.ChosenMove.Name, null);
        SetMoveEffectString();
        // yield return new WaitForSeconds(1f);
        yield return null;
    }

    private IEnumerator ItemEffect()
    {
        Item item = _user.BattleStatus.ChosenItem;
        item.UseItem(Target);
        EnableAllCharacterHUD(true);
        BattleSimStatus.CheckGraveyardStatus(Target);
        UpdateInventory(item);
        UpdateBattleCharacter(Target, null, null);
        SetItemEffectString();
        // yield return new WaitForSeconds(1f);
        yield return null;
    }

    private IEnumerator SecondaryEffect(Effect effect)
    {
        string[] effectTexts = effect.UseEffect(Target);
        BattleSimStatus.CheckGraveyardStatus(Target);
        if (Target.BaseStats.Hp <= 0)
            BattleSimStatus.AddToGraveYard(Target);
        string statusCondition = null;
        if (effect.Type.Equals(EffectType.STATUS_CONDITION))
        {
            StatusConditionEffect statusConditionEffect = (StatusConditionEffect)effect;
            foreach (string text in effectTexts)
            {
                if (text.Contains("It does not effect") || text.Contains("does not stack"))
                {
                    statusCondition = null;
                    break;
                }
                else
                    statusCondition = statusConditionEffect._statusCondition.Name;
            }
        }
        _effectText.AddRange(effectTexts);
        UpdateBattleCharacter(Target, null, statusCondition);
        BattleSimStatus.CheckGraveyardStatus(Target);
        yield return new WaitForSeconds(1f);
    }

    private void UpdateInventory(Item item)
    {
        if (!_user.Equals(Player.Instance()))
            return;
        Player player = Player.Instance();
        switch (item.Type)
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
        TextBoxBattle.KeepTextBoxOpened = true;
        DialogueManager.Instance.CurrentStory = new Story(_dialogueData.InkJSON.text);

        for (int i = 0; i < _effectText.Count; i++)
        {
            int val = i + 1;
            string variable = val >= 2 ? "text" + val.ToString() : "text";
            DialogueManager.Instance.CurrentStory.variablesState[variable] = _effectText[i];
        }

        DialogueManager.Instance.TextBox = _textBox;
        DialogueManager.Instance.DisplayNextDialogue(_dialogueData);

        // _textBox.OpenTextBox();
        // _textBox.StartNarration(_dialogueData);
        StartedDialogue = true;
    }

    private void UpdateBattleCharacter(Character character, string moveHitWith, string statusCondition)
    {
        BattleCharacter battleCharacter = BattleSimStatus.GetBattleCharacter(character, _battlePlayer, _battleAllies, _battleEnemies);
        battleCharacter.UpdateHUD();

        if (!string.IsNullOrEmpty(moveHitWith))
            battleCharacter.MoveHitWith = moveHitWith;

        if (!string.IsNullOrEmpty(statusCondition))
            battleCharacter.CharacterHUD.AddStatusSymbol(BattleSimStatus.ReturnStatusConditionSymbol(statusCondition));
    }

    private void SetMoveEffectString()
    {
        if (_effect.Equals("MOVE"))
        {
            Move move = _user.BattleStatus.ChosenMove;
            _effectText.Clear();
            switch (move.Type)
            {
                case MoveType.REGULAR:
                case MoveType.PRIORITY:
                    _effectText.Add("It hit " + Target.Name + "!");
                    break;
                case MoveType.HEALING:
                    _effectText.Add(_user.Name + " healed " + Target.Name + "!");
                    break;
                case MoveType.STAT_CHANGING:
                    StatChangingMove statChangingMove = (StatChangingMove)move;
                    for (int i = 0; i < statChangingMove._stats.Length; i++)
                    {
                        string changed = statChangingMove._stages[i] > 0 ? " increased " : " decreased ";
                        int stageLevel = Mathf.Abs(statChangingMove._stages[i]);
                        _effectText.Add(Target.Name + "'s " + statChangingMove._stats[i] + changed + stageLevel + " stages!");
                    }
                    break;
                case MoveType.STATUS_CHANGING:
                    StatusChangingMove statusChangingMove = (StatusChangingMove)move;
                    if (Target.BattleStatus.StatusConditions[statusChangingMove._statusCondition.Name].Equals(statusChangingMove._statusCondition))
                        _effectText.Add(Target.Name + " is " + statusChangingMove._statusCondition.AfflictionText + "!");
                    else
                        _effectText.Add("The move failed!");
                    break;
                case MoveType.COUNTER:
                    string targetSex = "themselves";
                    if (Target.Sex.Equals("MALE"))
                        targetSex = "himself";
                    else if (Target.Sex.Equals("FEMALE"))
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
        if (!_effect.Equals("ITEM"))
            return;

        Item item = Player.Instance().BattleStatus.ChosenItem;
        switch (item.Type)
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
        if (_effect == "ITEM")
        {
            Item item = _user.BattleStatus.ChosenItem;
            switch (item.Type)
            {
                case ItemType.FOOD:
                    if (Target.BaseStats.Hp == Target.BaseStats.FullHp)
                    {
                        _effectText.Add("It had no effect!");
                        return false;
                    }
                    return true;
                case ItemType.MEDICAL:
                    MedicalItem medicalItem = (MedicalItem)item;
                    if (medicalItem._healAmount == 0)
                    {
                        foreach (string status in medicalItem._statusConditions)
                        {
                            if (status.Equals("ALL") || Target.BattleStatus.StatusConditions.ContainsKey(status))
                                return true;
                        }
                        _effectText.Add("It had no effect!");
                        return false;
                    }
                    else
                    {
                        if (Target.BaseStats.Hp == Target.BaseStats.FullHp)
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
        else if (_effect == "MOVE")
        {
            Move move = _user.BattleStatus.ChosenMove;
            if (move.DidMoveHit(_user, Target))
            {
                if (Target.BaseStats.Hp == 0)
                {
                    _effectText.Add(Target.Name + " is already knocked out!");
                    return false;
                }
                if (Target.BattleStatus.ProtectionStatus != "NONE" && !BattleSimStatus.OnSameSide(Target, _user))
                {
                    _effectText.Add(Target.Name + " was protected!");
                    return false;
                }
            }
            else
            {
                _effectText.Add("But the move missed!");
                return false;
            }
        }
        return true;
    }

    private bool SecondaryEffectSuccessful(Move move, Effect effect)
    {
        List<BattleCharacter> bcList = new()
        {
            _battlePlayer
        };
        bcList.AddRange(_battleAllies);
        bcList.AddRange(_battleEnemies);

        foreach (BattleCharacter bc in bcList)
        {
            if (bc == null || bc.Character == null)
                continue;
            if (!bc.Character.Equals(Target))
                continue;
            if (!bc.MoveHitWith.Equals(move.Name))
                continue;

            return (effect.Accuracy * 100) > UnityEngine.Random.Range(0, 100);
        }
        return false;
    }

    private MoveEffects GetMoveEffects(Character character)
    {
        BattleCharacter battleCharacter = BattleSimStatus.GetBattleCharacter(character, _battlePlayer, _battleAllies, _battleEnemies);
        if (battleCharacter != null)
            return battleCharacter.MoveEffects;
        return null;
    }

    private void EnableAllCharacterHUD(bool enable)
    {
        _battlePlayer.EnableHUD(enable);
        foreach (BattleCharacter ally in _battleAllies)
            ally.EnableHUD(enable);
        foreach (BattleCharacter enemy in _battleEnemies)
            enemy.EnableHUD(enable);
    }
}