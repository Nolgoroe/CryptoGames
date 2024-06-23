using UnityEngine;

public class SpecificObjectDestructionPower : PowerupBase
{
    [Header("Preset Data")]
    [SerializeField] LayerMask layerToDetect;
    [SerializeField] int maxBallIndexAllowed = 4;

    [Header("Screen")]
    [SerializeField] private GameObject screenPrefab;
    [SerializeField] private Transform screenParent;

    bool usingPower = false;
    GameObject spawnedScreen;

    public override void UsePower()
    {        
        // show UI screen.
        if (spawnedScreen) return;

        spawnedScreen = Instantiate(screenPrefab, screenParent);

        usingPower = true;
        GameManager.instance.SetGameIsControllable(false);

        GeneralStatsManager.instance.HeighlightBallsFromIndex(maxBallIndexAllowed, true);
    }

    private void Update()
    {
        if (!usingPower) return;
        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            TryDestroyObject(touch.position);
        }

    }
    private void TryDestroyObject(Vector2 touchPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 0));
        if (Physics.Raycast(worldPos, Vector3.forward, out RaycastHit hit, 1000, layerToDetect))
        {
            hit.transform.TryGetComponent<BallBase>(out BallBase ball);

            if (ball)
            {
                if (ball.ReturnBallIndex() > maxBallIndexAllowed) return;
            }

            Debug.Log("Something Hit");
            Destroy(hit.transform.gameObject);

            localResetData();

        }
    }

    protected override void localResetData()
    {
        GameManager.instance.CallReActivateControllable();
        usingPower = false;

        Destroy(spawnedScreen.gameObject);
        ResetPowerUsage();

        GeneralStatsManager.instance.HeighlightBallsFromIndex(maxBallIndexAllowed, false);
    }
}
