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

    [Header("Needed References")]
    [SerializeField] BallDatabaseSO currentBallDatabase;

    ITimer timerObject;
    List<IPrelude> preludeActions = new List<IPrelude>();

    //Temp
    public int startDelay = 5;
    private void Awake()
    {
        SetBallDatabase(currentBallDatabase);


        instance = this;

        TryGetTimer();
        timerObject?.InitTimer();
    }

    private void Start()
    {
        foreach (IPrelude toDo in preludeActions)
        {
            toDo.DoAction();
        }

        StartCoroutine(DelayBeforeStart()); //FLAG temp - think of better way to wait for all prelude to finish.. maybe remove from list when done and then check when list is empty after every remove?
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
        //temp
        if (!gameIsRunning) return;

        timerObject?.AddToTime(amount);
    }

    public void AddToPreludeAcitons(IPrelude toAdd)
    {
        preludeActions.Add(toAdd);
    }

    public void RemoveFromPreludeAcitons(IPrelude toRemove)
    {
        preludeActions.Remove(toRemove);
    }





    //Temp
    private IEnumerator DelayBeforeStart()
    {
        yield return new WaitForSeconds(startDelay);
        gameIsRunning = true;
    }

}
