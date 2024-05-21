using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBarMovement : MonoBehaviour, ITimer
{
    [Header("Preset data")]
    [SerializeField] float delayBetweenMoves;
    [SerializeField] float amountMoveY;
    [SerializeField] float moveTime;
    [SerializeField] GameOverTrigger gameOverTrigger;
    [SerializeField] float minHeight;

    [Header("Live data")]
    [SerializeField] float currentDelay;


    public void InitTimer()
    {
        currentDelay = delayBetweenMoves;
    }

    public void TickTime()
    {
        currentDelay -= Time.deltaTime;

        if(currentDelay < 0)
        {
            currentDelay = delayBetweenMoves;

            //Move Game over trigger down
            TweenGameOverTrigger(-SetAmountTomove());
        }
    }

    public void AddToTime(int timeToAdd)
    {

    }

    private float SetAmountTomove()
    {
        if(gameOverTrigger.transform.position.y - amountMoveY <= minHeight)
        {
            return gameOverTrigger.transform.position.y - minHeight;
        }

        return amountMoveY;
    }
    private void TweenGameOverTrigger(float amount)
    {
        float currentY = gameOverTrigger.gameObject.transform.position.y;
        LeanTween.moveY(gameOverTrigger.gameObject, currentY + amount, moveTime);
    }
}
