using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool gameIsRunning;
    public static event Action onGameOver;
    public static BallDatabaseSO staticBallDatabase;
    public static int maxBallIndexReached;

    ITimer timerObject;

    [SerializeField] BallDatabaseSO currentBallDatabase;

    private void Awake()
    {
        SetBallDatabase(currentBallDatabase);

        gameIsRunning = true;
        instance = this;

        TryGetTimer();
        timerObject?.InitTimer();
    }

    private void TryGetTimer()
    {
        transform.TryGetComponent<ITimer>(out timerObject);
    }
    private void SetBallDatabase(BallDatabaseSO database)
    {
        staticBallDatabase = database;
    }

    private void Update()
    {
        if (!gameIsRunning) return;

        timerObject?.TickTime();
    }

    public void GameOver()
    {
        gameIsRunning = false;

        onGameOver?.Invoke();
    }


    public void UpdateBallIndexReached(int index)
    {
        maxBallIndexReached = index;
    }
    public void SendAddToTimer(float amount)
    {
        timerObject?.AddToTime(amount);
    }
}
