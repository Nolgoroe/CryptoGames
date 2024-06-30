using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class BallSaveData
{
    public int ballIndex;
    public Vector3 ballPosition;
    public Vector3 ballVelocity;
}

[System.Serializable]
public class BallsTracker : MonoBehaviour, ISaveLoadable
{
    [SerializeField] List<BallSaveData> ballSaveData = new List<BallSaveData>();

    public void LoadData(GameData data)
    {
        ballSaveData = data.gameBalls;

        CreateBallsOnLoad();
    }

    public void SaveData(GameData data)
    {
        ballSaveData.Clear();

        List<BallBase> tempBallBaseList = FindAllBalls();

        foreach (BallBase ball in tempBallBaseList)
        {
            BallSaveData ballData = new BallSaveData();
            ballData.ballIndex = ball.ReturnBallIndex();
            ballData.ballPosition = ball.ReturnBallPosition();
            ballData.ballVelocity = ball.ReturnBallVelocity();

            ballSaveData.Add(ballData);
        }


        data.gameBalls = ballSaveData;
    }
    private List<BallBase> FindAllBalls()
    {
        //find all monobehaviours that inherit from ballBase - all of the balls we want to save.
        IEnumerable<BallBase> ballBases = FindObjectsOfType<MonoBehaviour>().OfType<BallBase>();

        //initialize a new list with the data of all the objects we just found.
        return new List<BallBase>(ballBases);
    }

    private void CreateBallsOnLoad()
    {
        List<BallBase> ballsSpawned = new List<BallBase>();

        foreach (BallSaveData ball in ballSaveData)
        {
            BallBase toSpawn = GameManager.staticBallDatabase.balls[ball.ballIndex];
            BallBase spawnedBall = Instantiate(toSpawn, ball.ballPosition, Quaternion.identity);
            ballsSpawned.Add(spawnedBall);
        }


        if(ballsSpawned.Count > 0)
            StartCoroutine(ApplyDataToBalls(ballsSpawned));
    }

    IEnumerator ApplyDataToBalls(List<BallBase> ballsSpawned)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < ballsSpawned.Count; i++)
        {
            ballsSpawned[i].ManualSetVelocity(ballSaveData[i].ballVelocity);
        }
    }
}
