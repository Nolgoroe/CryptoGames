using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
abstract public class BallBase : MonoBehaviour
{
    [Header("Ball Display data")]
    [SerializeField] GameObject ballHighlight;

    [Header("Ball General stats")]
    [SerializeField] protected int ballIndex;
    [SerializeField] float ballOffsetSize = 1;
    [SerializeField] float startMass = 100;
    [SerializeField] float startMassMerge = 1000;
    [SerializeField] float stationaryMassPercentage = 0.25f;
    [SerializeField] float timeToReduceMass = 4;
    [SerializeField] bool isBallMergable = false;
    [SerializeField] SpriteRenderer ballSpriteRenderer;
    float StationaryMass;

    [Header("Ball Live stats")]
    [SerializeField] Vector3 ballPosition;

    [Header("Ball Scoring stats")]
    [SerializeField] int ballScoreSpawn = 1;
    [SerializeField] protected int ballScoreMerge = 2;
    [SerializeField] protected int ballScoreMergeOld = 2;

    [Header("Ball Multiplier stats")]
    [SerializeField] float ballMultiplierToAdd = 1;

    [Header("Ball Powerup stats")]
    [SerializeField] protected float ballPowerToAdd = 1;

    [Header("Ball Gravity Data")]
    [SerializeField] float currentDownForce = 0;
    [SerializeField] float spawnDownForce = -4;
    [SerializeField] float stationairyDownForce = 19.6f;
    [SerializeField] float maxDownwardForce = 200;
    [SerializeField] float completeForceCalc = 0;


    [Header("Ball Gravity Data")]
    [SerializeField] PhysicMaterial ballMaterialTest;
    [SerializeField] PhysicMaterial normalMaterial;
    [SerializeField] float timeToChangeMat = 0.1f;

    int layerIndex;
    bool collided = false;

    //temp serializable - Flag
    [SerializeField] Rigidbody rb;
    [SerializeField] Collider col;

    //This might be a problem - violates the Liskov Sub priciple?
    bool isCombining; //should this be here even though not all balls that inherit this will be able to combine?? FLAG
    protected Action OnMergeBall;


    private void OnValidate()
    {
        if (ballSpriteRenderer == null)
            ballSpriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        if (col == null)
            col = GetComponent<Collider>();

    }
    private void Awake()
    {

        rb.mass = startMass;
        //rb.useGravity = false;

        StationaryMass = startMass * stationaryMassPercentage;
        layerIndex = gameObject.layer;
        ScoreManager.instance.AddScore(ballScoreSpawn); //does this break single responsibility? FLAG. this happens on start of game after delay
    }

    public void TestTestFunc()
    {
        StartCoroutine(TestFunc());
    }
    IEnumerator TestFunc()
    {
        rb.mass = startMassMerge;
        col.sharedMaterial = ballMaterialTest;

        yield return new WaitForSeconds(timeToChangeMat);

        rb.mass = StationaryMass;
        col.sharedMaterial = normalMaterial;

    }

    private void OnCollisionEnter(Collision collision)
    {
        //currentDownForce = stationairyDownForce;
        collided = true;
        //rb.useGravity = true;

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

    private void Update()
    {
        ballPosition = transform.position;
    }
    private void FixedUpdate()
    {

        if (!collided)
        {
            //rb.AddForce(new Vector3(0, currentDownForce, 0), ForceMode.VelocityChange);
        }
        else
        {
            if (rb.velocity.magnitude < 0.1f)
            {
                //currentForceTest = (currentDownForce * transform.position.y) - rb.mass;

                completeForceCalc = stationairyDownForce * rb.mass;
                completeForceCalc = Mathf.Clamp(completeForceCalc, 100, maxDownwardForce);

                rb.AddForce(Vector3.down * completeForceCalc, ForceMode.Force);
            }
        }
    }

    protected void Start()
    {
        currentDownForce = spawnDownForce;
        ReduceMass();
        //ChangeDownForce();
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
    public Vector3 ReturnBallPosition()
    {
        return ballPosition;
    }
    public Vector3 ReturnBallVelocity()
    {
        return rb.velocity;
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
    public float ReturnMultiplierToAdd()
    {
        return ballMultiplierToAdd;
    }

    public SpriteRenderer ReturnBallSpriteRenderer()
    {
        return ballSpriteRenderer;
    }

    public void ActivateBallHelighlight(bool turnOn)
    {
        if (ballHighlight)
            ballHighlight.SetActive(turnOn);
    }
    #endregion



    [ContextMenu("reduce mass manual")]
    public void ReduceMassManual()
    {
        rb.mass = rb.mass * stationaryMassPercentage;
    }

    [ContextMenu("Add force manual")]
    public void ManualSetVelocity(Vector3 vel)
    {
        if(rb != null)
            rb.velocity = vel;
    }
}
