using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleOrderBST is a class that determines
/// the order the <c>Character</c>s will have
/// in order by using a Binary Search Tree (BST).
/// </summary>
public class BattleOrderBST
{
    //public variables
    public BST CharacterBST { get; private set; }

    /// <summary>
    /// BST is a subclass that acts as the Binary
    /// Search Tree for <c>BattleOrderBST</c>.
    /// </summary>
    public class BST
    {
        //public variables
        public Character character;
        public BST left;
        public BST right;

        //Constructor
        public BST(Character c)
        {
            character = c;
            left = null;
            right = null;
        }
    }

    //Constructor
    public BattleOrderBST()
    {
        CharacterBST = new BST(Player.Instance());
    }

    /// <summary>
    /// Adds and arranges all the <c>Character</c>s 
    /// in battle into a BST. The BST is arranged by 
    /// the following order:
    /// <list type="bullet">
    ///     <item>RUNNING</item>
    ///     <item>ITEM SELECTION</item>
    ///     <item>INITIATIVE</item>
    ///     <item>PROTECT MOVE</item>
    ///     <item>COUNTER MOVE</item>
    ///     <item>PRIORITY 1</item>
    ///     <item>PRIORITY 2</item>
    ///     <item>PRIORITY 3</item>
    ///     <item>FIGHT</item>
    ///     <item>SKIP</item>
    ///     <item>NOTHING</item>
    /// </list>
    /// </summary>
    public void ArrangeBST()
    {
        BattleSimStatus.BattleQueue.Clear();

        //Add all characters to list
        List<Character> list = new List<Character>();
        list.AddRange(BattleSimStatus.Allies.ToArray());
        list.AddRange(BattleSimStatus.Enemies.ToArray());

        //Determine player's turn
        DetermineTurn(Player.Instance());

        //Determine every other character's turn
        //  and add them to BST
        foreach (Character c in list)
        {
            DetermineTurn(c);
            BST bst = CharacterBST;
            AddBST(ref bst, c);
        }

        OrderBattleQueue(CharacterBST);
    }

    private void OrderBattleQueue(BST bst)
    {
        //go through character BST from left to right and add to list
        if (bst.left != null)
            OrderBattleQueue(bst.left);

        BattleSimStatus.BattleQueue.Enqueue(bst.character);

        if (bst.right != null)
            OrderBattleQueue(bst.right);
    }

    private void DetermineTurn(Character c)
    {
        if (c.BattleStatus.RollRun)
        {
            c.BattleStatus.SetTurnStatus(TurnStatus.RUN);
            return;
        }
        if (c.BattleStatus.ChosenItem != null)
        {
            c.BattleStatus.SetTurnStatus(TurnStatus.ITEM);
        }
        else if (c.BattleStatus.RollInitiative)
        {
            c.BattleStatus.SetTurnStatus(TurnStatus.INITIATIVE);
        }
        else if (c.BattleStatus.ChosenMove != null)
        {
            Move move = c.BattleStatus.ChosenMove;
            switch (move.Type)
            {
                case MoveType.PROTECT:
                    c.BattleStatus.SetTurnStatus(TurnStatus.PROTECT);
                    break;
                case MoveType.COUNTER:
                    c.BattleStatus.SetTurnStatus(TurnStatus.COUNTER);
                    break;
                case MoveType.PRIORITY:
                    PriorityMove priorityMove = (PriorityMove)move;
                    switch (priorityMove.PriorityLevel)
                    {
                        case 1:
                            c.BattleStatus.SetTurnStatus(TurnStatus.PRIORITY_1);
                            break;
                        case 2:
                            c.BattleStatus.SetTurnStatus(TurnStatus.PRIORITY_2);
                            break;
                        case 3:
                            c.BattleStatus.SetTurnStatus(TurnStatus.PRIORITY_3);
                            break;
                    }
                    ;
                    break;
                default:
                    c.BattleStatus.SetTurnStatus(TurnStatus.FIGHT);
                    break;
            }
        }
        else if (c.BattleStatus.SkipTurn)
            c.BattleStatus.SetTurnStatus(TurnStatus.SKIP);
        else
            c.BattleStatus.SetTurnStatus(TurnStatus.NOTHING);
    }

    private void AddBST(ref BST bst, Character c)
    {
        //Do not add to battle order binary search tree if they do not have a turn
        if (c.BattleStatus.TurnStatus.Equals(TurnStatus.NOTHING) || c.BattleStatus.TurnStatus.Equals(TurnStatus.SKIP))
            return;

        //If binary search tree does not have a node value, add the character and return
        if (bst == null || bst.character == null)
        {
            bst = new BST(c);
            return;
        }

        //If BST has a node value, determine if character should be before or after based off 
        //  turn status and speed
        if ((int)bst.character.BattleStatus.TurnStatus < (int)c.BattleStatus.TurnStatus)
            AddBST(ref bst.right, c);
        else if ((int)bst.character.BattleStatus.TurnStatus > (int)c.BattleStatus.TurnStatus)
            AddBST(ref bst.left, c);
        else
        {
            //determine who's faster
            if (bst.character.BaseStats.Spd >= c.BaseStats.Spd)
                AddBST(ref bst.right, c);
            else
                AddBST(ref bst.left, c);
        }
    }

}