using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity; //FLAG

public enum ScoreType
{
    old,
    newNoMulti,
    NewAll
}

public class ScoreManager : MonoBehaviour, ISaveLoadable
{
    public static ScoreManager instance;

    [Header("Score Data")]
    [SerializeField] int currentScore;
    [SerializeField] int currentScoreNoMulti;
    [SerializeField] int currentOldScore;

    private void Awake()
    {
        instance = this;
    }





    public void AddOldScore(int amount)
    {        
        //temp
        if (!GameManager.gameIsRunning) return;

        currentOldScore += amount;

        UnityGoogleSheetsSaveData.Instance.UpdateScores(ScoreType.old, currentOldScore);
    }
    public void AddScoreNoMultis(int amount)
    {
        //temp
        if (!GameManager.gameIsRunning) return;

        currentScoreNoMulti += amount;

        UnityGoogleSheetsSaveData.Instance.UpdateScores(ScoreType.newNoMulti, currentScoreNoMulti);
    }

    public void AddScore(int amount)
    {        
        //temp
        if (!GameManager.gameIsRunning) return;

        currentScore += Mathf.RoundToInt(amount * ChainManager.currentMultiplierReached);
        UIManager.instance.UpdateScoreText(currentScore);

        UnityGoogleSheetsSaveData.Instance.UpdateScores(ScoreType.NewAll, currentScore);
    }

    public void RemoveScore(int amount)
    {
        currentScore -= amount;
        UIManager.instance.UpdateScoreText(currentScore);
    }





    public void LoadData(GameData data)
    {
        currentScore = data.gameCurrentScore;
        currentScoreNoMulti = data.gameCurrentScoreNoMulti;
        currentOldScore = data.gameCurrentOldScore;

        UIManager.instance.UpdateScoreText(currentScore);
    }

    public void SaveData(GameData data)
    {
        data.gameCurrentScore = currentScore;
        data.gameCurrentScoreNoMulti = currentScoreNoMulti;
        data.gameCurrentOldScore = currentOldScore;
    }
}
