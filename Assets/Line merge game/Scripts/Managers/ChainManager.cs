using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChainManager : MonoBehaviour
{
    public static ChainManager instance;

    List<IChainAction> chainActions = new List<IChainAction>(); //ovserver design pattern

    [Header("Combo Detection Stats")]
    [SerializeField] float timeBeforeResetCombo;

    [Header("live Combo Data")]
    [SerializeField] int currentComboReached;
    [SerializeField] float currentTimeResetCombo;
    [SerializeField] bool ComboDetected;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ResetChainTimer();
    }

    private void Update()
    {
        if (ComboDetected)
        {
            currentTimeResetCombo -= Time.deltaTime;

            if (currentTimeResetCombo < 0)
            {
                ResetChainTimer();
            }
        }
    }

    private void ResetChainTimer()
    {
        ComboDetected = false;
        currentTimeResetCombo = timeBeforeResetCombo;

        Debug.Log("Reset at: " + currentComboReached);
        currentComboReached = 0;

    }

    private void NotifyAllObservers()
    {
        foreach (IChainAction observer in chainActions)
        {
            observer.NotifyObserver(currentComboReached);
        }
    }





    public void AddToChainCount()
    {
        //temp
        if (!GameManager.gameIsRunning) return;

        currentComboReached++;

        currentTimeResetCombo = timeBeforeResetCombo;

        ComboDetected = true;

        NotifyAllObservers();

        GeneralStatsManager.instance.TrackComboData(currentComboReached);
    }

    public void AddObserver(IChainAction observerChain)
    {
        chainActions.Add(observerChain);
    }

    public void RemoveObserver(IChainAction observerChain)
    {
        chainActions.Remove(observerChain);
    }

}
