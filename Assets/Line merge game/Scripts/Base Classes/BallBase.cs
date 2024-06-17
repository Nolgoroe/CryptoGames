using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
abstract public class BallBase : MonoBehaviour
{
    [Header("Ball Display data")]
    [SerializeField] GameObject ballHighlight;

    [Header("Ball General stats")]
    [SerializeField] protected int ballIndex;
    [SerializeField] float ballOffsetSize = 1;
    [SerializeField] float startMass = 50;
    [SerializeField] float stationaryMassPercentage = 0.25f;
    [SerializeField] float timeToReduceMass = 4;
    [SerializeField] bool isBallMergable = false;
    [SerializeField] SpriteRenderer ballSpriteRenderer;
    float StationaryMass;

    [Header("Ball Scoring stats")]
    [SerializeField] int ballScoreSpawn = 1;
    [SerializeField] protected int ballScoreMerge = 2;

    [Header("Ball Powerup stats")]
    [SerializeField] protected float ballPowerToAdd = 1;

    int layerIndex;

    //temp serializable - Flag
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D col;

    //This might be a problem - violates the Liskov Sub priciple?
    bool isCombining; //should this be here even though not all balls that inherit this will be able to combine?? FLAG
    protected Action OnMergeBall;



    private void OnValidate()
    {
        if(col == null)
            col = GetComponent<Collider2D>();

        if(ballSpriteRenderer == null)
            ballSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        StationaryMass = startMass * stationaryMassPercentage;

        layerIndex = gameObject.layer;

        ScoreManager.instance.AddScore(ballScoreSpawn); //does this break single responsibility? FLAG. this happens on start of game after delay
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerIndex)
        {
            collision.gameObject.TryGetComponent<BallBase>(out BallBase otherBall);
            if (!otherBall || !isBallMergable) return;

            CheckCanDoMerge(otherBall);
        }
    }

    private void CheckCanDoMerge(BallBase otherBall)
    {
        // check if both balls are Imergables - if they are - continue
        // this limits us to combinig 2 balls for now - FLAG
        transform.TryGetComponent<IMergable>(out IMergable mergable);
        if (mergable == null) return;

        mergable.MergeAction(otherBall);

    }
    private void ReduceMass()
    {
        LeanTween.value(gameObject, rb.mass, StationaryMass, timeToReduceMass).setOnUpdate(ChangeMassAction);
    }

    private void ChangeMassAction(float value)
    {
        rb.mass = value;
    }




    protected void Start()
    {
        rb.mass = startMass;
        ReduceMass();
    }

    protected void OnMerge()
    {
        OnMergeBall?.Invoke();
    }




    public void SetIsCombining() //should this be here even though not all balls that inherit this will be able to combine?? FLAG
    {
        isCombining = true;
    }

    #region Public Returns
    public bool ReturnIsDuringCombine() //should this be here even though not all balls that inherit this will be able to combine?? FLAG
    {
        return isCombining;
    }
    public int ReturnBallIndex()
    {
        return ballIndex;
    }
    public float ReturnOffsetSize()
    {
        ballOffsetSize = transform.localScale.x / 2;

        return ballOffsetSize;
    }
    public float ReturnPowerToAdd()
    {
        return ballPowerToAdd;
    }

    public SpriteRenderer ReturnBallSpriteRenderer()
    {
        return ballSpriteRenderer;
    }

    public void ActivateBallHelighlight(bool turnOn)
    {
        if(ballHighlight)
        ballHighlight.SetActive(turnOn);
    }
    #endregion

}
