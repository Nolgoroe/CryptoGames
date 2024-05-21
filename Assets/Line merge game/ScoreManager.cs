using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] int currentScore;

    private void Awake()
    {
        instance = this;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UIManager.instance.UpdateScoreText(currentScore);
    }
    public void RemoveScore(int amount)
    {
        currentScore -= amount;
        UIManager.instance.UpdateScoreText(currentScore);
    }
}
