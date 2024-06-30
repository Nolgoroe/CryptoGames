using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity; //FLAG
public enum FillAmount
{
    fill30,
    fill60,
    fill90
}

public class ContainerOverXPercent : MonoBehaviour
{
    [Header("Trigger Preset Data")]
    [SerializeField] float timeToActivate;
    [SerializeField] FillAmount fillAmount;

    [Header("Trigger Live Data")]
    [SerializeField] float currentTimer;
    [SerializeField] bool isTriggered = false;


    [SerializeField] List<Collider> CollidersDetected = new List<Collider>();

    private void OnTriggerStay(Collider other)
    {
        if (!GameManager.gameIsRunning) return;

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
        if (!GameManager.gameIsRunning) return;

        if (!isTriggered && CollidersDetected.Count > 0)
        {
            currentTimer += Time.deltaTime;

            if (currentTimer >= timeToActivate)
            {
                isTriggered = true;

                if(fillAmount == FillAmount.fill60) //FLAG
                {
                    PowerupManager.instance.ToggleContainterAboveSixty(isTriggered);
                }

                UnityGoogleSheetsSaveData.Instance.UpdateContainerFilledTimes(fillAmount);
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
                currentTimer = 0;
            }
        }

        StartCoroutine(CleanAction());
    }
}
