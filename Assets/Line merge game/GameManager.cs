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
    [SerializeField] Transform spawnArea;

    //Temp
    public int startDelay = 5;
    private void Awake()
    {
        SetBallDatabase(currentBallDatabase);

        StartCoroutine(DelayBeforeStart());

        instance = this;

        TryGetTimer();
        timerObject?.InitTimer();
    }

    private void Start()
    {
        StartCoroutine(SpawnOnStart());
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

    private IEnumerator SpawnOnStart()
    {
        int randomNum = UnityEngine.Random.Range(15, 25);
        Vector3 center = spawnArea.transform.position;

        for (int i = 0; i < randomNum; i++)
        {
            int randomBallIndex = UnityEngine.Random.Range(0, currentBallDatabase.balls.Length);

            Ball toSpawn = staticBallDatabase.ReturnRandomBallInIndex(randomBallIndex);
            float randomX = UnityEngine.Random.Range((-spawnArea.localScale.x / 2),
                (spawnArea.localScale.x / 2));

            Vector3 newPos = center + new Vector3(randomX, 0, 0);

            Instantiate(toSpawn, newPos, Quaternion.identity);

            yield return new WaitForSeconds(0.05f);
        }
    }

    //Temp

    private IEnumerator DelayBeforeStart()
    {
        yield return new WaitForSeconds(startDelay);
        gameIsRunning = true;
    }

}
