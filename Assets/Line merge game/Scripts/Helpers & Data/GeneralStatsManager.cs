using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BallAmountList
{
    public int amountOfBall;
    public List<BallBase> ballList;
}
public class GeneralStatsManager : MonoBehaviour
{
    public static GeneralStatsManager instance;

    [Header("Combo Parameters")]
    [Tooltip("From what amount of combo do we consider a chain to be a bonus?")]
    [SerializeField] int bonusThreshold = 5;

    [Header("Combo Data")]
    [SerializeField] int highestComboReached;
    [SerializeField] int amountOfBonusesAquired;


    /*[SerializeField]*/ List<BallAmountList> ballAmountList = new List<BallAmountList>();

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
            newElement.ballList = new List<BallBase>();

            ballAmountList.Add(newElement);
        }
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

        StartCoroutine(CleanAction()); 
    }



    public void AddToBallAmountList(BallBase ball)
    {
        ballAmountList[ball.ReturnBallIndex()].amountOfBall++;
        ballAmountList[ball.ReturnBallIndex()].ballList.Add(ball);
    }

    public void TrackComboData(int currentComboReached )
    {
        if (currentComboReached > highestComboReached)
        {
            highestComboReached = currentComboReached;
        }

        if (currentComboReached % bonusThreshold == 0)
        {
            amountOfBonusesAquired++;
        }
    }


    #region Public Return Data
    public List<List<BallBase>> returnMostCommonBallLists()
    {
        List<List<BallBase>> ballLists = new List<List<BallBase>>();

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
