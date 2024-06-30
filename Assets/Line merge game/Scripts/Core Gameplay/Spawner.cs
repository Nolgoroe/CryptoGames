using GoogleSheetsForUnity; //FLAG - should this be here
using System;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class LimitedSpawnBalls
{
    public BallBase ball;
    public int percentageToSpawn = 100;
}
public class Spawner : MonoBehaviour, ISaveLoadable
{
    public static Spawner instance;


    [Header("Preset data")]
    [SerializeField] Transform leftBound;
    [SerializeField] Transform rightBound;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Transform lowerScreenLimit;
    [SerializeField] float ballBoundOffset = 1;
    [SerializeField] float constBoundOffset = 0.7f;
    [SerializeField] float delayBetweenDrops = 1;
    [SerializeField] float cancelTouchPosUpOffset = 0.5f;
    [SerializeField] LimitedSpawnBalls[] ballsCanSpawn;

    [Header("Live data")]
    [SerializeField] float currentDelayBetweenDrops = 0;
    [SerializeField] GameObject currentNonPhysDisplay;
    [SerializeField] BallBase currentPhysBall;
    [SerializeField] int currentPhysBallIndex;
    [SerializeField] BallBase nextPhysBall;

    [Header("Live Spawn data")]
    [SerializeField] BallBase classPhysBallSpawned;
    [SerializeField] BallBase livePhysBallSpawned;

    float leftBoundX, rightBoundX;

    private void Start()
    {
        instance = this;

        DateTime now = DateTime.Now;
        //string seedString = now.ToString("yyyyMMddHHmmssfff");
        string seedString = now.ToString("yyyyMMdd");
        int seed = seedString.GetHashCode();

        UnityEngine.Random.InitState(seed);

        UnityGoogleSheetsSaveData.Instance.UpdateRangeOfBalls(0, ballsCanSpawn.Length - 1);

        currentDelayBetweenDrops = 0;

        GameManager.onGameOver += ResetSpawnerData;

        SpawnBallOnStart();
    }

    private void Update()
    {
        if (!GameManager.gameIsRunning || !GameManager.gameIsControllable) return;
        if (EventSystem.current.IsPointerOverGameObject(0)) return;

        CountDropCooldown();


        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);

        //if (!CheckIsTouchPosLowerPlayer(touch.position)) return;
        //if (!CheckIsTouchPosAboveLowerLimit(touch.position)) return;

        if (touch.phase == TouchPhase.Began)
        {
            FollowMouse();
        }

        if (touch.phase == TouchPhase.Moved)
        {
            FollowMouse();
        }

