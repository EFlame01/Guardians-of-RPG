using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// FightObject is a class that extends the 
/// <c>InteractableObject</c> class. FightObject
/// Will either have the ability to detect the <c>Player</c>
/// walking along it's field of vision, or once
/// the player interacts with it, it will begin
/// to initiate a battle with the player
/// </summary>
public class FightObject : NPCObject
{
    [Header("FightObject Properties")]
    [SerializeField] public PlayerDirection NPCDirection;
    [SerializeField] public PlayerDirection PlayerViewDirection;
    [SerializeField] public Animator exclamationEmote;
    [SerializeField] public PlayerSprite NPCSprite;
    [SerializeField] public DialogueData DialogueDataAfterBattle;
    [Header("FightObject Properties")]
    [SerializeField] public string Environment;
    [SerializeField] public BattleCharacterData BattlePlayerData;
    [SerializeField] public BattleCharacterData[] BattleAlliesData;
    [SerializeField] public BattleCharacterData[] BattleEnemiesData;
    [Header("Transition")]
    [SerializeField] public TransitionType TransitionType;
    [Header("Story Flags")]
    [SerializeField] public bool startedBattle;
    [SerializeField] public string[] StoryFlagsIfWon;
    [Header("Music")]
    [SerializeField] public string TrackName;

    private bool _bumpIntoPlayer;
    private bool _confrontedPlayer;

    public override void InteractWithObject()
    {
        if (CanInteract)
        {
            CanInteract = false;
            if(_npcData.foughtPlayer)
                ConfrontPlayer3();
            else
                StartCoroutine(ConfrontPlayer1());
        }
    }

    private IEnumerator ConfrontPlayer1()
    {
        if(!_confrontedPlayer)
        {
            Debug.Log("ConfrontPlayer1()...");
            _confrontedPlayer = true;
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            TurnToPlayer();
            yield return TalkToPlayer();
            StartFight();
        }
    }

    public IEnumerator ConfrontPlayer2()
    {
        if(!_npcData.foughtPlayer)
        {
            Debug.Log("ConfrontPlayer2()...");
            _confrontedPlayer = true;
            CanInteract = false;
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            yield return PerformExclamation();
            yield return WalkToPlayer();
            NPCSprite.PerformIdleAnimation(PlayerViewDirection);
            yield return new WaitForSeconds(0.5f);
            yield return TalkToPlayer();
            StartFight();
        }
    }

    private void ConfrontPlayer3()
    {
        if(!_talkedToPlayer)
        {
            Debug.Log("ConfrontPlayer3()...");
            _talkedToPlayer = true;
            _dialogueData = DialogueDataAfterBattle;
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            StartCoroutine(TalkToPlayer());
        }
    }

    private IEnumerator PerformExclamation()
    {
        exclamationEmote.Play("surprised");
        yield return new WaitForSeconds(1f);
        exclamationEmote.Play("no_emote");
    }

    private IEnumerator WalkToPlayer()
    {
        Vector2 direction = Vector2.zero;
        switch (PlayerViewDirection)
        {
            case PlayerDirection.UP:
                //walk up
                NPCSprite.PerformWalkAnimation("walk_up");
                direction = Vector2.up;
                break;
            case PlayerDirection.LEFT:
                NPCSprite.PerformWalkAnimation("walk_left");
                direction = Vector2.left;
                break;
            case PlayerDirection.DOWN:
                NPCSprite.PerformWalkAnimation("walk_down");
                direction = Vector2.down;
                break;
            case PlayerDirection.RIGHT:
                NPCSprite.PerformWalkAnimation("walk_right");
                direction = Vector2.right;
                break;
            default:
                break;
        }
        while(!_bumpIntoPlayer)
        {
            //move to player position
            transform.Translate(direction.normalized * 3 * Time.fixedDeltaTime);
            yield return null;
        }
    }

    private void StartFight()
    {
        startedBattle = true;
        _npcData.foughtPlayer = true;
        _npcData.Position = transform.position;
        if(PlayerViewDirection.Equals(PlayerDirection.NONE))
            _npcData.direction = GetCollisionSide().ToString();
        else
            _npcData.direction = PlayerViewDirection.ToString();
        SetUpBattleMusic();
        SetUpForBattle();
    }

    private void SetUpBattleMusic()
    {
        AudioManager.Instance.PlayMusic(TrackName, true);
    }

    private void SetUpForBattle()
    {
        BattleInformation.Environment = Environment;
        BattleInformation.BattlePlayerData = BattlePlayerData;
        BattleInformation.PlayerPosition = PlayerSpawn.PlayerPosition;
        BattleInformation.StoryFlagsIfWon = StoryFlagsIfWon;
        
        Debug.Log(PlayerSpawn.PlayerPosition);

        for(int i = 0; i < BattleAlliesData.Length; i++)
        {
            if(BattleAlliesData[i] != null)
                BattleInformation.BattleAlliesData[i] = BattleAlliesData[i];
        }

        for(int i = 0; i < BattleEnemiesData.Length; i++)
        {
            if(BattleEnemiesData[i] != null)
                BattleInformation.BattleEnemiesData[i] = BattleEnemiesData[i];
        }

        BattleSimStatus.SceneName = SceneManager.GetActiveScene().name;
        SceneLoader.Instance.LoadScene("Battle Scene", TransitionType);
    }

    public override void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(GameManager.Instance.PlayerState.Equals(PlayerState.INTERACTING_WITH_OBJECT))
        {
            if(collision2D.gameObject.tag.Equals("Player"))
                _bumpIntoPlayer = true;
            else
                _bumpIntoPlayer = false;
        }
        else
            base.OnCollisionEnter2D(collision2D);
    }
}
