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

    [SerializeField] List<Collider> CollidersDetected = new List<Collider>();

    private void OnTriggerStay(Collider other)
    {
        if (!GameManager.gameIsRunning) return;
        if (invincible) return;

        if (other.gameObject.layer == 6 && !CollidersDetected.Contains(other))
        {
            CollidersDetected.Add(other);
        }
    }
    private void Start()
    {
        StartCoroutine(CleanAction());
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6 && CollidersDetected.Contains(other))
        {
            CollidersDetected.Remove(other);

            if (CollidersDetected.Count <= 0)
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


    private IEnumerator CleanAction()
    {
        yield return new WaitForSeconds(1);

        foreach (Collider col in CollidersDetected.ToArray())
        {
            if (col == null)
                CollidersDetected.Remove(col);

            if (CollidersDetected.Count <= 0)
            {
                CollidersDetected.Clear();
                ResetTimer();
            }
        }

        StartCoroutine(CleanAction());
    }



    public void SetInvincibility(bool inInvincibile)
    {
        invincible = inInvincibile;
    }
}
