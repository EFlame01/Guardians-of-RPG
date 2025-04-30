using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System.Linq;
using System;
using UnityEditorInternal;
using TMPro.Examples;

/// <summary>
/// CutScene is a class that is used to create and start all 
/// cut scenes within the game.
/// </summary>
public class CutScene : MonoBehaviour
{
    [SerializeField] public float StartTime;
    [SerializeField] public CutSceneState CurrentState;
    [SerializeField] public PlayableDirector director;
    [SerializeField] public Animator[] animators;
    [SerializeField] public PlayerState TransitionOrCutScene;
    [SerializeField] public bool PlayOnStart;
    [SerializeField] public bool ResumeOnEnd;
    [SerializeField] public PlayerDirection EndDirection;
    [SerializeField] public ActivateObject ActivateObject;
    [SerializeField] private bool _refreshCutScene;

    private int _trackIndex;
    private RuntimeAnimatorController[] _tempControllers;
    private bool _cutSceneStarted;
    private CutSceneState _head;

    // Start is called before the first frame update
    public void Start()
    {
        //If we have an active object component
        //  check if game object should be destoryed
        //  before starting the cut scene
        if(ActivateObject != null)
        {
            bool activate = ActivateObject.DetermineActivateCriteria();
            bool deactivate = ActivateObject.DetermineDeactivateCriteria();
            if(!activate || deactivate)
            {
                // Debug.Log("Cut Scene for gameObject " + gameObject.name + " did not meet the activate criteria, or it met the deactivate criteria: " + activate + " " + deactivate);
                gameObject.SetActive(false);
                return;
            }
        }

        //If this cut scene should be played
        //  immediately, then start the cut scene
        if(PlayOnStart)
            StartCutScene();
    }

    // Update is called once per frame
    public void Update()
    {
        if(!_cutSceneStarted)
            return;

        if(director.time >= director.duration - 0.10)
            EndCutScene();

        if(CurrentState == null)
            return;
        
        switch(director.state)
        {
            case PlayState.Playing:
                if(director.time >= CurrentState.TimeStamp)
                {
                    director.Pause();
                    CurrentState.Enter();
                }
                break;
            case PlayState.Paused:
                if(CurrentState.IsDone)
                {
                    CurrentState = CurrentState.NextState;
                    director.Resume();
                }
                break;
        }
    }

    /// <summary>
    /// Initializes a Cut Scene by adjusting
    /// the player sprites to fit the actual
    /// avatar of the player.
    /// </summary>
    public void Init()
    {

        if(Player.Instance().MaleOrFemale().Equals("MALE"))
            Mute(true);
        else
            Mute(false);
    }

    /// <summary>
    /// Disable the animator controllers of the 
    /// gameobjects involved in Cut Scene and starts
    /// the PlayableDirector.
    ///</summary>
    public void StartCutScene()
    {
        _tempControllers = new RuntimeAnimatorController[animators.Length];
        
        for(int i = 0; i < animators.Length; i++)
        {
            if(animators[i] != null)
            {
                _tempControllers[i] = animators[i].runtimeAnimatorController;
                animators[i].runtimeAnimatorController = null;
            }
        }

        Init();
        GameManager.Instance.PlayerState = TransitionOrCutScene;
        director.time = StartTime;
        
        if(CurrentState != null)
            _head = CurrentState;

        if(director.time > 0)
            director.Resume();
        else
            director.Play();
            
        _cutSceneStarted = true;
    }

    /// <summary>
    /// Stops the PlayableDirector and 
    /// enables the animator controllers of the
    /// gameobjects involved in Cut Scene.
    /// </summary>
    private void EndCutScene()
    {
        if(!EndDirection.Equals(PlayerDirection.NONE))
            PlayerSpawn.PlayerDirection = EndDirection;

        director.Stop();
        ResetCamera();
        if(ResumeOnEnd)
            GameManager.Instance.PlayerState = PlayerState.NOT_MOVING;
        
        for(int i = 0; i < animators.Length; i++)
        {
            if(animators[i] != null && _tempControllers[i] != null)
                animators[i].runtimeAnimatorController = _tempControllers[i];
        }

        _tempControllers = null;
        
        if(_refreshCutScene)
        {
            RefreshCutScene(_head);
            CurrentState = _head;
        }
    }

    /// <summary>
    /// Finds the track index 'female player track'
    /// </summary>
    private void FindTrack(string trackName)
    {
        // var director = GetComponent<PlayableDirector>();
        if (director != null && director.playableAsset != null)// && director.playableAsset != null
        {
            _trackIndex = -1;
            var bindings = director.playableAsset.outputs.ToArray();
            for (int i = 0; i < bindings.Length; i++)
            {
                if (bindings[i].streamName.Equals(trackName))
                {
                    _trackIndex = i;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Finds and mutes the 'female player track'.
    /// </summary>
    private void Mute(bool mute)
    {
        FindTrack("female player track");
        if (_trackIndex == -1)
            return;

        TimelineAsset asset = director.playableAsset as TimelineAsset;
        asset.GetOutputTrack(_trackIndex).muted = mute;
        director.RebuildGraph();
    }

    /// <summary>
    /// Resets the camera position to the default
    /// position.
    /// </summary>
    private void ResetCamera()
    {
        FindTrack("camera animation track");
        Debug.Log("camera animation track found: " + (_trackIndex != -1));
        if(_trackIndex == -1)
            return;
        
        TimelineAsset asset = director.playableAsset as TimelineAsset;
        AnimationTrack cameraAnimationTrack = (AnimationTrack) asset.GetOutputTrack(_trackIndex);
        cameraAnimationTrack.position = new Vector3(0,0,0);
        Debug.Log(cameraAnimationTrack.position);
        asset.GetOutputTrack(_trackIndex) = cameraAnimationTrack;
    }

    /// <summary>
    /// Resets all of the CutSceneStates in a 
    /// cutscene in order to replay the cutscene.
    /// </summary>
    /// <param name="cutSceneState"></param>
    private void RefreshCutScene(CutSceneState cutSceneState)
    {
        //if cutscene state is null
        if(cutSceneState == null)
            return;
        //if cutscene state is decision state
        else if (cutSceneState is DecisionState decisionState)
        {
            foreach (CutSceneState css in decisionState.StateOptions)
                RefreshCutScene(css);
            decisionState.NextState = null;
            decisionState.IsDone = false;
        }
        //all other edge cases
        else
        {
            cutSceneState.IsDone = false;
            RefreshCutScene(cutSceneState.NextState);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag.Equals("Player") && !_cutSceneStarted)
        {
            StartCutScene();
            _cutSceneStarted = true;
        }
    }

}