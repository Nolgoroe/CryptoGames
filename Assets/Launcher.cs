using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Rigidbody ballPrefab;
    [SerializeField] private Transform ballSpawnPosition;
    [SerializeField] private float maxForce = 35;
    [SerializeField] private float forceAddRate = 10;
    [SerializeField] private float pullSpeed = 10;
    [SerializeField] private float returnSpeed = 0.2f;
    [SerializeField] private Transform pullPoint;

    [SerializeField] private Rigidbody currentBall;
    [SerializeField] private float currentForce = 0;

    bool holding = false;


    void Update()
    {
        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            currentBall = Instantiate(ballPrefab, ballSpawnPosition.transform.position, Quaternion.identity);
        }

        if (touch.phase == TouchPhase.Stationary)
        {
            holding = true;

            if (currentForce < maxForce)
            {
                currentForce += Time.deltaTime * forceAddRate;
            }
        }


        if (touch.phase == TouchPhase.Ended)
        {
            holding = false;

            currentBall.AddForce(new Vector3(0, 1, 0) * currentForce, ForceMode.Impulse);

            ResetData();
        }
    }

    private void FixedUpdate()
    {
        if(holding)
        {
            if (Vector3.Distance(transform.position, pullPoint.transform.position) > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, pullPoint.transform.position, Time.fixedDeltaTime * pullSpeed);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.zero, Time.fixedDeltaTime * returnSpeed);
        }
    }

    private void ResetData()
    {
        currentBall = null;
        currentForce = 0;
    }
}
