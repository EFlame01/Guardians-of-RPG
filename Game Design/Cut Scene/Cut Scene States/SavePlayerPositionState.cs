using UnityEngine;

public class SavePlayerPositionState : CutSceneState
{
    [SerializeField] public string sceneName;
    [SerializeField] public string mapLocationName;
    [SerializeField] public Vector3 playerPosition;

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.SavePlayerLocationData();
        Exit();
    }
}