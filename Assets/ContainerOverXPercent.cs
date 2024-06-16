using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerOverXPercent : MonoBehaviour
{
    [Header("Trigger Preset Data")]
    [SerializeField] float timeToActivate;

    [Header("Trigger Live Data")]
    [SerializeField] float currentTimer;
    [SerializeField] bool isTriggered = false;


    [SerializeField] List<Collider2D> CollidersDetected = new List<Collider2D>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameManager.gameIsRunning) return;

        if (collision.gameObject.layer == 6 && !CollidersDetected.Contains(collision))
        {
            CollidersDetected.Add(collision);
        }
    }

    private void Update()
    {
        if (!GameManager.gameIsRunning) return;

        if (!isTriggered && CollidersDetected.Count > 0)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= timeToActivate)
            {
                isTriggered = true;
                PowerupManager.instance.ToggleContainterAboveSixty(isTriggered);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 && CollidersDetected.Contains(collision))
        {
            CollidersDetected.Remove(collision);

            if (CollidersDetected.Count <= 0)
            {
                ResetData();
            }
        }
    }

    private void ResetData()
    {
        currentTimer = 0;

        if (isTriggered)
        {
            isTriggered = false;
            PowerupManager.instance.ToggleContainterAboveSixty(isTriggered);
        }
    }
}
