using System.Collections.Generic;
using UnityEngine;

public class BombPower : PowerupBase
{
    [Header("Bomb data")]
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] GameObject boardRoof;
    [SerializeField] int maxBallIndexAllowed = 4;

    [Header("Needed references")]
    [SerializeField] Animator bombEffectPrefab;

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
            TryTransformToBomb(touch.position);
        }
    }


    private void TryTransformToBomb(Vector2 touchPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 0));
        if(Physics.Raycast(worldPos, Vector3.forward, out RaycastHit hit, 1000, detectionLayer))
        {
            hit.transform.TryGetComponent<BallBase>(out BallBase ball);

            if (ball)
            {
                if (ball.ReturnBallIndex() > maxBallIndexAllowed) return;
            }

            BombBall bomb = hit.transform.gameObject.AddComponent<BombBall>();
            bomb.SetBombEffect(bombEffectPrefab);

            localResetData();

            boardRoof.gameObject.SetActive(true);
        }

    }

    protected override void localResetData()
    {
        usingPower = false;

        Destroy(spawnedScreen.gameObject);
        ResetPowerUsage();

        GeneralStatsManager.instance.HeighlightBallsFromIndex(maxBallIndexAllowed, false);
    }
}
