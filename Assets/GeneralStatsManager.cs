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

    [Header("Combo Detection Stats")]
    [SerializeField] float timeBeforeResetCombo;
    [SerializeField] int bonusThreshold;

    [Header("live Combo Data")]
    [SerializeField] int highestComboReached;
    [SerializeField] int currentComboReached;
    [SerializeField] int amountOfBonuses;
    [SerializeField] float currentTimeResetCombo;
    [SerializeField] bool ComboDetected;

    //Temp
    [Header("Temp data")]
    public float timeToAddOnCombo;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateBallAmountList();

        StartCoroutine(CleanAction());

        ResetChainTimer();
    }

    private void Update()
    {
        if(ComboDetected)
        {
            currentTimeResetCombo -= Time.deltaTime;

            if(currentTimeResetCombo < 0)
            {
                ResetChainTimer();
            }
        }
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

    private void ResetChainTimer()
    {
        ComboDetected = false;
        currentTimeResetCombo = timeBeforeResetCombo;

        if(currentComboReached > highestComboReached)
        {
            highestComboReached = currentComboReached;
        }

        Debug.Log("Reset at: " + currentComboReached);
        currentComboReached = 0;

    }
    public void AddToChainCount()
    {
        //temp
        if (!GameManager.gameIsRunning) return;

        currentComboReached++;
            
        if (currentComboReached % bonusThreshold == 0)
        {
            //add time to player
            GameManager.instance.SendAddToTimer(timeToAddOnCombo); // Flag - I do not like that this is here.
            amountOfBonuses++;
        }

        currentTimeResetCombo = timeBeforeResetCombo;

        ComboDetected = true;
    }

    public void AddToBallAmountList(Ball ball)
    {
        ballAmountList[ball.ReturnBallIndex()].amountOfBall++;
        ballAmountList[ball.ReturnBallIndex()].ballList.Add(ball);
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