        if (touch.phase == TouchPhase.Ended)
        {
            if (currentDelayBetweenDrops > 0) return;

            Destroy(currentNonPhysDisplay.gameObject);
            BallBase go = Instantiate(currentPhysBall, spawnPosition.position, Quaternion.identity);

            //set tracking instantaited ball data
            classPhysBallSpawned = GameManager.staticBallDatabase.balls[go.ReturnBallIndex()];
            livePhysBallSpawned = go;

            currentPhysBall = null;

            DisplayNextBall();

            ResetDropCooldown();

            //on spawn action event should be here - Flag
            PowerupManager.instance.UpdateCurrentPowerAmount(go.ReturnPowerToAdd()); //does this break single responsibility? FLAG

            GeneralStatsManager.instance.StartDeadBallTimer(go); //Flag - should this be here?
            GeneralStatsManager.instance.StartBallDroppedTimer(); //Flag - should this be here?

            UnityGoogleSheetsSaveData.Instance.UpdateDataOnDropBall();
        }
    }

    private bool CheckIsTouchPosLowerPlayer(Vector2 touchPos)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(touchPos);

        return pos.y < transform.position.y - cancelTouchPosUpOffset;
    }
    private bool CheckIsTouchPosAboveLowerLimit(Vector2 touchPos)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(touchPos);

        return pos.y >= lowerScreenLimit.position.y;
    }

    private void FollowMouse()
    {
        //follow mouse on X while clamped to right and left bounds.
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 newPos = new Vector3(mouse.x, transform.position.y, transform.position.z);
        newPos.x = Mathf.Clamp(newPos.x, leftBoundX, rightBoundX);

        transform.position = newPos;

    }
    private void ResetDropCooldown()
    {
        currentDelayBetweenDrops = delayBetweenDrops;
    }
    private void CountDropCooldown()
    {
        if (currentDelayBetweenDrops > 0)
        {
            currentDelayBetweenDrops -= Time.deltaTime;
        }
    }

    private void SpawnBallOnStart()
    {
        //int randomNum = UnityEngine.Random.Range(0, Spawner.limitMaxBall + 1); //Excludes last num, so + 1 to reverse the exclude.
        currentPhysBall = RollBallNumbers();
        SpawnNonPhysDisplay(currentPhysBall.ReturnBallIndex());

        SetNewBoundOffset(currentPhysBall);

        DecideNextBall();
    }
    private void DisplayNextBall()
    {
        if (!nextPhysBall) return;

        currentPhysBall = nextPhysBall;
        currentPhysBallIndex = currentPhysBall.ReturnBallIndex();

        int currentBallIndex = currentPhysBall.ReturnBallIndex();
        SpawnNonPhysDisplay(currentBallIndex);

        SetNewBoundOffset(currentPhysBall);
        DecideNextBall();
    }

    private void SpawnNonPhysDisplay(int ballIndex)
    {
        currentNonPhysDisplay = Instantiate(GameManager.staticBallDatabase.NonPhysicBalls[ballIndex], spawnPosition);
    }
    private void DecideNextBall()
    {
        nextPhysBall = RollBallNumbers();

        //set next ball in UI - send for UI Manager to take care of it.
        UIManager.instance.SetNextBallDisplay(nextPhysBall);
    }

    private BallBase RollBallNumbers()
    {
        BallBase ball = null;

        int randomNum = UnityEngine.Random.Range(0, 101);
        Debug.Log(randomNum);
        int low;    
        int high = 0;
        for (int i = 0; i < ballsCanSpawn.Length; i++)
        {
            low = high; // set low limit
            high += ballsCanSpawn[i].percentageToSpawn; // set high limit

            if (randomNum >= low && randomNum <= high)
            {
                ball = GameManager.staticBallDatabase.balls[i];
                return ball;
            }
        }






        return ball;

    }
    private void SetNewBoundOffset(BallBase ballData)
    {
        // balls change size - to prevent a ball from clipping into a boundry, we just change the offset.
        // The offset is from the side boundries - the bigger the ball, the bigger the offset and vice versa.
        ballBoundOffset = ballData.ReturnOffsetSize();
        leftBoundX = leftBound.transform.position.x + constBoundOffset + ballBoundOffset;
        rightBoundX = rightBound.transform.position.x - constBoundOffset - ballBoundOffset;
    }

    private void ResetSpawnerData()
    {
        Destroy(currentNonPhysDisplay.gameObject);
        currentPhysBall = null;
    }



    public void ForceNewBall(BallBase otherBall)
    {
        ////get rid of current displayed ball.
        Destroy(currentNonPhysDisplay.gameObject);

        ////spawn the next ball's display as non phys
        int nextBallIndex = otherBall.ReturnBallIndex();
        SpawnNonPhysDisplay(nextBallIndex);

        currentPhysBall = otherBall;
    }

    public void UndoAction()
    {
        if (livePhysBallSpawned)
            Destroy(livePhysBallSpawned.gameObject);

        ForceNewBall(classPhysBallSpawned);
    }
    //public void UpdateBallIndexReached(int index)
    //{
    //    limitMaxBall = index;

    //    if (limitMaxBall > 2)
    //    {
    //        limitMaxBall = 2;
    //    }
    //}



    public void LoadData(GameData data)
    {
        currentPhysBallIndex = data.currentBallDropping;
        ForceNewBall(GameManager.staticBallDatabase.balls[currentPhysBallIndex]);
    }

    public void SaveData(GameData data)
    {
        data.currentBallDropping = currentPhysBallIndex;
    }
}
