using System.Collections;
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
    //Serialized variables
    [Header("FightObject Properties")]
    public PlayerDirection NPCDirection;
    public PlayerDirection PlayerViewDirection;
    public Animator exclamationEmote;
    public PlayerSprite NPCSprite;
    public DialogueData DialogueDataAfterBattle;

    [Header("FightObject Properties")]
    public string Environment;
    public BattleCharacterData BattlePlayerData;
    public BattleCharacterData[] BattleAlliesData;
    public BattleCharacterData[] BattleEnemiesData;

    [Header("Transition")]
    public TransitionType TransitionType;

    [Header("Story Flags")]
    public bool startedBattle;
    public string[] StoryFlagsIfWon;

    [Header("Music")]
    public string TrackName;

    [Header("Lose Battle Information")]
    public bool GameOverScreen;
    public string SceneName;
    public string LoseMessage;
    public Vector3 Position = new Vector3(0, 0, 0);

    //private variables
    private bool _bumpIntoPlayer;
    private bool _confrontedPlayer;

    /// <summary>
    /// If Player can interact with
    /// object, it will check if the Player
    /// has fought the object before. If it
    /// has, it will talk to Player.
    /// Otherwise, it will fight Player.
    /// </summary>
    public override void InteractWithObject()
    {
        if (CanInteract)
        {
            CanInteract = false;
            if (NpcData.foughtPlayer)
                ConfrontPlayer3();
            else
                StartCoroutine(ConfrontPlayer1());
        }
    }

    /// <summary>
    /// Confronts the <c>Player</c> only if they
    /// have chose to interact with the object.
    /// The object will turn to player, talk to
    /// them, and initiate combat.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ConfrontPlayer1()
    {
        if (!_confrontedPlayer)
        {
            _confrontedPlayer = true;
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            TurnToPlayer();
            yield return TalkToPlayer();
            StartFight();
        }
    }

    /// <summary>
    /// Confronts the <c>Player</c> when the 
    /// object has spotted the <c>Player</c>.
    /// The object will walk to the player,
    /// talk to them, and initiate combat.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ConfrontPlayer2()
    {
        if (!NpcData.foughtPlayer)
        {
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

    /// <summary>
    /// Talks to the <c>Player</c>.
    /// This method is typically called
    /// after the object has finished fighting
    /// and is confronted by the player a 
    /// second time.
    /// </summary>
    /// <returns></returns>
    private void ConfrontPlayer3()
    {
        if (!TalkedToPlayer)
        {
            TalkedToPlayer = true;
            _dialogueData = DialogueDataAfterBattle;
            GameManager.Instance.PlayerState = PlayerState.INTERACTING_WITH_OBJECT;
            StartCoroutine(TalkToPlayer());
        }
    }

    /// <summary>
    /// Uses the exclamationEmote to 
    /// play the surprised emote for 1
    /// second.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformExclamation()
    {
        AudioManager.Instance.PlaySoundEffect(Units.SoundEffect.EMOTE);
        exclamationEmote.Play("surprised");
        yield return new WaitForSeconds(1f);
        exclamationEmote.Play("no_emote");
    }

    /// <summary>
    /// Determines the player's direction
    /// and walks to them. Once the object
    /// has bumped into the player, it stops.
    /// </summary>
    /// <returns></returns>
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
        while (!_bumpIntoPlayer)
        {
            //move to player position
            transform.Translate(direction.normalized * 3 * Time.fixedDeltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// This method sets up everything needed
    /// for battle and starts the fight.
    /// </summary>
    private void StartFight()
    {
        startedBattle = true;
        NpcData.foughtPlayer = true;
        NpcData.Position = transform.position;
        if (PlayerViewDirection.Equals(PlayerDirection.NONE))
            NpcData.direction = GetCollisionSide().ToString();
        else
            NpcData.direction = PlayerViewDirection.ToString();
        SetUpBattleMusic();
        SetUpForBattle();
    }

    /// <summary>
    /// Sets up the battle music.
    /// </summary>
    private void SetUpBattleMusic()
    {
        AudioManager.Instance.PlayMusic(TrackName, true);
    }

    /// <summary>
    /// Sets up the environment, character data,
    /// and story flags for battle.
    /// </summary>
    private void SetUpForBattle()
    {
        BattleInformation.Environment = Environment;
        BattleInformation.BattlePlayerData = BattlePlayerData;
        BattleInformation.PlayerPosition = PlayerSpawn.PlayerPosition;
        BattleInformation.StoryFlagsIfWon = StoryFlagsIfWon;

        BattleSimStatus.GameOverScreenIfLost = GameOverScreen;

        if (GameOverScreen)
            GameOverScene.SetScene(LoseMessage, SceneName, Position);

        for (int i = 0; i < BattleAlliesData.Length; i++)
        {
            if (BattleAlliesData[i] != null)
                BattleInformation.BattleAlliesData[i] = BattleAlliesData[i];
        }

        for (int i = 0; i < BattleEnemiesData.Length; i++)
        {
            if (BattleEnemiesData[i] != null)
                BattleInformation.BattleEnemiesData[i] = BattleEnemiesData[i];
        }

        BattleSimStatus.CanRun = false;
        BattleSimStatus.SceneName = SceneManager.GetActiveScene().name;
        SceneLoader.Instance.LoadScene("Battle Scene", TransitionType);
    }

    public override void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (GameManager.Instance.PlayerState.Equals(PlayerState.INTERACTING_WITH_OBJECT))
        {
            if (collision2D.gameObject.CompareTag("Player"))
                _bumpIntoPlayer = true;
            else
                _bumpIntoPlayer = false;
        }
        else
            base.OnCollisionEnter2D(collision2D);
    }
}
