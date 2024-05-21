using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    [SerializeField] int ballIndex;
    [SerializeField] int ballScoreSpawn;
    [SerializeField] int ballScoreMerge;
    [SerializeField] float ballOffsetSize;
    int layerIndex;

    public event Action OnMergeBall;
    private void Awake()
    {
        layerIndex = gameObject.layer;

        ScoreManager.instance.AddScore(ballScoreSpawn); //does this break single responsibility? FLAG
    }

    private void Start()
    {
        OnMergeBall += OnMerge;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerIndex)
        {
            collision.gameObject.TryGetComponent<Ball>(out Ball otherBall);
            if (!otherBall) return;

            if (ballIndex == otherBall.ReturnBallIndex())
            {
                //make sure this merge logic happends only once by getting the instance ID of both objects and letting the higher one manage the merge.
                int thisID = gameObject.GetInstanceID();
                int otherID = otherBall.gameObject.GetInstanceID();

                if (thisID > otherID)
                {
                    if (ballIndex + 1 < GameManager.staticBallDatabase.balls.Length)
                    {
                        if(ballIndex + 1 > GameManager.furthestBallIndexReached && ballIndex + 1 < GameManager.staticBallDatabase.balls.Length - 1)
                        {
                            GameManager.instance.UpdateBallIndexReached(ballIndex + 1); //does this break single responsibility? FLAG
                        }

                        Vector3 MidPos = (transform.position + collision.transform.position) / 2;
                        Ball newBall = GameManager.staticBallDatabase.balls[ballIndex + 1];

                        if (newBall)
                        {
                            //basic actions that need to happen always are - destroy current and other ball and spawn the merged ball
                            Instantiate(newBall.gameObject, MidPos, Quaternion.identity);

                            OnMergeBall?.Invoke(); // additional logic.

                            Destroy(collision.gameObject);
                            Destroy(gameObject);

                        }
                    }
                }
            }
        }
    }
    
    private void OnMerge()
    {
        //handles anything that is extended from base logic of destroying and spawning balls.
        //add score, show effects, etc...

        ScoreManager.instance.AddScore(ballScoreMerge); //does this break single responsibility? FLAG
    }

    #region Public Returns
    public int ReturnBallIndex()
    {
        return ballIndex;
    }
    public float ReturnOffsetSize()
    {
        return ballOffsetSize;
    }
    #endregion
}
