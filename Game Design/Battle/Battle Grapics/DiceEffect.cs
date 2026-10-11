using System.Collections;
using UnityEngine;

/// <summary>
/// DiceEffect is a class that handles the 
/// dice rolls animations for the characters
/// while they are battling.
/// </summary>
public class DiceEffect : MonoBehaviour
{

    //public methods
    public Animator RollAnimator;
    public BattleCharacter[] BattleCharacters;
    public int RollNumber { get; private set; }

    //private  methods
    private string DEFAULT_DICE_ANIMATION = "default";

    public IEnumerator RollRun(Character character)
    {
        RollNumber = DiceRolls.GetInstance().RollD20(character);
        PlayRollAnimation(RollAnimator);
        yield return new WaitForSeconds(1.25f);
    }

    /// <summary>
    /// This method allows all characters who selected the option
    /// to roll for initiative for that round.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RollInitiative()
    {
        foreach (BattleCharacter battleCharacter in BattleCharacters)
        {
            if (battleCharacter == null || battleCharacter.Character == null)
                continue;
            if (battleCharacter.Character.BaseStats.Hp <= 0)
                continue;
            if (!battleCharacter.Character.BattleStatus.TurnStatus.Equals(TurnStatus.INITIATIVE))
                continue;

            RollNumber = DiceRolls.GetInstance().RollD20(battleCharacter.Character);
            battleCharacter.RollInitiativeNumber = RollNumber;
            battleCharacter.Character.BattleStatus.InitiativeStatus.SetInitiativeRollValue(RollNumber);
            battleCharacter.Character.BattleStatus.InitiativeStatus.SetInitiativeCount(battleCharacter.Character.BattleStatus.InitiativeStatus.InitiativeCount - 1);
            PlayRollAnimation(battleCharacter.DieAnimator);

            yield return new WaitForSeconds(0.33F);

            if (RollNumber <= 5)
                battleCharacter.Character.BattleStatus.InitiativeStatus.SetInitiativeLevel(TurnStatus.MOVE_LAST);
            else if (RollNumber >= 6 && RollNumber <= 10)
                battleCharacter.Character.BattleStatus.InitiativeStatus.SetInitiativeLevel(TurnStatus.NOTHING);
            else if (RollNumber >= 11 && RollNumber <= 15)
                battleCharacter.Character.BattleStatus.InitiativeStatus.SetInitiativeLevel(TurnStatus.PRIORITY_2);
            else if (RollNumber >= 16 && RollNumber < 20)
                battleCharacter.Character.BattleStatus.InitiativeStatus.SetInitiativeLevel(TurnStatus.PRIORITY_1);
            else if (RollNumber == 20)
                battleCharacter.Character.BattleStatus.InitiativeStatus.SetInitiativeLevel(TurnStatus.MOVE_FIRST);

            battleCharacter.Character.BattleStatus.InitiativeStatus.SetRollInitiative(false);
        }

        yield return new WaitForSeconds(1.5F);

        foreach (BattleCharacter battleCharacter in BattleCharacters)
        {
            if (battleCharacter == null || battleCharacter.Character == null)
                continue;
            if (battleCharacter.Character.BaseStats.Hp <= 0)
                continue;
            PlayDefaultDieAnimation(battleCharacter.DieAnimator);
        }
    }

    /// <summary>
    /// This method displays the roll animation
    /// for a specific <paramref name="character"/> that has chosen
    /// to block. It searches for this character in
    /// the list of <c>BattleCharacter</c>s and performs 
    /// the animation.
    /// </summary>
    /// <param name="character">The character blocking.</param>
    /// <returns></returns>
    public IEnumerator RollBlock(Character character)
    {
        RollNumber = DiceRolls.GetInstance().RollD20(character);
        foreach (BattleCharacter battleCharacter in BattleCharacters)
        {
            if (battleCharacter == null || battleCharacter.Character == null)
                continue;
            if (battleCharacter.Character.BaseStats.Hp <= 0)
                continue;
            if (battleCharacter.Character.Equals(character))
            {
                character.BattleStatus.BlockStatus.SetBlockValue(RollNumber);
                character.BattleStatus.BlockStatus.SetBlockCount(character.BattleStatus.BlockStatus.BlockCount - 1);
                PlayRollAnimation(battleCharacter.DieAnimator);
                yield return new WaitForSeconds(1f);
            }
        }

        yield return new WaitForSeconds(.5F);

        foreach (BattleCharacter battleCharacter in BattleCharacters)
        {
            if (battleCharacter == null || battleCharacter.Character == null)
                continue;
            if (battleCharacter.Character.BaseStats.Hp <= 0)
                continue;
            PlayDefaultDieAnimation(battleCharacter.DieAnimator);
        }
    }

    /// <summary>
    /// PlayRollAnimation plays the dice roll animation
    /// based on the <paramref name="animator"/> and the 
    /// <c>RollNumber</c> provided. If the number does not
    /// exist as an animation (0, -n, or anything above 20)
    /// Then the animation will not play.
    /// </summary>
    /// <param name="animator">The gameobject that has the animation</param>
    public void PlayRollAnimation(Animator animator)
    {
        if (animator.HasState(0, Animator.StringToHash(RollNumber.ToString())))
            animator.Play(RollNumber.ToString());
        else
            Debug.LogWarning("WARNING: Animation " + RollNumber.ToString() + " could not be played...");
    }

    /// <summary>
    /// PlayDefaultDieAnimation sets the dice animation
    /// to the default state (invisible) to get the animation
    /// ready for the next dice roll.
    /// </summary>
    /// <param name="animator">The gameobject that has the animation</param>
    private void PlayDefaultDieAnimation(Animator animator)
    {
        if (animator.HasState(0, Animator.StringToHash(DEFAULT_DICE_ANIMATION)))
            animator.Play(DEFAULT_DICE_ANIMATION);
        else
            Debug.LogWarning("WARNING: Animation " + RollNumber.ToString() + " could not be played...");
    }
}