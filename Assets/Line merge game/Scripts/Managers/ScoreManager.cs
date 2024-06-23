using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Score Data")]
    [SerializeField] int currentScore;

    private void Awake()
    {
        instance = this;
    }





    public void AddScore(int amount)
    {        
        //temp
        if (!GameManager.gameIsRunning) return;

        currentScore += Mathf.RoundToInt(amount * ChainManager.currentMultiplierReached);
        UIManager.instance.UpdateScoreText(currentScore);
    }

    public void RemoveScore(int amount)
    {
        currentScore -= amount;
        UIManager.instance.UpdateScoreText(currentScore);
    }
}
