using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// MoveOption is a class that handles
/// the UI responsible for selecting the
/// <c>Move</c> the <c>Player</c> will use
/// in the next round.
/// </summary>
public class MoveOption : MonoBehaviour
{
    //serialized variables
    [SerializeField] private Transform MoveLayout;
    [SerializeField] private GameObject MoveButtonPrefab;
    [SerializeField] private TextMeshProUGUI MovePowerText;
    [SerializeField] private TextMeshProUGUI MoveAccuracyText;
    [SerializeField] private TextMeshProUGUI MoveTypeText;
    [SerializeField] private TextMeshProUGUI MoveDescriptionText;
    [SerializeField] private Button NextButton;

    //private variables
    private string _moveName;

    public void Start()
    {
        InitializeMoveOption();
    }

    public void Update()
    {
        NextButton.interactable = _moveName != null && Player.Instance().BattleStatus.ChosenMove != null;

        if (!NextButton.interactable)
        {
            _moveName = null;
            SetMoveDescription();
        }
    }

    /// <summary>
    /// Displays the information for the basic attack.
    /// </summary>
    public void OnBasicAttackButtonPressed()
    {
        Move move = Units.BASE_ATTACK;
        _moveName = move.Name;

        Player.Instance().BattleStatus.ChosenMove = move;
        MovePowerText.text = ((int)(move.Power * 100)).ToString();
        MoveAccuracyText.text = ((int)(move.Accuracy * 100)).ToString();
        MoveTypeText.text = move.Type.ToString();
        MoveDescriptionText.text = move.Description;
    }

    private void InitializeMoveOption()
    {
        MovePowerText.text = "000";
        MoveAccuracyText.text = "000";
        MoveTypeText.text = "";
        MoveDescriptionText.text = "";

        foreach (Move move in Player.Instance().BattleMoves)
        {
            if (move != null)
            {
                Button moveButton = Instantiate(MoveButtonPrefab, MoveLayout).GetComponent<Button>();
                TextMeshProUGUI moveButtonText = moveButton.GetComponentInChildren<TextMeshProUGUI>();
                moveButtonText.text = move.Name + "\n" + move.EP.ToString() + " EP";
                moveButton.interactable = Player.Instance().BaseStats.Elx < move.EP;
                moveButton.onClick.AddListener(() =>
                {
                    _moveName = move.Name;
                    SetMoveDescription();
                });
            }
        }
    }

    private void SetMoveDescription()
    {
        if (_moveName == null)
        {
            MovePowerText.text = "000";
            MoveAccuracyText.text = "000";
            MoveTypeText.text = "";
            MoveDescriptionText.text = "";
            return;
        }

        Move move = MoveManager.MoveDictionary[_moveName];

        Player.Instance().BattleStatus.ChosenMove = move;
        MovePowerText.text = ((int)(move.Power * 100)).ToString();
        MoveAccuracyText.text = ((int)(move.Accuracy * 100)).ToString();
        MoveTypeText.text = move.Type.ToString();
        MoveDescriptionText.text = move.Description;
    }
}
