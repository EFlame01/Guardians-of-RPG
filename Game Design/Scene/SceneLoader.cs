using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneLoader is a class that manages
/// how to load scenes in the game.
/// </summary>
public class SceneLoader : Singleton<SceneLoader>
{
    //Serialized variables
    [SerializeField] private Animator _transitionAnimator;
    [SerializeField] private Material _material;
    [SerializeField] private TransitionTexture[] _transTexturesList;
    [SerializeField] private Canvas _transitionTextureCanvas;
    [SerializeField] private CutScene _walkCutScene;
    [SerializeField] private PlayableDirector[] _playableDirectors;
    [SerializeField] private CutScene[] _cutScenes;

    //public static variables
    public static bool walkInAnimation;

    //private variables
    private Dictionary<TransitionType, Texture2D> _textureDictionary;
    private static bool _transitionOnStart;
    private static TransitionType _transitionType;

    protected override void Awake()
    {
        base.Awake();
        _textureDictionary = new Dictionary<TransitionType, Texture2D>();
        InitTextureDictionary();
    }

    private void Start()
    {
        if(_transitionOnStart)
            ShowScene();
    }

    /// <summary>
    /// Transfers the player to a new
    /// scene based on the name of the scene
    /// and it's transition type.
    /// </summary>
    /// <param name="nameOfScene">scene player will be transfered to</param>
    /// <param name="transType">the transition type</param>
    public void LoadScene(string nameOfScene, TransitionType transType)
    {
        GameManager.Instance.PlayerState = PlayerState.TRANSITION;
        GameManager.Instance.EnableKeyboardInputs = false;
        _transitionType = transType;
        Texture2D texture = _textureDictionary[_transitionType];

        switch(_transitionType)
        {
            case TransitionType.FADE_TO_BLACK:
                _transitionOnStart = true;
                StartCoroutine(LoadWithFade(nameOfScene, "fade_to_black"));
                break;
            case TransitionType.FADE_TO_WHITE:
                _transitionOnStart = true;
                StartCoroutine(LoadWithFade(nameOfScene, "fade_to_white"));
                break;
            case TransitionType.FOREST:
            case TransitionType.DESERT:
                _transitionOnStart = true;
                StartCoroutine(LoadWithTransition(nameOfScene, null, texture, false));
                break;
            case TransitionType.NONE:
                SceneManager.LoadScene(nameOfScene);
                break;
            default:
                _transitionOnStart = true;
                if(texture == null)
                    return;
                StartCoroutine(LoadWithTransition(nameOfScene, "battle_transition", texture, true));
                break;
        }
    }

    /// <summary>
    /// Displays scene based on transition type.
    /// </summary>
    public void ShowScene()
    {
        switch(_transitionType)
        {
            case TransitionType.FADE_TO_BLACK:
                StartCoroutine(LoadWithFade(null, "black_to_scene"));
                break;
            case TransitionType.FADE_TO_WHITE:
                StartCoroutine(LoadWithFade(null, "white_to_scene"));
                break;
            case TransitionType.NONE:
                break;
            default:
                StartCoroutine(ShowWithTransition());
                break;
        }

        //checks if you should activate the walkIn CutScene
        //  by checking the variable and if there are any active
        //  cut scenes.
        if(!walkInAnimation)
            return;

        WalkIn();
        GameManager.Instance.EnableKeyboardInputs = false;
    }

    /// <summary>
    /// Determines how the player will walk into a scene.
    /// This method is called only if the player should walk
    /// into a scene.
    /// </summary>
    private void WalkIn()
    {
        Debug.Log("Test 1: Made it to WalkIn()...");
        walkInAnimation = false;

        if(CutScenesActive())
            return;
                
        Debug.Log("Test 2: No cut scenes are active...");
        switch(PlayerSpawn.PlayerDirection)
        {
            case PlayerDirection.DOWN:
                _walkCutScene.director = _playableDirectors[Units.DOWN];
                _playableDirectors[Units.DOWN].gameObject.SetActive(true);
                _playableDirectors[Units.LEFT].gameObject.SetActive(false);
                _playableDirectors[Units.RIGHT].gameObject.SetActive(false);
                _playableDirectors[Units.UP].gameObject.SetActive(false);
                break;
            case PlayerDirection.LEFT:
                _walkCutScene.director = _playableDirectors[Units.LEFT];
                _playableDirectors[Units.LEFT].gameObject.SetActive(true);
                _playableDirectors[Units.DOWN].gameObject.SetActive(false);
                _playableDirectors[Units.RIGHT].gameObject.SetActive(false);
                _playableDirectors[Units.UP].gameObject.SetActive(false);
                break;
            case PlayerDirection.RIGHT:
                _walkCutScene.director = _playableDirectors[Units.RIGHT];
                _playableDirectors[Units.RIGHT].gameObject.SetActive(true);
                _playableDirectors[Units.DOWN].gameObject.SetActive(false);
                _playableDirectors[Units.LEFT].gameObject.SetActive(false);
                _playableDirectors[Units.UP].gameObject.SetActive(false);
                break;
            case PlayerDirection.UP:
                _walkCutScene.director = _playableDirectors[Units.UP];
                _playableDirectors[Units.UP].gameObject.SetActive(true);
                _playableDirectors[Units.DOWN].gameObject.SetActive(false);
                _playableDirectors[Units.LEFT].gameObject.SetActive(false);
                _playableDirectors[Units.RIGHT].gameObject.SetActive(false);
                break;
        }

        Debug.Log("Test 3: PlayableDirector used: " + (_walkCutScene.director != null));
        _walkCutScene.gameObject.SetActive(true);
        _walkCutScene.StartCutScene();
    }

