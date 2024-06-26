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
    public static bool gameIsRestarting;
    public static event Action onGameOver;
    public static BallDatabaseSO staticBallDatabase;
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
        gameIsRestarting = false; 

        foreach (IPrelude toDo in preludeActions)
        {
            toDo.DoAction();
        }

        StartCoroutine(DelayBeforeStart()); //FLAG temp - think of better way to wait for all prelude to finish.. maybe remove from list when done and then check when list is empty after every remove?

        UnityGoogleSheetsSaveData.Instance.UpdateAllBallSizes(currentBallDatabase.balls);

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

        UnityGoogleSheetsSaveData.Instance.UpdateEndCondition("Lose");

        onGameOver?.Invoke();
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

    public void ButtonRestartGame()
    {
        UnityGoogleSheetsSaveData.Instance.UpdateEndCondition("Restart");
        RestartGame();
    }
    private void RestartGame()
    {
        if (gameIsRestarting) return;

        gameIsRestarting = true;
        UpdateSaveData();

        UnityGoogleSheetsSaveData.Instance.CallSaveState();

        //add delay

        StartCoroutine(DelayBeforeReset());
    }

    IEnumerator DelayBeforeReset()
    {
        yield return new WaitForSeconds(0.5f); //FLAG magic numbers

        UnityGoogleSheetsSaveData.Instance.DataReset(); //FLAG - WHY IS THIS HERE?!

        yield return new WaitForSeconds(0.5f); //FLAG magic numbers
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
        UnityGoogleSheetsSaveData.Instance.AddToGamesThisSession();
    }
}
