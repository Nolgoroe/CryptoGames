using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Preset data")]
    [SerializeField] Transform leftBound;
    [SerializeField] Transform rightBound;
    [SerializeField] float boundOffset = 1; //FLAG - there has to be a better way - need to think of a dynamic and scalable solution.

    [Header("Live data")]
    [SerializeField] GameObject currentNonPhysDisplay;
    [SerializeField] Ball currentPhysBall;
    [SerializeField] Ball nextPhysBall;

    float leftBoundX, rightBoundX;

    private void Start()
    {
        GameManager.onGameOver += ResetSpawnerData;

        SpawnBallOnStart();
    }

    private void Update()
    {
        if (!GameManager.gameIsRunning) return;

        //follow mouse on X while clamped to right and left bounds.
        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        Vector3 newPos = new Vector3(mouse.x, transform.position.y, transform.position.z);
        newPos.x = Mathf.Clamp(newPos.x, leftBoundX, rightBoundX);

        transform.position = newPos;

        // release ball
        if (Input.GetMouseButtonDown(0))
        {
            int randomNum = Random.Range(0, GameManager.staticBallDatabase.balls.Length);

            Destroy(currentNonPhysDisplay.gameObject);
            Instantiate(currentPhysBall.gameObject, transform.position, Quaternion.identity);
            currentPhysBall = null;


            DisplayNextBall();
        }

        if(Input.GetMouseButtonDown(1))
        {
            //Swap currentPhysBall to nextPhysBall
            SwapBalls();
        }
    }

    private void SwapBalls()
    {
        //get rid of current displayed ball.
        Destroy(currentNonPhysDisplay.gameObject);

        //spawn the next ball's display as non phys
        int nextBallIndex = nextPhysBall.ReturnBallIndex();
        currentNonPhysDisplay = Instantiate(GameManager.staticBallDatabase.NonPhysicBalls[nextBallIndex], transform);

        //current ball = next and next = current
        Ball tempBall = currentPhysBall;
        currentPhysBall = nextPhysBall;
        nextPhysBall = tempBall;

        UIManager.instance.SetNextBallDisplay(nextPhysBall);
    }

    private void SpawnBallOnStart()
    {
        int randomNum = Random.Range(0, GameManager.furthestBallIndexReached + 1); //Excludes last num, so + 1 to reverse the exclude.
        currentPhysBall = GameManager.staticBallDatabase.balls[randomNum];
        currentNonPhysDisplay = Instantiate(GameManager.staticBallDatabase.NonPhysicBalls[randomNum], transform);

        SetNewBoundOffset(currentPhysBall);

        DecideNextBall();
    }
    private void DisplayNextBall()
    {
        if (!nextPhysBall) return;

        currentPhysBall = nextPhysBall;
        int currentBallIndex = currentPhysBall.ReturnBallIndex();
        currentNonPhysDisplay = Instantiate(GameManager.staticBallDatabase.NonPhysicBalls[currentBallIndex], transform);

        SetNewBoundOffset(currentPhysBall);
        DecideNextBall();
    }

    private void DecideNextBall()
    {
        int randomNum = Random.Range(0, GameManager.furthestBallIndexReached + 1); //Excludes last num, so + 1 to reverse the exclude.
        nextPhysBall = GameManager.staticBallDatabase.balls[randomNum];

        //set next ball in UI - send for UI Manager to take care of it.
        UIManager.instance.SetNextBallDisplay(nextPhysBall);
    }
    private void SetNewBoundOffset(Ball ballData)
    {
        // balls change size - to prevent a ball from clipping into a boundry, we just change the offset.
        // The offset is from the side boundries - the bigger the ball, the bigger the offset and vice versa.
        boundOffset = ballData.ReturnOffsetSize();
        leftBoundX = leftBound.transform.position.x + boundOffset;
        rightBoundX = rightBound.transform.position.x - boundOffset;
    }

    private void ResetSpawnerData()
    {
        Destroy(currentNonPhysDisplay.gameObject);
        currentPhysBall = null;
    }
}
