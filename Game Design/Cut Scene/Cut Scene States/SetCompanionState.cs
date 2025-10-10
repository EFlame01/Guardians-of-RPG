using UnityEngine;

/// <summary>
/// SetCompanionState is a class that extends the 
/// CutSceneState class. SetCompanionState helps 
/// set up the companions that the player has
/// in the event that the player and their companions
/// face a wild encounter.
/// </summary>
public class SetCompanionState : CutSceneState
{
    //Serialized variables
    [SerializeField] private string[] companionsToRemove;
    [SerializeField] private BattleCharacterData[] companionsToAdd;

    public override void Enter()
    {
        base.Enter();
        PlayerCompanions.Instance.ClearCompanionList();
        PlayerCompanions.Instance.RemoveCompanions(companionsToRemove);
        PlayerCompanions.Instance.AddCompanions(companionsToAdd);
        Exit();
    }
}