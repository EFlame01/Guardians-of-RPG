using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPCLogic is a class that determines
/// the action that the <c>Character</c>s
/// will take during battle.
/// </summary>
public class NPCLogic
{
    //public variables
    public Character character;
    public List<Character> CharacterAllies { get; private set; }
    public List<Character> CharacterEnemies { get; private set; }

    //Constructor
    public NPCLogic(Character c)
    {
        character = c;
        CharacterAllies = new List<Character>();
        CharacterEnemies = new List<Character>();
    }

    /// <summary>
    /// Creates a list of potential targets that
    /// the <c>Character</c> can have based on the
    /// character.
    /// </summary>
    public void SetTargetList()
    {
        List<Character> targets = new List<Character>();
        Player player = Player.Instance();
        string type = character.Type;

        if (type.Equals("ALLY"))
            AddCharactersToTargetList(CharacterAllies, CharacterEnemies, false);
        else if (type.Equals("ENEMY"))
            AddCharactersToTargetList(CharacterEnemies, CharacterAllies, true);
        else
            Debug.Log(type);
    }

    /// <summary>
    /// Sets a random <c>Move</c> for the <c>Character</c>
    /// based on the following:
    /// <list type="bullet">
    ///     <item>The variable character</item>
    ///     <item>The moves available to the <c>Character</c></item>
    /// </list>
    /// </summary>
    public void SetRandomMove()
    {
        List<Move> allMoves = new List<Move>();

        foreach (Move move in character.BattleMoves)
        {
            if (move != null)
                allMoves.Add(move);
        }

        if (allMoves.Count > 0)
        {
            int randomIndex = Random.Range(0, 100) % allMoves.Count;
            Move move = character.BattleMoves[randomIndex];
            if (CanUseMove(move, CharacterAllies, CharacterEnemies))
                character.BattleStatus.ChosenMove = character.BattleMoves[randomIndex];
            else
                character.BattleStatus.ChosenMove = Units.BASE_ATTACK;
        }
        else
            character.BattleStatus.ChosenMove = Units.BASE_ATTACK;

    }

    /// <summary>
    /// Sets a random target for the <c>Character</c>
    /// based on the following:
    /// <list type="bullet">
    ///     <item>The variable character</item>
    ///     <item>The move the <c>Character</c> selected</item>
    ///     <item>The targets available in the battle</item>
    /// </list>
    /// </summary>
    public void SetRandomTargets()
    {
        Move move = character.BattleStatus.ChosenMove;
        int allyIndex = CharacterAllies.Count <= 0 ? -1 : Random.Range(0, 100) % CharacterAllies.Count;
        int enemyIndex = CharacterEnemies.Count <= 0 ? -1 : Random.Range(0, 100) % CharacterEnemies.Count;

        if (move == null)
            return;

        switch (move.Target)
        {
            case MoveTarget.USER:
                character.BattleStatus.ChosenTargets.Add(character);
                break;
            case MoveTarget.ENEMY:
                if (enemyIndex != -1)
                    character.BattleStatus.ChosenTargets.Add(CharacterEnemies[enemyIndex]);
                break;
            case MoveTarget.ALL_ENEMIES:
                character.BattleStatus.ChosenTargets.AddRange(CharacterEnemies.ToArray());
                break;
            case MoveTarget.ALLY:
                if (allyIndex != -1)
                    character.BattleStatus.ChosenTargets.Add(CharacterAllies[allyIndex]);
                break;
            case MoveTarget.ALL_ALLIES:
                character.BattleStatus.ChosenTargets.AddRange(CharacterAllies.ToArray());
                break;
            case MoveTarget.ALLY_SIDE:
                character.BattleStatus.ChosenTargets.Add(character);
                character.BattleStatus.ChosenTargets.AddRange(CharacterAllies.ToArray());
                break;
            case MoveTarget.EVERYONE:
                character.BattleStatus.ChosenTargets.AddRange(CharacterEnemies.ToArray());
                character.BattleStatus.ChosenTargets.AddRange(CharacterAllies.ToArray());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Sets a specific <c>Move</c> for the <c>Character</c>
    /// based on the following:
    /// <list type="bullet">
    ///     <item>The variable character</item>
    ///     <item>The moves available to the <c>Character</c></item>
    ///     <item>How much elixir the <c>Character</c> has</item>
    ///     <item>How much health the <c>Character</c> has</item>
    ///     <item>How much health the other <c>Character</c>s have</item>
    /// </list>
    /// (THIS METHOD IS NOT COMPLETE. DO NOT USE)
    /// </summary>
    public void SetSpecificMove()
    {
        //TODO: create sophisticated algorithm to determine the best choice to use
    }

    /// <summary>
    /// Sets a specific target for the <c>Character</c>
    /// based on the following:
    /// <list type="bullet">
    ///     <item>The variable character</item>
    ///     <item>The move the <c>Character</c> selected</item>
    ///     <item>The targets available in the battle</item>
    ///     <item>How much health the potential targets have</item>
    /// </list>
    /// (THIS METHOD IS NOT COMPLETE. DO NOT USE)
    /// </summary>
    public void SetSpecificTarget()
    {
        //TODO: create sophisticated algorithm to determine the best target to select
    }

    /// <summary>
    /// Sorts characters in Battle into list of 
    /// <paramref name="allies"/>, <paramref name="enemies"/>,
    /// based on if the character is an ALLY or ENEMY, and the 
    /// variable <paramref name="isPlayerEnemy"/>
    /// </summary>
    /// <param name="allies">The list filled with the character's allies</param>
    /// <param name="enemies">The list filled with the character's enemies</param>
    /// <param name="isPlayerEnemy">Boolean value to determine which list to put the player in</param>
    private void AddCharactersToTargetList(List<Character> allies, List<Character> enemies, bool isPlayerEnemy)
    {
        Player player = Player.Instance();
        if (player.BaseStats.Hp > 0)
            allies.Add(player);

        foreach (Character c in BattleSimStatus.Allies)
        {
            if (c.BaseStats.Hp > 0 && !c.Id.Equals(character.Id))
                allies.Add(c);
        }

        foreach (Character c in BattleSimStatus.Enemies)
        {
            if (c.BaseStats.Hp > 0 && !c.Id.Equals(character.Id))
                enemies.Add(c);
        }
    }

    private bool CanUseMove(Move move, List<Character> allies, List<Character> enemies)
    {
        MoveTarget moveTarget = move.Target;

        switch (moveTarget)
        {
            case MoveTarget.USER:
            case MoveTarget.EVERYONE:
            case MoveTarget.ALL_ALLIES:
                return true;
            case MoveTarget.ENEMY:
            case MoveTarget.ALL_ENEMIES:
                if (enemies.Count == 0)
                    return false;
                else
                    return true;
            case MoveTarget.ALLY:
            case MoveTarget.ALLY_SIDE:
                if (allies.Count == 0)
                    return false;
                else
                    return true;
            default:
                return true;
        }
    }
}