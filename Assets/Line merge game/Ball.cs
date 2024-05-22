using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [Header("Ball General stats")]
    [SerializeField] int ballIndex;
    [SerializeField] float ballOffsetSize;
    [SerializeField] float startMass;
    [SerializeField] float stationaryMassPercentage;
    [SerializeField] float timeToReduceMass;
    float StationaryMass;

    [Header("Ball Scoring stats")]
    [SerializeField] int ballScoreSpawn;
    [SerializeField] int ballScoreMerge;

    [Header("Ball Timer stats")]
    //[SerializeField] float ballTimeToAdd;

    [Header("Ball Powerup stats")]
    [SerializeField] float ballPowerToAdd;

    int layerIndex;
    bool isCombining;
    Rigidbody2D rb;

    public event Action OnMergeBall;


    //public LayerMask tempLayer;
    //public bool isExploding;
    //public float timeToExplode;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StationaryMass = startMass * stationaryMassPercentage;

        layerIndex = gameObject.layer;

        ScoreManager.instance.AddScore(ballScoreSpawn); //does this break single responsibility? FLAG

        GeneralStatsManager.instance.AddToBallAmountList(this); //FLAG - I feel this is weird..

    }

    private void Start()
    {
        OnMergeBall += OnMerge;

        rb.mass = startMass;

        ReduceMass();
    }

    private void Update()
    {
        //if (isExploding)
        //{
        //    timeToExplode -= Time.deltaTime;

        //    if(timeToExplode < 0)
        //    {
        //        isExploding = false;
        //        Explode();
        //    }
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerIndex)
        {
            collision.gameObject.TryGetComponent<Ball>(out Ball otherBall);
            if (!otherBall) return;

            if (ballIndex == otherBall.ReturnBallIndex() && !otherBall.isCombining)
            {
                //make sure this merge logic happends only once by getting the instance ID of both objects and letting the higher one manage the merge.
                int thisID = gameObject.GetInstanceID();
                int otherID = otherBall.gameObject.GetInstanceID();

                if (thisID > otherID)
                {
                    if (ballIndex + 1 < GameManager.staticBallDatabase.balls.Length)
                    {
                        if(ballIndex + 1 > GameManager.maxBallIndexReached && ballIndex + 1 < GameManager.staticBallDatabase.balls.Length - 1)
                        {
                            GameManager.instance.UpdateBallIndexReached(ballIndex + 1); //does this break single responsibility? FLAG
                        }

                        Vector3 MidPos = (transform.position + collision.transform.position) / 2;
                        Ball newBall = GameManager.staticBallDatabase.balls[ballIndex + 1];

                        if (newBall)
                        {
                            SetIsCombining();
                            otherBall.SetIsCombining();

                            //basic actions that need to happen always are - destroy current and other ball and spawn the merged ball
                            GameObject go = Instantiate(newBall.gameObject, MidPos, Quaternion.identity);

                            OnMergeBall?.Invoke(); // additional logic.

                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        OnMergeBall?.Invoke(); // additional logic.

                        Destroy(collision.gameObject);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
    
    //private void Explode()
    //{
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5, tempLayer);
    //    foreach (Collider2D hit in colliders)
    //    {
    //        if (hit.gameObject == gameObject) continue;

    //        Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

    //        if (rb != null)
    //        {
    //            Vector2 distanceVec = hit.transform.position - transform.position;
    //            if (distanceVec.magnitude > 0)
    //            {
    //                float explodeForce = 7500;
    //                rb.AddForce((distanceVec.normalized * explodeForce), ForceMode2D.Impulse);
    //            }
    //        }
    //    }


    //    Destroy(gameObject);
    //}
    private void ReduceMass()
    {
        LeanTween.value(gameObject, rb.mass, StationaryMass, timeToReduceMass).setOnUpdate(ChangeMassAction);
    }

    private void ChangeMassAction(float value)
    {
        rb.mass = value;
    }
    private void OnMerge()
    {        

        //handles anything that is extended from base logic of destroying and spawning balls.
        //add score, show effects, etc...

        ScoreManager.instance.AddScore(ballScoreMerge); //does this break single responsibility? FLAG
        PowerupManager.instance.UpdateCurrentPowerAmount(ballPowerToAdd);
        GeneralStatsManager.instance.AddToChainCount();

    }

    public void SetIsCombining()
    {
        isCombining = true;
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
