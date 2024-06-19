using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool gameIsRunning;
    public static bool gameIsControllable;
    public static event Action onGameOver;
    public static BallDatabaseSO staticBallDatabase;
    public static int maxBallIndexReached;

    [SerializeField] int limitMaxBall;
    [SerializeField] int originalBallLimit = 3;

    [Header("Needed References")]
    [SerializeField] BallDatabaseSO currentBallDatabase;

    ITimer timerObject;
    List<IPrelude> preludeActions = new List<IPrelude>();

    //Temp
    public int startDelay = 5;
    //public int maxballs;

    private void Awake()
    {
        DateTime now = DateTime.Now;
        string seedString = now.ToString("yyyyMMddHHmmssfff");
        int seed = seedString.GetHashCode();

        UnityEngine.Random.InitState(seed);
        Application.targetFrameRate = 60;

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

        //flag - temp
        maxBallIndexReached = originalBallLimit;
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
        SetGameIsControllable(false);

        onGameOver?.Invoke();
    }

    public void UpdateBallIndexReached(int index)
    {
        maxBallIndexReached = index;

        if(maxBallIndexReached > limitMaxBall)
        {
            maxBallIndexReached = limitMaxBall;
        }
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

    public void SetGameIsControllable(bool isControllable)
    {
        gameIsControllable = isControllable;
    }



    //Temp
    private IEnumerator DelayBeforeStart()
    {
        yield return new WaitForSeconds(startDelay);
        gameIsRunning = true;
        SetGameIsControllable(true);
    }

    //Temp functions
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }


    public void CallReActivateControllable()
    {
        StartCoroutine(ReactivateControllable());
    }
    IEnumerator ReactivateControllable()
    {
        yield return new WaitForSeconds(0.5f);
        SetGameIsControllable(true);
    }

}
