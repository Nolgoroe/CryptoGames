using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificObjectDestructionPower : PowerupBase
{
    [SerializeField] LayerMask transformableLayerMask;
    [SerializeField] int maxBallIndexAllowed = 4;

    bool usingPower = false;

    public override void UsePower()
    {
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
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.forward, 1000, transformableLayerMask);

        if (hit)
        {
            Debug.Log("Something Hit");
            Destroy(hit.transform.gameObject);

            localResetData();
        }
    }

    protected override void localResetData()
    {
        GameManager.instance.CallReActivateControllable();
        usingPower = false;

        ResetPowerUsage();

        GeneralStatsManager.instance.HeighlightBallsFromIndex(maxBallIndexAllowed, false);
    }
}
