using System.Collections.Generic;
using UnityEngine;

public class BombPower : PowerupBase
{
    [SerializeField] LayerMask transformableLayerMask;

    bool usingPower = false;
    public override void UsePower()
    {
        usingPower = true;
        GameManager.gameIsControllable = false;
        // find most of same color balls in world.

        //List<BallBase> relaventBallLists = GeneralStatsManager.instance.ReturnSpecificBallList(0);
        //if (relaventBallLists.Count <= 0) return;

        //int randomNum = Random.Range(0, relaventBallLists.Count);


        //foreach (BallBase ball in relaventBallLists)
        //{
        //    if (ball)
        //        Destroy(ball.gameObject);
        //}

        //ResetPowerUsage();
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
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.forward, 1000, transformableLayerMask);

        if(hit)
        {
            Debug.Log("Something Hit");
            hit.transform.gameObject.AddComponent<BombBall>();

            localResetData();
        }
    }

    protected override void localResetData()
    {
        GameManager.gameIsControllable = true;
        usingPower = false;

        ResetPowerUsage();
    }
}
