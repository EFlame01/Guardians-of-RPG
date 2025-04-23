using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoveSetMenu : MenuState
{
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

    public void OnRemoveButtonPressed()
    {
        Player player = Player.Instance();
        if(player.MoveManager.TotalBattleMoves() == 1)
        {
            //TODO: narrate player needs at least one battle move
            Debug.Log("Player needs to have at least one move in battle slot");
        }
        else
        {
            player.MoveManager.RemoveBattleMove(chosenBattleMove.Name);
            SetUpMoveSets();
        }
    }

    public void OnAddButtonPressed()
    {
        Player player = Player.Instance();
        if(player.MoveManager.TotalBattleMoves() == 4)
        {
            //TODO: narrate battle move slot is full.
            Debug.Log("Battle Slot is full");
        }
        else
        {
            player.MoveManager.AddToBattleMoves(chosenMoveLearned.Name);
            SetUpMoveSets();
        }
    }

    private void SetUpMoveSets()
    {
        Player player = Player.Instance();
        chosenBattleMove = null;
        chosenMoveLearned = null;
        moveNameText.text  = "";
        moveTypeText.text = "";
        movePowerText.text = "0.00";
        moveAccText.text = "0.00";
        moveEPText.text = "0.00";
        moveDescText.text = "";

        ClearMoves();

        foreach(Move battleMove in player.BattleMoves)
        {
            if(battleMove != null)
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

        foreach(KeyValuePair<string, Move> moveInfo in MoveManager.MoveDictionary)
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
        Move chosenMove = chosenBattleMove != null ? chosenBattleMove : chosenMoveLearned;
        
        if(chosenMove == null)
            return;
        
        moveNameText.text  = chosenMove.Name;
        moveTypeText.text = chosenMove.Type.ToString();
        movePowerText.text = chosenMove.Power.ToString();
        moveAccText.text = chosenMove.Accuracy.ToString();
        moveEPText.text = chosenMove.EP.ToString();
        moveDescText.text = chosenMove.Description;
    }

    public void ClearMoves()
    {
        foreach(Transform child in battleMovesLayout)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in movesLearnedLayout)
        {
            Destroy(child.gameObject);
        }
    }
}
