using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity; //FLAG - Should be here?

[System.Serializable]
public class BallAmountList
{
    public int amountOfBall;
    public List<BallBase> ballList;
}
[System.Serializable]
public class DeadBallItem
{
    public float curretnTime = 0;
    public BallBase ball;
}

[System.Serializable]
public class ComboCounter
{
    public int comboNumber;
    public int timesReached;
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
    [SerializeField] List<ComboCounter> comboCounterList;

    [Header("Dead balls tracker")]
    [SerializeField] List<DeadBallItem> deadBallList;
    [SerializeField] float timeToCountDeadBall = 2;

    [Header("Consecutive ball drop tracker")]
    [SerializeField] int consecutiveBallsDropped = 0;
    [SerializeField] float currentConsecutiveTimer = 2;
    [SerializeField] float consecutiveTimerMaxTime = 2;
    [SerializeField] bool consecutiveCounterOn = false;

    [SerializeField] List<BallAmountList> ballAmountList = new List<BallAmountList>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        deadBallList = new List<DeadBallItem>();
        comboCounterList = new List<ComboCounter>();

        currentConsecutiveTimer = consecutiveTimerMaxTime;

        CreateBallAmountList();

        StartCoroutine(CleanAction());

    }

    private void Update()
    {
        if(deadBallList.Count > 0)
        {
            foreach (DeadBallItem deadBall in deadBallList.ToArray())
            {
                deadBall.curretnTime += Time.deltaTime;

                if(deadBall.curretnTime >= timeToCountDeadBall)
                {
                    deadBallList.Remove(deadBall);
                    UnityGoogleSheetsSaveData.Instance.AddToDeadBalls();
                }
            }
        }

        if(consecutiveCounterOn)
        {
            currentConsecutiveTimer -= Time.deltaTime;
            if(currentConsecutiveTimer <= 0)
            {
                consecutiveCounterOn = false;
                currentConsecutiveTimer = consecutiveTimerMaxTime;
            }
        }
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

    public void StartBallDroppedTimer()
    {
        if(currentConsecutiveTimer < consecutiveTimerMaxTime)
        {
            consecutiveBallsDropped++;

            UnityGoogleSheetsSaveData.Instance.AddToConsecutiveBallsDroppedWithinXTime();
        }


        consecutiveCounterOn = true;
        currentConsecutiveTimer = consecutiveTimerMaxTime;
    }

    public void StartDeadBallTimer(BallBase ball)
    {
        DeadBallItem deadBall = new DeadBallItem();
        deadBall.ball = ball;

        deadBallList.Add(deadBall);
    }
    public void StopDeadBallTimer()
    {
        deadBallList.Clear();
    }

    public void AddToComboCounter(int comboNumber)
    {
        if (comboNumber < 2) return;

        foreach (ComboCounter counter in comboCounterList)
        {
            if (counter.comboNumber == comboNumber)
            {
                counter.timesReached++;
                return;
            }
        }


        ComboCounter newCounter = new ComboCounter();
        newCounter.comboNumber = comboNumber;
        newCounter.timesReached = 1;
        comboCounterList.Add(newCounter);
    }
    #region Public Return Data
    public List<List<BallBase>> ReturnMostCommonBallLists()
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
    public List<BallBase> ReturnSpecificBallList(int index)
    {
        return ballAmountList[index].ballList;
    }
    public List<BallAmountList> ReturnBallAmountsList()
    {
        return ballAmountList;
    }
    public List<ComboCounter> ReturnComboCounterList()
    {
        return comboCounterList;
    }

    public void HeighlightBallsFromIndex(int indexFrom, bool activate)
    {
        for (int i = indexFrom; i >= 0; i--)
        {
            foreach (BallBase ball in ballAmountList[i].ballList)
            {
                ball.ActivateBallHelighlight(activate);
            }
        }
    }

    #endregion
}
