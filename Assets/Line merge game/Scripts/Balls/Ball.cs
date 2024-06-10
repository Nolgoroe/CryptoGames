using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            //basic actions that need to happen always are - destroy current and other ball and spawn the merged ball
            BallBase go = Instantiate(newBall, MidPos, Quaternion.identity);

            OnMerge();
            Destroy(otherBall.gameObject);
            Destroy(gameObject);
        }
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
                    if (ballIndex + 1 > GameManager.maxBallIndexReached && ballIndex + 1 < GameManager.staticBallDatabase.balls.Length - 1)
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
        PowerupManager.instance.UpdateCurrentPowerAmount(ballPowerToAdd); //does this break single responsibility? FLAG
        ChainManager.instance.AddToChainCount(); //does this break single responsibility? FLAG
    }

}
