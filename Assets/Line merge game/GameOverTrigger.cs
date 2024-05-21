using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] float currentTimer;
    [SerializeField] float timeToLose;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.gameIsRunning) return;

        if(collision.gameObject.layer == 6)
        {
            currentTimer += Time.deltaTime;

            if(currentTimer >= timeToLose)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            currentTimer = 0;
        }
    }
}
