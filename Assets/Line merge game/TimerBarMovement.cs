using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBarMovement : MonoBehaviour, ITimer
{
    [Header("Preset data")]
    [SerializeField] float delayBetweenMoves = 2.5f;
    [SerializeField] float originalAmountMoveY = -1;
    [SerializeField] float moveTime = 1;
    [SerializeField] float minHeight = -3.7f;
    [SerializeField] float maxHeight = 4f;
    [SerializeField] GameOverTrigger gameOverTrigger;

    [Header("Live data")]
    [SerializeField] float currentDelay;
    [SerializeField] float amountMoveY;

    private void OnValidate()
    {
        if(!gameOverTrigger)
        gameOverTrigger = FindObjectOfType<GameOverTrigger>();
    }

    public void InitTimer()
    {
        currentDelay = delayBetweenMoves;
        amountMoveY = originalAmountMoveY;
    }

    public void TickTime()
    {
        currentDelay -= Time.deltaTime;

        if(currentDelay < 0)
        {
            currentDelay = delayBetweenMoves;

            //Move Game over trigger down

            TweenGameOverTrigger(SetAmountTomove(amountMoveY));
        }
    }

    public void AddToTime(float timeToAdd)
    {
        amountMoveY += timeToAdd;
    }

    private float SetAmountTomove(float amount)
    {
        if(amount < 0 && gameOverTrigger.transform.position.y + amount <= minHeight)
        {
            return gameOverTrigger.transform.position.y - minHeight;
        }

        if(amount > 0 && gameOverTrigger.transform.position.y + amount >= maxHeight)
        {
            return maxHeight - gameOverTrigger.transform.position.y ;
        }

        return amount;
    }
    private void TweenGameOverTrigger(float amount)
    {
        float currentY = gameOverTrigger.gameObject.transform.position.y;
        LeanTween.moveY(gameOverTrigger.gameObject, currentY + amount, moveTime).setEaseOutCubic();


        amountMoveY = originalAmountMoveY;
    }
}
