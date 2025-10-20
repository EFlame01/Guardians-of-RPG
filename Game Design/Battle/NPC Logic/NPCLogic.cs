using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public int NPCLevel { get; private set; }

    //Constructors
    public NPCLogic(Character c)
    {
        character = c;
        CharacterAllies = new List<Character>();
        CharacterEnemies = new List<Character>();
        NPCLevel = 0;
    }

    public NPCLogic(Character c, int npcLevel)
    {
        character = c;
        CharacterAllies = new List<Character>();
        CharacterEnemies = new List<Character>();
        NPCLevel = npcLevel;
    }

    /// <summary>
    /// Determines the behavior of the NPC
    /// when it comes to selecting a move
    /// and a target for that move. The Level
    /// of strategy that the NPC will go through
    /// ranges from Level 0 to Level 3.
    /// </summary>
    public void DetermineBehaviour()
    {
        switch (NPCLevel)
        {
            case 0:
                LevelZeroBehavior();
                break;
            case 1:
                LevelOneBehavior();
                break;
            case 2:
                LevelTwoBehavior();
                break;
            case 3:
                LevelThreeBehavior();
                break;
            default:
                LevelZeroBehavior();
                break;
        }
    }

    /// <summary>
    /// Level 0 Behavior selects a random
    /// move and a random target.
    /// </summary>
    private void LevelZeroBehavior()
    {
        SetRandomMove();
        SetTargetList();
        SetRandomTargets();
    }

    /// <summary>
    /// Level 1 Behavior rolls the dice and
    /// has the chance to perform a random move
    /// or a specific move, and a random target
    /// or a specific target. The odds are not 
    /// weighted.
    /// </summary>
    private void LevelOneBehavior()
    {
        SetPartialSpecificMove(0);
        SetTargetList();
        SetPartialSpecificTargets(0);
    }

    /// <summary>
    /// Level 2 Behavior rolls the dice and
    /// has the chance to perform a random move
    /// or a specific move, and a random target
    /// or a specific target. The odds are not 
    /// weighted for selecting a move, but they
    /// are weighted for selecting a target.
    /// </summary>
    private void LevelTwoBehavior()
    {
        SetPartialSpecificMove(0);
        SetTargetList();
        SetPartialSpecificTargets(5);
    }

    /// <summary>
    /// Level 3 Behavior selects a specific
    /// move and a specific target. The most
    /// strategic of all the behaviors.
    /// </summary>
    private void LevelThreeBehavior()
    {
        SetSpecificMove();
        SetTargetList();
        SetSpecificTargets();
    }

    /// <summary>
    /// Creates a list of potential targets that
    /// the <c>Character</c> can have based on the
    /// character.
    /// </summary>
    private void SetTargetList()
    {
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
    private void SetRandomMove()
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
            if (CanUseMove(move, CharacterAllies, CharacterEnemies) && move.EP <= character.BaseStats.Elx)
                character.BattleStatus.ChosenMove = move;
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
    private void SetRandomTargets()
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
    private void SetSpecificMove()
    {
        int elixir = character.BaseStats.Elx;
        Move[] moves = character.BattleMoves;
        Move move = null;
        Dictionary<Move, int> moveFavorIndex = new Dictionary<Move, int>
        {
            { Units.BASE_ATTACK, 0 }
        };

        try
        {
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] == null)
                    continue;
                moveFavorIndex.Add(moves[i], 0);
                if (moves[i].EP > elixir)
                    moveFavorIndex[moves[i]]--;
                else
                    moveFavorIndex[moves[i]]++;
                if (CanUseMove(moves[i], CharacterAllies, CharacterEnemies))
                    moveFavorIndex[moves[i]]--;
                else
                    moveFavorIndex[moves[i]]++;
            }

            foreach (KeyValuePair<Move, int> moveIndex in moveFavorIndex)
            {
                if (move == null)
                    move = moveIndex.Key;
                else if (moveFavorIndex[move] < moveIndex.Value)
                    move = moveIndex.Key;
                else if (moveFavorIndex[move] == moveIndex.Value)
                {
                    //Determine the best move between the two.
                    //  - value elixir saving
                    //  - value easy KOs
                    //  - value helping allies
                    int elixirSaving = Random.Range(0, 100) + 1;
                    int easyKO = Random.Range(0, 100) + 1;
                    int helpingAllies = Random.Range(0, 100) + 1;

                    if (elixirSaving > easyKO && elixirSaving > helpingAllies)
                        move = moveIndex.Key.EP > move.EP ? move : moveIndex.Key;
                    else if (easyKO > elixirSaving && easyKO > helpingAllies)
                        move = moveIndex.Key.Power < move.Power ? move : moveIndex.Key;
                    else if (helpingAllies > elixirSaving && helpingAllies > easyKO)
                    {
                        if (moveIndex.Key.Target.Equals(MoveTarget.ALLY) || moveIndex.Key.Target.Equals(MoveTarget.ALL_ALLIES) || moveIndex.Key.Target.Equals(MoveTarget.ALLY_SIDE))
                            move = moveIndex.Key;
                    }
                    else
                        move = Random.Range(0, 100) + 1 > 50 ? move : moveIndex.Key;
                }
            }

            character.BattleStatus.ChosenMove = move;
        }
        catch (Exception e)
        {
            Debug.LogWarning("WARNING: " + e.Message);
            character.BattleStatus.ChosenMove = Units.BASE_ATTACK;
        }

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
    private void SetSpecificTargets()
    {
        Move move = character.BattleStatus.ChosenMove;
        if (move == null)
            return;

        switch (move.Target)
        {
            case MoveTarget.USER:
                character.BattleStatus.ChosenTargets.Add(character);
                break;
            case MoveTarget.ENEMY:
                if (CharacterEnemies.Count == 1)
                    character.BattleStatus.ChosenTargets.Add(CharacterEnemies[0]);
                else
                {
                    Character enemyTarget = null;
                    switch (move.Type)
                    {
                        case MoveType.REGULAR:
                            foreach (Character enemy in CharacterEnemies)
                            {
                                if (enemyTarget == null)
                                    enemyTarget = enemy;
                                else if (enemy.BaseStats.Hp < enemyTarget.BaseStats.Hp)
                                    enemyTarget = enemy;
                            }
                            break;
                        case MoveType.PRIORITY:
                            foreach (Character enemy in CharacterEnemies)
                            {
                                if (enemyTarget == null)
                                    enemyTarget = enemy;
                                else if (enemy.BaseStats.Spd > enemyTarget.BaseStats.Spd || enemy.BaseStats.Spd > character.BaseStats.Spd)
                                    enemyTarget = enemy;
                            }
                            break;
                        case MoveType.KNOCK_OUT:
                            foreach (Character enemy in CharacterEnemies)
                            {
                                if (enemyTarget == null)
                                    enemyTarget = enemy;
                                else if (enemy.BaseStats.Hp > enemyTarget.BaseStats.Hp)
                                    enemyTarget = enemy;
                            }
                            break;
                        default:
                            foreach (Character enemy in CharacterEnemies)
                            {
                                if (enemyTarget == null)
                                    enemyTarget = enemy;
                                else if (enemy.BaseStats.Hp < enemyTarget.BaseStats.Hp)
                                    enemyTarget = enemy;
                            }
                            break;
                    }
                    character.BattleStatus.ChosenTargets.Add(enemyTarget);
                }
                break;
            case MoveTarget.ALL_ENEMIES:
                character.BattleStatus.ChosenTargets.AddRange(CharacterEnemies);
                break;
            case MoveTarget.ALLY:
                if (CharacterAllies.Count == 1)
                    character.BattleStatus.ChosenTargets.Add(CharacterAllies[0]);
                else
                {
                    Character allyTarget = null;
                    Character ally1 = CharacterAllies[0];
                    Character ally2 = CharacterAllies[2];
                    switch (move.Type)
                    {
                        case MoveType.HEALING:
                            if (ally1.BaseStats.Hp > ally2.BaseStats.Hp)
                                character.BattleStatus.ChosenTargets.Add(ally2);
                            else
                                character.BattleStatus.ChosenTargets.Add(ally1);
                            break;
                        case MoveType.STAT_CHANGING:
                            StatChangingMove stMove = (StatChangingMove)move;
                            switch (stMove._stats[0])
                            {
                                case "ATK":
                                    if (ally1.BaseStats.Atk > ally2.BaseStats.Atk)
                                        character.BattleStatus.ChosenTargets.Add(ally2);
                                    else
                                        character.BattleStatus.ChosenTargets.Add(ally1);
                                    break;
                                case "DEF":
                                    if (ally1.BaseStats.Def > ally2.BaseStats.Def)
                                        character.BattleStatus.ChosenTargets.Add(ally2);
                                    else
                                        character.BattleStatus.ChosenTargets.Add(ally1);
                                    break;
                                case "EVA":
                                    if (ally1.BaseStats.Eva > ally2.BaseStats.Eva)
                                        character.BattleStatus.ChosenTargets.Add(ally2);
                                    else
                                        character.BattleStatus.ChosenTargets.Add(ally1);
                                    break;
                                case "SPD":
                                    if (ally1.BaseStats.Spd > ally2.BaseStats.Spd)
                                        character.BattleStatus.ChosenTargets.Add(ally2);
                                    else
                                        character.BattleStatus.ChosenTargets.Add(ally1);
                                    break;
                                case "HP":
                                    if (ally1.BaseStats.Hp > ally2.BaseStats.Hp)
                                        character.BattleStatus.ChosenTargets.Add(ally2);
                                    else
                                        character.BattleStatus.ChosenTargets.Add(ally1);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            foreach (Character ally in CharacterAllies)
                            {
                                if (allyTarget == null)
                                    allyTarget = ally;
                                else if (allyTarget.BaseStats.Hp > ally.BaseStats.Hp)
                                    allyTarget = ally;
                            }
                            character.BattleStatus.ChosenTargets.Add(allyTarget);
                            break;
                    }
                }
                break;
            case MoveTarget.ALL_ALLIES:
                character.BattleStatus.ChosenTargets.Add(character);
                character.BattleStatus.ChosenTargets.AddRange(CharacterAllies);
                break;
            case MoveTarget.ALLY_SIDE:
                character.BattleStatus.ChosenTargets.AddRange(CharacterAllies);
                break;
            case MoveTarget.EVERYONE:
                character.BattleStatus.ChosenTargets.AddRange(CharacterEnemies);
                character.BattleStatus.ChosenTargets.AddRange(CharacterAllies);
                break;
        }
    }

    /// <summary>
    /// Determines based on probability if the NPC
    /// will select a base attack, a random move, or 
    /// a specific move. This probability is also
    /// determined by the <paramref name="weight"/>.
    /// </summary>
    /// <param name="weight">The degree in which the NPC will think critically.</param>
    private void SetPartialSpecificMove(int weight)
    {
        int thinkOption = Random.Range(0, 20) + 1 + weight;
        Mathf.Clamp(thinkOption, 0, 20);
        if (thinkOption < 5)
            character.BattleStatus.ChosenMove = Units.BASE_ATTACK;
        else if (thinkOption <= 10)
            SetRandomMove();
        else
            SetSpecificMove();
    }

    /// <summary>
    /// Determines based on probability if the NPC
    /// will select a random target or a specific
    /// target. This probability is also
    /// determined by the <paramref name="weight"/>.
    /// </summary>
    /// <param name="weight">The degree in which the NPC will think critically.</param>
    private void SetPartialSpecificTargets(int weight)
    {
        int thinkOption = Random.Range(0, 20) + 1 + weight;
        Mathf.Clamp(thinkOption, 0, 20);
        if (thinkOption <= 10)
            SetRandomTargets();
        else
            SetSpecificTargets();
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

    /// <summary>
    /// Determines if there are targets the NPC can use a <paramref name="move"/> on
    /// based on the <paramref name="allies"/> list and the <paramref name="enemies"/>
    /// list. If there are targets available for the move to work, it will return <c>TRUE</c>.
    /// Otherwise it will return <c>FALSE</c>.
    /// </summary>
    /// <param name="move">The move the NPC wishes to use</param>
    /// <param name="allies">The list of allies that the NPC would have to choose from to use the move.</param>
    /// <param name="enemies">The list of enemies that the NPC would have to choose from to use the move.</param>
    /// <return><c>TRUE</c> if there are targets to choose from. <c>FALSE</c> if otherwise.</return>
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