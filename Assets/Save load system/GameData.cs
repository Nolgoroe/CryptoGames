using GoogleSheetsForUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public SaveDataContainer googleSheetData;
    public comboSaveData comboSaveData;
    //public GeneralStatsSaveData generalStatsData;

    public List<BallSaveData> gameBalls;

    public int gameCurrentScore;
    public int gameCurrentScoreNoMulti;
    public int gameCurrentOldScore;

    public int currentBallDropping;

    public float[] currentPowerupAmounts;

    public GameData()
    {
        googleSheetData = new SaveDataContainer();
        comboSaveData = new comboSaveData();
        gameBalls = new List<BallSaveData>();
        gameCurrentScore = 0;
        gameCurrentScoreNoMulti = 0;
        gameCurrentOldScore = 0;
        currentBallDropping = 0;
        currentPowerupAmounts = new float[3];
        //generalStatsData = new GeneralStatsSaveData();
    }
}
