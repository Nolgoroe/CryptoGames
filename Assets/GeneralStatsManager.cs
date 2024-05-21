using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BallAmountList
{
    public int amountOfBall;
    public List<Ball> ballList;
}
public class GeneralStatsManager : MonoBehaviour
{
    public static GeneralStatsManager instance;
    [SerializeField] List<BallAmountList> ballAmountList = new List<BallAmountList>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateBallAmountList();

        StartCoroutine(CleanAction());
    }

    private void CreateBallAmountList()
    {
        for (int i = 0; i < GameManager.staticBallDatabase.balls.Length; i++)
        {
            BallAmountList newElement = new BallAmountList();
            newElement.ballList = new List<Ball>();

            ballAmountList.Add(newElement);
        }
    }

    public void AddToBallAmountList(Ball ball)
    {
        ballAmountList[ball.ReturnBallIndex()].amountOfBall++;
        ballAmountList[ball.ReturnBallIndex()].ballList.Add(ball);
    }
    private IEnumerator CleanAction()
    {
        yield return new WaitForSeconds(1);

        foreach (BallAmountList combo in ballAmountList)
        {
            for (int i = combo.ballList.Count - 1; i >= 0; i--)
            {
                if (combo.ballList[i] == null)
                {
                    combo.ballList.RemoveAt(i);
                }
            }
            combo.amountOfBall = combo.ballList.Count;
        }

        StartCoroutine(CleanAction()); //FLAG - I don't know if this endless loop is a great idea
    }

    #region Public Return Data
    public List<List<Ball>> returnMostCommonBallLists()
    {
        List<List<Ball>> ballLists = new List<List<Ball>>();

        foreach (BallAmountList combo in ballAmountList)
        {
            if(combo.ballList.Count >= 3)
            {
                ballLists.Add(combo.ballList);
            }
        }

        return ballLists;
    }
    #endregion
}
