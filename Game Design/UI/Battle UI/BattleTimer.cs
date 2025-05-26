using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// BattleTimer is a class that sets a 
/// timer to inform the player how much
/// time is left in the round to make a
/// move. Once the timer is up, it will
/// automatically skip their turn, and 
/// begin the next round.
/// </summary>
public class BattleTimer : MonoBehaviour
{
    //public variables
    public TextMeshProUGUI TimerText;
    public Animator animator;
    public double TimeLeft {get; private set;}
    public Options options;
    public BattleOptions battleOptions;

    //private variables
    private int _startTime;
    private bool _timeToStart;

    public void Update()
    {
        if(EndTime() && _timeToStart)
        {
            StopTimer(); //this sets _timeToStart to falses
            Player.Instance().BattleStatus.SetTurnStatus(TurnStatus.SKIP);
            if(options.gameObject.activeSelf)
                StartCoroutine(options.CloseOptions());
            if(battleOptions.gameObject.activeSelf)
                StartCoroutine(battleOptions.CloseBattleOptions());
            BattleSimStatus.EndPlayerOption = true;
        }
        
        if(_timeToStart)
        {
            TimeLeft -= Time.fixedDeltaTime;
            int tempTime = (int) Mathf.Ceil((float)TimeLeft);
            TimerText.text = tempTime.ToString();
        }
    }

    /// <summary>
    /// Begins the timer and displays
    /// the timer UI.
    /// </summary>
    public void StartTimer()
    {
        _startTime = 60;
        _timeToStart = true;
        TimeLeft = _startTime;
        StartCoroutine(PlayTimerAnimation("slide_down_to_ui"));
    }

    /// <summary>
    /// Stops the timer and removes the 
    /// timer UI.
    /// </summary>
    public void StopTimer()
    {
        _timeToStart = false;
        StartCoroutine(PlayTimerAnimation("ui_to_fade"));
    }

    private bool EndTime()
    {
        return TimeLeft <= 0f;
    }

    private IEnumerator PlayTimerAnimation(string name)
    {
        animator.Play(name);
        yield return new WaitForSeconds(0.5f);
    }
}