    /// <summary>
    /// Checks if there are any active 
    /// <c>CutScene</c> playing. If there
    /// is, it will return TRUE. If not,
    /// it will return FALSE.
    /// </summary>
    /// <returns>TRUE if there is an active CutScene. FALSE otherwise.</returns>
    private bool CutScenesActive()
    {
        if(_cutScenes.Length <= 0)
            return false;

        foreach(CutScene cutScene in _cutScenes)
        {
            if(cutScene != null && cutScene.gameObject.activeSelf && cutScene.PlayOnStart)
            {
                Debug.Log("Active Cut Scene: " + cutScene.gameObject.name + " " + cutScene.gameObject.activeSelf);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Fades scene in and out based on name
    /// of animation.
    /// </summary>
    /// <param name="nameOfScene">scene that will be shown</param>
    /// <param name="nameOfAnimation">animation that will be played</param>
    /// <returns></returns>
    private IEnumerator LoadWithFade(string nameOfScene, string nameOfAnimation)
    {
        _transitionAnimator.Play(nameOfAnimation);
        
        yield return new WaitForSeconds(1f);
        
        if(nameOfScene != null)
            SceneManager.LoadScene(nameOfScene);
        else
        {
            if(GameManager.Instance.PlayerState != PlayerState.CUT_SCENE)
                GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
        }
    }

    /// <summary>
    /// Fills screen with texture to
    /// and loads new scene.
    /// </summary>
    /// <param name="nameOfScene">scene name to load</param>
    /// <param name="nameOfAnimation">animation name to animate</param>
    /// <param name="texture">texture that covers screen during scene load</param>
    /// <param name="battleTransition">boolean variable that determines if transition is a battle trasition</param>
    /// <returns></returns>
    private IEnumerator LoadWithTransition(string nameOfScene, string nameOfAnimation, Texture2D texture, bool battleTransition)
    {
        if(nameOfAnimation != null)
        {
            _transitionAnimator.Play(nameOfAnimation);
            yield return new WaitForSeconds(0.5f);
        }

        _transitionTextureCanvas.gameObject.SetActive(true);
        _material.SetTexture("_TransitionTex", texture);
        _material.SetFloat("_Cutoff", 0f);
        float cutOffValue = 0f;
        
        while(cutOffValue <= 1)
        {
            _material.SetFloat("_Cutoff", cutOffValue);
            cutOffValue += 0.05f;
            yield return new WaitForSeconds(0.04f);
        }
        _material.SetFloat("_Cutoff", 1f);
        _transitionType = battleTransition ? TransitionType.OPENING_BATTLE : _transitionType;
        _transitionOnStart = true;
        
        //load next scene
        SceneManager.LoadScene(nameOfScene);
    }

    /// <summary>
    /// Shows new scene based on transition type.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowWithTransition()
    {
        _material.SetTexture("_TransitionTex", _textureDictionary[_transitionType]);
        _material.SetFloat("_Cutoff", 1f);
        _transitionTextureCanvas.gameObject.SetActive(true);
        float cutOffValue = 1f;
        
        while(cutOffValue >= 0)
        {
            _material.SetFloat("_Cutoff", cutOffValue);
            cutOffValue -= 0.05f;
            yield return new WaitForSeconds(0.04f);
        }
        
        _material.SetFloat("_Cutoff", 0f);
        _transitionOnStart = false;
        _transitionTextureCanvas.gameObject.SetActive(false);
        
        if(GameManager.Instance.PlayerState != PlayerState.CUT_SCENE)
                GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
    }

    /// <summary>
    /// Adds textures to texture dictionary.
    /// </summary>
    private void InitTextureDictionary()
    {
        foreach(TransitionTexture transTexture in _transTexturesList)
            _textureDictionary[transTexture.Type] = transTexture.Texture;
    }
}