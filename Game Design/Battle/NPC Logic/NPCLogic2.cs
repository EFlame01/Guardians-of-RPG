using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCLogic2
{
    //public variables
    public Character character;
    public List<Character> CharacterAllies { get; private set; }
    public List<Character> CharacterEnemies { get; private set; }
    public int NPCLevel { get; private set; }

    private int _dealDamage;
    private int _provideSupport;
    private int _savingElixir;

    public NPCLogic2(Character c)
    {
        character = c;
        CharacterAllies = new List<Character>();
        CharacterEnemies = new List<Character>();
        NPCLevel = 0;
        _dealDamage = 0;
        _provideSupport = 0;
        _savingElixir = 0;
    }
    public NPCLogic2(Character c, int npcLevel)
    {
        character = c;
        CharacterAllies = new List<Character>();
        CharacterEnemies = new List<Character>();
        NPCLevel = npcLevel;
        _dealDamage = 0;
        _provideSupport = 0;
        _savingElixir = 0;
    }

    private void GenerateDecisionWeights()
    {
        CalculateAvgHealth();
        CalculateBaseStatRatio();
        CalculateElixir();

        if (_dealDamage >= _provideSupport && _dealDamage >= _savingElixir)
        {
            //deal damange
            DealDamage();
        }
        else if (_provideSupport >= _dealDamage && _provideSupport >= _savingElixir)
        {
            //provide support
            ProvideSupport();
        }
        else if (_savingElixir >= _dealDamage && _savingElixir >= _provideSupport)
        {
            //save elixir
            SaveElixir();
        }
        else
        {
            int option = Random.Range(1, 4);
            switch (option)
            {
                case 1:
                    //deal damage
                    DealDamage();
                    break;
                case 2:
                    //provide support
                    ProvideSupport();
                    break;
                case 3:
                    //save elixir
                    SaveElixir();
                    break;
                default:
                    //deal damage
                    DealDamage();
                    break;
            }
        }
    }

    private void CalculateAvgHealth()
    {
        double avgEnemyHealth = CalculateAvgHealth(CharacterEnemies);
        double avgAllyHealth = CalculateAvgHealth(CharacterAllies);
        double avgCharacterHealth = (double)character.BaseStats.Hp / (double)character.BaseStats.FullHp;


        //comparing enemies' health to static doubles
        if (avgEnemyHealth > .50)
        {
            _dealDamage += 2;
            _provideSupport -= 2;
            _savingElixir -= 1;
        }
        else if (avgEnemyHealth > .25)
        {
            _dealDamage += 0;
            _provideSupport += 1;
            _savingElixir += 1;
        }
        else if (avgEnemyHealth < .25)
        {
            _dealDamage += 0;
            _provideSupport += 0;
            _savingElixir += 0;
        }

        //comparing character's health to enemies' health
        if (avgCharacterHealth > avgEnemyHealth)
        {
            _dealDamage += 0;
            _provideSupport += 2;
            _savingElixir += 0;
        }
        else if (avgCharacterHealth < avgEnemyHealth)
        {
            _dealDamage += 2;
            _provideSupport -= 2;
            _savingElixir -= 1;
        }
        else if (avgCharacterHealth == avgEnemyHealth)
        {
            _dealDamage += 0;
            _provideSupport += 0;
            _savingElixir += 0;
        }

        //checking if allies should be compared
        if (CharacterAllies.Count < 0)
            return;

        //comparing allies' health to enemies' health
        if (avgAllyHealth > avgEnemyHealth)
        {
            _dealDamage += 1;
            _provideSupport += 1;
            _savingElixir += 0;
        }
        else if (avgAllyHealth < avgEnemyHealth)
        {
            _dealDamage += 1;
            _provideSupport += 2;
            _savingElixir += 0;
        }
        else if (avgAllyHealth == avgEnemyHealth)
        {
            _dealDamage += 0;
            _provideSupport += 0;
            _savingElixir += 0;
        }
    }

    private double CalculateAvgHealth(List<Character> characters)
    {
        double totalHp = 0;
        double totalFullHp = 0;

        if (characters.Count == 0)
            return 0;

        foreach (Character c in characters)
        {
            totalHp += c.BaseStats.Hp;
            totalFullHp += c.BaseStats.FullHp;
        }
        return totalHp / totalFullHp;
    }

    private void CalculateBaseStatRatio()
    {
        double enemyBSTRatio = CalculateBaseStatRatio(CharacterEnemies);
        double allyBSTRatio = CalculateBaseStatRatio(CharacterAllies);
        double characterBSTRatio = 1;

        //comparing enemy bst ratio to character
        if (enemyBSTRatio > characterBSTRatio)
        {
            _dealDamage += 1;
            _provideSupport += 2;
            _savingElixir -= 1;
        }
        else if (enemyBSTRatio < characterBSTRatio)
        {
            _dealDamage += 1;
            _provideSupport -= 1;
            _savingElixir -= 1;
        }
        else
        {
            _dealDamage += 0;
            _provideSupport += 0;
            _savingElixir += 0;
        }

        //checking if allyBSTRatio should impact weights
        if (CharacterAllies.Count == 0)
            return;

        //comparing enemy bst ratio to ally
        if (enemyBSTRatio > allyBSTRatio)
        {
            _dealDamage += 1;
            _provideSupport += 2;
            _savingElixir -= 1;
        }
        else if (enemyBSTRatio < allyBSTRatio)
        {
            _dealDamage += 1;
            _provideSupport -= 1;
            _savingElixir += 0;
        }
        else
        {
            _dealDamage += 0;
            _provideSupport += 0;
            _savingElixir += 0;
        }
    }

    private double CalculateBaseStatRatio(List<Character> characters)
    {
        double bst = 0;

        if (characters.Count == 0)
            return 0;

        foreach (Character c in characters)
        {
            bst += c.BaseStats.GetBaseStatTotal();
        }

        return bst / character.BaseStats.GetBaseStatTotal();
    }

    private Character CompareMostStats(Character c1, Character c2, int statIndex)
    {
        if (c1 == null || c2 == null)
            return null;

        return statIndex switch
        {
            Units.ATK_INDEX => c1.BaseStats.Atk >= c2.BaseStats.Atk ? c1 : c2,
            Units.DEF_INDEX => c1.BaseStats.Def >= c2.BaseStats.Def ? c1 : c2,
            Units.EVA_INDEX => c1.BaseStats.Eva >= c2.BaseStats.Eva ? c1 : c2,
            Units.SPD_INDEX => c1.BaseStats.Spd >= c2.BaseStats.Spd ? c1 : c2,
            Units.HP_INDEX => c1.BaseStats.Hp >= c2.BaseStats.Hp ? c1 : c2,
            _ => null,
        };
    }

    private Character CompareLeastStats(Character c1, Character c2, int statIndex)
    {
        if (c1 == null || c2 == null)
            return null;

        return statIndex switch
        {
            Units.ATK_INDEX => c1.BaseStats.Atk <= c2.BaseStats.Atk ? c1 : c2,
            Units.DEF_INDEX => c1.BaseStats.Def <= c2.BaseStats.Def ? c1 : c2,
            Units.EVA_INDEX => c1.BaseStats.Eva <= c2.BaseStats.Eva ? c1 : c2,
            Units.SPD_INDEX => c1.BaseStats.Spd <= c2.BaseStats.Spd ? c1 : c2,
            Units.HP_INDEX => c1.BaseStats.Hp <= c2.BaseStats.Hp ? c1 : c2,
            _ => null,
        };
    }

    private void CalculateElixir()
    {
        // avgElixirCost/totalElixir : prime elixir percent (PEP)
        // currentElixir/totalElixir : actual elixir percent (AEP)
        // elixir ratio: AEP/PEP
        // ER > 1: has more than average elixir to perform all techniques
        // 1 > ER > 0.5: has decent amount of elixir to perform all techniques
        // 0.5 > ER 0.25: has low amount of elixir to perform a few techniques
        // ER < 0.25: not enough elixir to perform techniques

        double pep = (double)character.GetAverageElixirCost() / (double)character.BaseStats.GetFullElx();
        double aep = (double)character.BaseStats.Elx / (double)character.BaseStats.GetFullElx();
        double threashold = aep / pep;

        if (threashold > 1)
        {
            _dealDamage += 2;
            _provideSupport += 2;
            _savingElixir -= 2;
        }
        else if (threashold > .5)
        {
            _dealDamage += 1;
            _provideSupport += 1;
            _savingElixir += 1;
        }
        else if (threashold > .25)
        {
            _dealDamage -= 1;
            _provideSupport -= 1;
            _savingElixir += 1;
        }
        else
        {
            _dealDamage -= 2;
            _provideSupport -= 2;
            _savingElixir += 3;
        }
    }

    private Character SelectTargetBasedOnStats(List<Character> characters, int statIndex, string leastOrMost)
    {
        Character target = null;

        for (int i = 0; i < characters.Count; i++)
        {
            for (int j = i + 1; j < characters.Count; j++)
            {
                switch (leastOrMost)
                {
                    case "LEAST":
                        target = CompareLeastStats(characters[i], characters[j], statIndex);
                        break;
                    case "MOST":
                        target = CompareMostStats(characters[i], characters[j], statIndex);
                        break;
                }
            }
        }

        return target;
    }

    private int CalculateHpLeft(Character target, Move move)
    {
        int userAtk = character.BaseStats.Atk;
        int targetDef = target.BaseStats.Def;
        int damage = Mathf.Clamp(userAtk - targetDef, 1, userAtk);
        return target.BaseStats.Hp - damage;
    }

    private void UpdateMoveAndTarget(Move move, Character target)
    {
        character.BattleStatus.ChosenTargets.Clear();
        character.BattleStatus.ChosenMove = move;
        if (move.Target.Equals(MoveTarget.ALL_ENEMIES))
            character.BattleStatus.ChosenTargets.AddRange(CharacterEnemies);
        else if (move.Target.Equals(MoveTarget.EVERYONE))
        {
            character.BattleStatus.ChosenTargets.AddRange(CharacterEnemies);
            character.BattleStatus.ChosenTargets.AddRange(CharacterAllies);
        }
        else
            character.BattleStatus.ChosenTargets.Add(target);
    }

    private void DealDamage()
    {
        //find technique character has that deals damage
        List<Move> damageMoves = new List<Move>();
        foreach (Move move in character.BattleMoves)
        {
            if (move != null && move.Power > 0)
                damageMoves.Add(move);
        }

        //determine target by who has least amount of health or defense
        Character target1 = SelectTargetBasedOnStats(CharacterEnemies, Units.HP_INDEX, "LEAST");
        Character target2 = SelectTargetBasedOnStats(CharacterEnemies, Units.DEF_INDEX, "LEAST");

        //determine move and target based on what deals the highest damage
        int hpLeft = target1.BaseStats.Hp + target2.BaseStats.Hp;
        foreach (Move move in damageMoves)
        {
            int hpLeft1 = CalculateHpLeft(target1, move);
            int hpLeft2 = CalculateHpLeft(target2, move);

            if (hpLeft1 < hpLeft && move.EP <= character.BaseStats.Elx)
            {
                hpLeft = hpLeft1;
                UpdateMoveAndTarget(move, target1);
            }
            if (hpLeft2 < hpLeft && move.EP <= character.BaseStats.Elx)
            {
                hpLeft = hpLeft2;
                UpdateMoveAndTarget(move, target2);
            }
        }

        //if there was no move/target selected, choose base attack
        if (character.BattleStatus.ChosenMove == null)
        {
            Character target = Random.Range(0, 100) + 1 > 50 ? target1 : target2;
            UpdateMoveAndTarget(Units.BASE_ATTACK, target);
        }
    }

    private void ProvideSupport()
    {
        /*
            TODO: 
            Several ways to provide support:
            - perform healing move
            - perform stat/status changing moves
            - perform priority move
            - perform protect move

            determine the best thing the character
            can do based on move set and circumstances
        */
    }

    private void SaveElixir()
    {
        /*  
            TODO: 
            Several ways to save elixir
            - use base attack
            - use least elixir costing move

            determine the best way to save elixir
            based on the move set and circumstances
        */
    }
}