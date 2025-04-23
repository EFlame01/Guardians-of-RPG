using System;
using System.Collections;
using UnityEngine;

public class AnimationState : CutSceneState
{
    [Serializable]
    public struct AnimInfo 
    {
        public GameObject Object;
        public Animator Animator;
        public string AnimationName;
        public float WaitTime;
        public float Duration;
        public Vector3 NewPosition;
        public float Speed;
    }

    [SerializeField] AnimInfo [] Animations;
    private int _animationsFinished;

    public override void Enter()
    {
        base.Enter();

        foreach(AnimInfo animation in Animations)
            StartCoroutine(StartAnimation(animation));
    }

    public override void Update()
    {
        base.Update();
        if(IsActive && _animationsFinished == Animations.Length)
            Exit();
            // ChangeState();
    }

    public IEnumerator StartAnimation(AnimInfo animation)
    {
        yield return new WaitForSeconds(animation.WaitTime);

        float currentTime = 0f;
        Vector2 startPos = animation.Object.transform.position;
        
        while(currentTime < animation.Duration)
        {
            //perform animation based on animation name
            if(animation.Animator != null)
                animation.Animator.Play(GetNewAnimation(animation.AnimationName));

            //transform object position based on new position
            if(animation.Speed == 0)
                animation.Object.transform.position = animation.NewPosition;
            else if(Vector3.Distance(animation.Object.transform.position, animation.NewPosition) > 0.005f)
                    animation.Object.transform.position = Vector2.Lerp(startPos, animation.NewPosition, currentTime/animation.Duration);

            currentTime += Time.fixedDeltaTime;
            yield return null;
        }

        animation.Object.transform.position = animation.NewPosition;

        _animationsFinished++;
        Debug.Log("animation " + _animationsFinished + " finished");
    }

    public void StartTransform(AnimInfo animation)
    {
        if(animation.Speed == 0)
                animation.Object.transform.position = animation.NewPosition;
        else
        {
            while(Vector3.Distance(animation.Object.transform.position, animation.NewPosition) > 0.005f)
                animation.Object.transform.position = Vector2.MoveTowards(animation.Object.transform.position, animation.NewPosition, Time.fixedDeltaTime * animation.Speed);
        }
    }

    public string GetNewAnimation(string animationName)
    {
        string player = (Player.Instance().Sex != null && (Player.Instance().Sex.Equals("MALE") || Player.Instance().Sex.Equals("MALEFE") && GameManager.Instance.Leaning.Equals("MALE"))) ? "adam" : "eve";
        if(animationName.Contains("player"))
            return animationName.Replace("player", player);
        return animationName;
    }
}
