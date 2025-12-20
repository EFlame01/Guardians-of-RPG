using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// MoveSetMenu is a class that extends the
/// <c>MenuState</c> class. MoveSetMenu
/// allows you to look at the <c>Player</c>'s entire
/// move-list divided into 2 parts: The Move List and
/// the Move Set, along with the ability 
/// to add and removes moves from the Move Set.
/// </summary>
public class MoveSetMenu : MenuState
{
    //Serialized Variables
    [SerializeField] private TextMeshProUGUI moveNameText;
    [SerializeField] private TextMeshProUGUI moveTypeText;
    [SerializeField] private TextMeshProUGUI movePowerText;
    [SerializeField] private TextMeshProUGUI moveAccText;
    [SerializeField] private TextMeshProUGUI moveEPText;
    [SerializeField] private TextMeshProUGUI moveDescText;
    [SerializeField] private Button battleMoveButtonPrefab;
    [SerializeField] private Button moveLearnedButtonPrefab;
    [SerializeField] private Transform battleMovesLayout;
    [SerializeField] private Transform movesLearnedLayout;
    [SerializeField] private Button removeButton;
    [SerializeField] private Button addButton;

    //private variables
    private Move chosenBattleMove;
    private Move chosenMoveLearned;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetUpMoveSets();
    }

    // Update is called once per frame
    void Update()
    {
        addButton.interactable = chosenMoveLearned != null;
        removeButton.interactable = chosenBattleMove != null;
    }

    /// <summary>
    /// Determines if a move can be removed from
    /// the Battle Moves slot. After displaying
    /// the results, it will then remove the move
    /// from the Battle Moves slot if it is not the 
    /// sole move in there.
    /// </summary>
    public void OnRemoveButtonPressed()
    {
        Player player = Player.Instance();
        string results = CanRemoveMove();

        if (player.MoveManager.TotalBattleMoves() > 1)
            player.MoveManager.RemoveBattleMove(chosenBattleMove.Name);

        SetUpMoveSets();
        moveDescText.text = results;
    }

    /// <summary>
    /// Determines if a move can be added to the
    /// Battle Moves slot. After displaying the
    /// results, it will then add the move to the
    /// Battle Moves slot if there is room in the 
    /// Battle Moves slot for it.
    /// </summary>
    public void OnAddButtonPressed()
    {
        Player player = Player.Instance();
        string results = CanAddMove();

        if (player.MoveManager.TotalBattleMoves() < 4)
            player.MoveManager.AddToBattleMoves(chosenMoveLearned.Name);

        SetUpMoveSets();
        moveDescText.text = results;
    }

    private void SetUpMoveSets()
    {
        Player player = Player.Instance();
        chosenBattleMove = null;
        chosenMoveLearned = null;
        moveNameText.text = "";
        moveTypeText.text = "";
        movePowerText.text = "0.00";
        moveAccText.text = "0.00";
        moveEPText.text = "0.00";
        moveDescText.text = "";

        ClearMoves();

        foreach (Move battleMove in player.BattleMoves)
        {
            if (battleMove != null)
            {
                Button battleMovesButton = Instantiate(battleMoveButtonPrefab, battleMovesLayout);
                TextMeshProUGUI battleMoveName = battleMovesButton.GetComponentInChildren<TextMeshProUGUI>();
                battleMoveName.text = battleMove.Name;
                battleMovesButton.onClick.AddListener(() =>
                {
                    chosenBattleMove = battleMove;
                    chosenMoveLearned = null;
                    DisplayMoveInformation();
                });
            }
        }

        foreach (KeyValuePair<string, Move> moveInfo in MoveManager.MoveDictionary)
        {
            Button moveLearnedButton = Instantiate(moveLearnedButtonPrefab, movesLearnedLayout);
            TextMeshProUGUI moveLearnedName = moveLearnedButton.GetComponentInChildren<TextMeshProUGUI>();
            moveLearnedName.text = moveInfo.Key;
            moveLearnedButton.onClick.AddListener(() =>
            {
                chosenMoveLearned = moveInfo.Value;
                chosenBattleMove = null;
                DisplayMoveInformation();
            });
        }
    }

    private void DisplayMoveInformation()
    {
        Move chosenMove = chosenBattleMove ?? chosenMoveLearned;

        if (chosenMove == null)
            return;

        Debug.Log(chosenMove.Name + " selected");

        moveNameText.text = chosenMove.Name;
        moveTypeText.text = chosenMove.Type.ToString();
        movePowerText.text = chosenMove.Power.ToString();
        moveAccText.text = chosenMove.Accuracy.ToString();
        moveEPText.text = chosenMove.EP.ToString();
        moveDescText.text = chosenMove.Description;
    }

    private void ClearMoves()
    {
        foreach (Transform child in battleMovesLayout)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in movesLearnedLayout)
        {
            Destroy(child.gameObject);
        }
    }

    private string CanRemoveMove()
    {
        if (Player.Instance().MoveManager.TotalBattleMoves() > 1)
            return chosenBattleMove.Name + " was removed from the Battle Moves slot!";
        return "You need at least one move in the Battle Moves slot...";
    }

    private string CanAddMove()
    {
        if (Player.Instance().MoveManager.TotalBattleMoves() < 4)
            return chosenMoveLearned.Name + " was added to the Battle Moves slot!";
        return "You have no room in your Battle Moves slot. Try removing a move in the list first.";
    }
}
