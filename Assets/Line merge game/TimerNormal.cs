using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerNormal : MonoBehaviour, ITimer
{
    [SerializeField] float maxGameTime = 30;
    [SerializeField] float currentGameTime;

    public void InitTimer()
    {
        currentGameTime = maxGameTime;
        UIManager.instance.UpdateTimerText(currentGameTime);
    }

    public void AddToTime(float timeToAdd)
    {
        currentGameTime += timeToAdd;
        UIManager.instance.UpdateTimerText(currentGameTime);
    }

    public void TickTime()
    {
        currentGameTime -= Time.deltaTime;
        UIManager.instance.UpdateTimerText(currentGameTime);

        if(currentGameTime < 0)
        {
            GameManager.instance.GameOver();
        }
    }
}
