using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupDestroyPowerup : PowerupBase
{
    [SerializeField] LayerMask detectionLayer;

    bool usingPower = false;

    public override void UsePower()
    {
        usingPower = true;
        GameManager.instance.SetGameIsControllable(false);

    }
    private void Update()
    {
        if (!usingPower) return;
        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            TryDestroyGroup(touch.position);
        }

    }
    private void TryDestroyGroup(Vector2 touchPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 0));
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.forward, 1000, detectionLayer);

        if (hit)
        {
            BallBase ballBase;
            if (!hit.transform.TryGetComponent<BallBase>(out ballBase)) return;


            Debug.Log("Something Hit");

            DestroyAction(ballBase);

            localResetData();
        }
    }

    private void DestroyAction(BallBase ballBase)
    {
        //find same color balls in world.
        List<BallBase> relaventBallLists = GeneralStatsManager.instance.ReturnSpecificBallList(ballBase.ReturnBallIndex());


        foreach (BallBase ball in relaventBallLists)
        {
            if (ball)
                Destroy(ball.gameObject);
        }
    }

    protected override void localResetData()
    {
        GameManager.instance.CallReActivateControllable();
        usingPower = false;

        ResetPowerUsage();
    }
}
