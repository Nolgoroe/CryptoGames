using System.Collections.Generic;
using UnityEngine;

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

    [Header("live multiplier Data")]
    public static float currentMultiplierReached = 1;

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

                ResetAllObservers();
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

    private void NotifyAllObservers(BallBase ball)
    {
        foreach (IChainAction observer in chainActions)
        {
            observer.NotifyObserver(ball, currentComboReached);
        }
    }
    private void ResetAllObservers()
    {
        foreach (IChainAction observer in chainActions)
        {
            observer.NotifyObserverReset();
        }
    }



    public void ChangeMulti(float amount)
    {
        currentMultiplierReached += amount;

        if (currentMultiplierReached < 1)
            currentMultiplierReached = 1;

        //update UI
        UIManager.instance.UpdateScoreMultiText(currentMultiplierReached);
    }

    public void AddToChainCount(BallBase ball)
    {
        //temp
        if (!GameManager.gameIsRunning) return;

        currentComboReached++;

        currentTimeResetCombo = timeBeforeResetCombo;

        ComboDetected = true;

        NotifyAllObservers(ball);

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
