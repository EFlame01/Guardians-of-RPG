using System.Collections;
using UnityEngine;

/// <summary>
/// NPCCompanion is a class that allows the NPC
/// to travel with the player.
/// </summary>
public class NPCCompanion : MonoBehaviour
{
    [SerializeField] public PlayerSprite npcSprite;
    [SerializeField] public CharacterPos targetPos;
    [SerializeField] public CharacterPos charPos;
    [SerializeField] public int zOffset;
    [SerializeField] public string npcID;
    [SerializeField] public string[] flags;
    [SerializeField] public bool[] flagValues;
    [SerializeField] public string cutSceneFlag;
    private float _speed;
    private int lastIndex = 0;
    private NpcData _npcData;

    /// <summary>
    /// When enabled, NPCCompanion will check the NPC data
    /// to ensure that the character is in the last position
    /// it was saved in, and whether or not the gameObject should
    /// be active based on the Save data.
    /// </summary>
    public void OnEnable()
    {
        _npcData = NpcDataContainer.GetNpcData(npcID) ?? new NpcData(npcID, transform.position, flags, flagValues);
        
        if(_npcData == null)
            return;
        
        transform.position = _npcData.Position;
        
        if(flags.Length <= 0 )
            return;

        for(int i = 0; i < flags.Length; i++)
            if(!StoryFlagManager.FlagDictionary[flags[i]].Value == flagValues[i])
                Destroy(gameObject);
    }

    /// <summary>
    /// When stated, NPCCompanion will set the NPC's speed
    /// to the player's speed, and sets the start position
    /// of the NPC Companion.
    /// </summary>
    private void Start()
    {
        _speed = GameManager.Instance.PlayerSpeed;
        InitPosition();
    }

    /// <summary>
    /// At every frame, NPCCompanion will check
    /// if the WayPoints should be reset based on
    /// the method <c>CheckToClearWayPoints</c>
    /// </summary>
    private void Update()
    {
        CheckToClearWayPoints();
    }

    /// <summary>
    /// During the fixed update, NPCCompanion will 
    /// check the WayPoints and travel to them at 
    /// every frame the player moves.
    /// </summary>
    private void FixedUpdate()
    {
        if(CanMove())
        {
            Vector3 startPosition = transform.position;
            WayPoint wayPoint = targetPos.WayPoints[lastIndex++];

            charPos.AddWayPoint(transform.position, charPos.Direction);
            charPos.Direction = wayPoint.Direction;
            
            switch(wayPoint.Direction)
            {
                case PlayerDirection.DOWN:
                    npcSprite.PerformWalkAnimation("walk_down");
                    startPosition = new Vector3(transform.position.x, transform.position.y, zOffset);
                    break;
                case PlayerDirection.UP:
                    npcSprite.PerformWalkAnimation("walk_up");
                    startPosition = new Vector3(transform.position.x, transform.position.y, -zOffset);
                    break;
                case PlayerDirection.LEFT:
                    npcSprite.PerformWalkAnimation("walk_left");
                    startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    break;
                case PlayerDirection.RIGHT:
                    npcSprite.PerformWalkAnimation("walk_right");
                    startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                    break;
                default:
                    npcSprite.PerformIdleAnimation(wayPoint.Direction);
                    break;
            }

            transform.position = Vector3.Lerp(startPosition, wayPoint.Position, Time.fixedDeltaTime * _speed);

        }
        else
            npcSprite.PerformIdleAnimation(charPos.Direction);
    }

    /// <summary>
    /// Initilizes the NPC to be in the correct
    /// position based on the target's location.
    /// </summary>
    private void InitPosition()
    {
        //CHECK IF CHARACTER SHOULD MOVE
        if(CheckToClearWayPoints())
            return;
        
        Vector3 position = targetPos.Position;
        PlayerDirection direction = targetPos.Direction;

        switch(direction)
        {
            case PlayerDirection.UP:
                position += Vector3.down;
                position.z = -zOffset;
                break;
            case PlayerDirection.DOWN:
                position += Vector3.up;
                position.z = zOffset;
                break;
            case PlayerDirection.LEFT:
                position += Vector3.right;
                position.z = 0;
                break;
            case PlayerDirection.RIGHT:
                position += Vector3.left;
                position.z = 0;
                break;
            default:
                break;
        }

        transform.position = position;
        npcSprite.PerformIdleAnimation(direction);
    }

    /// <summary>
    /// Determines if the NPC can 
    /// move if the target has way points
    /// the NPC can travel to.
    /// </summary>
    /// <returns>True if the NPC can travel, false otherwise.</returns>
    private bool CanMove()
    {
        return !InCutScene() && targetPos.WayPoints.Count > lastIndex;
    }

    /// <summary>
    /// CheckToClearWayPoints determines if the 
    /// WayPoints should be cleared based on the 
    /// method <c>InCutScene</c>. If it is the case, 
    /// it will clear the WayPoints for the character
    /// and the target and return TRUE. otherwise, 
    /// it will return FALSE.
    /// </summary>
    /// <returns><c>TRUE</c> if InCutScene() is true, <c>FALSE</c> if otherwise</returns>
    private bool CheckToClearWayPoints()
    {
        if(InCutScene())
        {
            charPos.ClearWayPoints();
            targetPos.ClearWayPoints();
            return true;
        }
        return false;
    }

    /// <summary>
    /// InCutScene() checks if the NPCCompanion is attached to a cut
    /// scene or is currently in a cut scene. If it meets one of these
    /// criteria, it will return TRUE. If it does not, it will return 
    /// FALSE.
    /// </summary>
    /// <returns><c>TRUE</c> if in or attached to a cutscene. <c>FALSE</c> if otherwise.</returns>
    private bool InCutScene()
    {
        if(cutSceneFlag == null || cutSceneFlag.Length == 0)
            return false;
        if(StoryFlagManager.FlagDictionary[cutSceneFlag] == null)
            return false;
        if(StoryFlagManager.FlagDictionary[cutSceneFlag].Value == false)
            return true;
            
        switch(GameManager.Instance.PlayerState)
        {
            case PlayerState.MOVING:
            case PlayerState.NOT_MOVING:
                return false;
            case PlayerState.PAUSED:
            case PlayerState.CUT_SCENE:
            case PlayerState.TRANSITION:
            case PlayerState.INTERACTING_WITH_OBJECT:
            case PlayerState.CANNOT_MOVE:
                return true;
            default:
                return true;

        }
    }
}