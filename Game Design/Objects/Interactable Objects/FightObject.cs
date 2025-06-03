using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Transform _leftView;
    [SerializeField] private Transform _rightView;
    [SerializeField] private Transform _topView;
    [SerializeField] private Transform _bottomView;
    [SerializeField] private WayPoint[] _wayPoints;
    [SerializeField] private float _speed;
    [SerializeField] private float _waitTime;
    [SerializeField] private DialogueData _dialogueData2;
    [SerializeField] private bool _npcLost;

    public override void InteractWithObject()
    {
        if (CanInteract)
            StartCoroutine(ConfrontPlayer1());
    }

    public IEnumerator ConfrontPlayer1()
    {
        yield return PlayEmote();
        yield return Engage();

        while (!DialogueManager.Instance.DialogueEnded)
            yield return null;

        yield return StartFight();
    }

    public IEnumerator ConfrontPlayer2()
    {
        yield return PlayEmote();
        yield return WalkTowardsPlayer();
        yield return Engage();

        while (!DialogueManager.Instance.DialogueEnded)
            yield return null;

        yield return StartFight();
    }

    public IEnumerator PlayEmote()
    {
        yield return null;
    }

    public IEnumerator WalkTowardsPlayer()
    {
        yield return null;
    }

    public IEnumerator Engage()
    {
        yield return null;
    }

    public IEnumerator StartFight()
    {
        yield return null;
    }
}
