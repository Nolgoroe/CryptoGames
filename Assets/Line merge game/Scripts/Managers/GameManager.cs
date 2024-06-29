using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using GoogleSheetsForUnity; //FLAG - Should be here?

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool gameIsRunning;
    public static bool gameIsControllable;
    public static event Action onGameOver;
    public static BallDatabaseSO staticBallDatabase;
    public static int limitMaxBall;

    [SerializeField] int controlLimitMaxBall;
    //[SerializeField] int originalBallLimit = 3;

    [Header("Needed References")]
    [SerializeField] BallDatabaseSO currentBallDatabase;

    ITimer timerObject;
    List<IPrelude> preludeActions = new List<IPrelude>();

    //Temp
    public int startDelay = 5;
    //public int maxballs;

    private void OnEnable()
    {
        onGameOver = null;
    }

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
        limitMaxBall = controlLimitMaxBall;


        UpdateSaveData();

        UnityGoogleSheetsSaveData.Instance.DataReset(); //FLAG - WHY IS THIS HERE?!

        onGameOver += RestartGame;
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
        limitMaxBall = index;

        if(limitMaxBall > controlLimitMaxBall)
        {
            limitMaxBall = controlLimitMaxBall;
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



    private IEnumerator DelayBeforeStart()
    {
        yield return new WaitForSeconds(startDelay);
        gameIsRunning = true;
        SetGameIsControllable(true);
    }

    public void RestartGame()
    {
        UnityGoogleSheetsSaveData.Instance.CallSaveState();

        //add delay

        StartCoroutine(DelayBeforeReset());
    }

    IEnumerator DelayBeforeReset()
    {
        yield return new WaitForSeconds(2); //FLAG magic numbers
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


    private void UpdateSaveData()
    {
        UnityGoogleSheetsSaveData.Instance.UpdateRangeOfBalls(0, limitMaxBall);
        UnityGoogleSheetsSaveData.Instance.AddToGamesThisSession();
    }
}
