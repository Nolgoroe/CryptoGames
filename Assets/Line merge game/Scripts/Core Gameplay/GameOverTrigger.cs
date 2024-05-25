using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [Header("Trigger Preset Data")]
    [SerializeField] float timeToLose;

    [Header("Trigger Live Data")]
    [SerializeField] float currentTimer;
    [SerializeField] bool invincible;

    List<Collider2D> CollidersDetected = new List<Collider2D>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.gameIsRunning) return;
        if (invincible) return;

        if(collision.gameObject.layer == 6 && !CollidersDetected.Contains(collision))
        {
            CollidersDetected.Add(collision);
        }
    }

    private void Update()
    {
        if(!GameManager.gameIsRunning) return;

        if(CollidersDetected.Count > 0)
        {
            currentTimer += Time.deltaTime;

            if (timeToLose - currentTimer < timeToLose - 1)
                UIManager.instance.UpdateGameOverTimerTextText(true, timeToLose - currentTimer);

            if (currentTimer >= timeToLose)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && CollidersDetected.Contains(collision))
        {
            CollidersDetected.Remove(collision);

            if(CollidersDetected.Count <= 0)
            {
                CollidersDetected.Clear();
                ResetTimer();
            }
        }
    }

    private void ResetTimer()
    {
        currentTimer = 0;

        UIManager.instance.UpdateGameOverTimerTextText(false, currentTimer);
    }





    public void SetInvincibility(bool inInvincibile)
    {
        invincible = inInvincibile;
    }
}
