using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity; //FLAG - Should be here?

[System.Serializable]
public class comboSaveData
{
    public int currentComboReached;
    public float currentTimeResetCombo;
    public bool ComboDetected;
}

public class ChainManager : MonoBehaviour, ISaveLoadable
{
    public static ChainManager instance;

    List<IChainAction> chainActions = new List<IChainAction>(); //ovserver design pattern

    [Header("Combo Detection Stats")]
    [SerializeField] float timeBeforeResetCombo;


    [Header("live Combo Data")]
    public comboSaveData comboData;

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
        if (comboData.ComboDetected)
        {
            comboData.currentTimeResetCombo -= Time.deltaTime;

            if (comboData.currentTimeResetCombo < 0)
            {
                ResetChainTimer();

                ResetAllObservers();
            }
        }
    }

    private void ResetChainTimer()
    {
        comboData.ComboDetected = false;
        comboData.currentTimeResetCombo = timeBeforeResetCombo;

        Debug.Log("Reset at: " + comboData.currentComboReached);

        GeneralStatsManager.instance.AddToComboCounter(comboData.currentComboReached);
        comboData.currentComboReached = 0;
    }

    private void NotifyAllObservers(BallBase ball)
    {
        foreach (IChainAction observer in chainActions)
        {
            observer.NotifyObserver(ball, comboData.currentComboReached);
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

        comboData.currentComboReached++;
        UnityGoogleSheetsSaveData.Instance.TranslateComboListToSpecificData(comboData.currentComboReached);

        comboData.currentTimeResetCombo = timeBeforeResetCombo;

        comboData.ComboDetected = true;

        NotifyAllObservers(ball);

        GeneralStatsManager.instance.TrackComboData(comboData.currentComboReached);
    }

    public void AddObserver(IChainAction observerChain)
    {
        chainActions.Add(observerChain);
    }

    public void RemoveObserver(IChainAction observerChain)
    {
        chainActions.Remove(observerChain);
    }





    public void LoadData(GameData data)
    {
        comboData = data.comboSaveData;
    }

    public void SaveData(GameData data)
    {
        data.comboSaveData = comboData;
    }
}
