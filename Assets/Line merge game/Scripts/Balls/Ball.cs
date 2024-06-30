using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity; //Flag - should not be here
public class Ball : BallBase, IMergable
{
    private void Start()
    {
        base.Start();

        OnMergeBall += OnMergeActions;

        GeneralStatsManager.instance.AddToBallAmountList(this);
    }
    private void SpawnNewBall(BallBase otherBall)
    {
        Vector3 MidPos = (transform.position + otherBall.transform.position) / 2;
        BallBase newBall = GameManager.staticBallDatabase.balls[ballIndex + 1];

        if (newBall)
        {
            SetIsCombining();
            otherBall.SetIsCombining();
            ReduceMassManual();
            //basic actions that need to happen always are - destroy current and other ball and spawn the merged ball
            BallBase go = Instantiate(newBall, MidPos, Quaternion.identity);
            go.TestTestFunc();
            OnMerge();
            Destroy(otherBall.gameObject);
            Destroy(gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void MergeAction(BallBase otherBall)
    {
        if (ballIndex == otherBall.ReturnBallIndex() && !otherBall.ReturnIsDuringCombine())
        {
            //make sure this merge logic happends only once by getting the instance ID of both objects and letting the higher one manage the merge.
            int thisID = gameObject.GetInstanceID();
            int otherID = otherBall.gameObject.GetInstanceID();

            if (thisID > otherID)
            {
                if (ballIndex + 1 < GameManager.staticBallDatabase.balls.Length)
                {
                    if (ballIndex + 1 > GameManager.limitMaxBall && ballIndex + 1 < GameManager.staticBallDatabase.balls.Length - 1)
                    {
                        GameManager.instance.UpdateBallIndexReached(ballIndex + 1); //does this break single responsibility? FLAG
                    }

                    SpawnNewBall(otherBall);

                }
                else
                {
                    //we reach here if the merging balls are the biggest size
                    OnMerge();
                    Destroy(otherBall.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void OnMergeActions()
    {
        //handles anything that is extended from base logic of destroying and spawning balls.
        //add score, show effects, etc...

        ScoreManager.instance.AddScore(ballScoreMerge); //does this break single responsibility? FLAG
        ScoreManager.instance.AddScoreNoMultis(ballScoreMerge); //does this break single responsibility? FLAG
        ScoreManager.instance.AddOldScore(ballScoreMergeOld); //does this break single responsibility? FLAG
        
        
        //PowerupManager.instance.UpdateCurrentPowerAmount(ballPowerToAdd); //does this break single responsibility? FLAG
        ChainManager.instance.AddToChainCount(this); //does this break single responsibility? FLAG
        GeneralStatsManager.instance.StopDeadBallTimer(); //Flag - should this be here?



        UnityGoogleSheetsSaveData.Instance.AddToTotalMerges();
        UnityGoogleSheetsSaveData.Instance.UpdateToMaxBallIndexReached(ballIndex + 1);
    }

}
