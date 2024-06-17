using System.Collections.Generic;
using UnityEngine;

public class BombPower : PowerupBase
{
    [Header("Bomb data")]
    [SerializeField] LayerMask detectionLayer;
    [SerializeField] GameObject boardRoof;

    bool usingPower = false;
    public override void UsePower()
    {
        usingPower = true;
        GameManager.gameIsControllable = false;
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
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.forward, 1000, detectionLayer);

        if(hit)
        {
            Debug.Log("Something Hit");
            hit.transform.gameObject.AddComponent<BombBall>();

            localResetData();

            boardRoof.gameObject.SetActive(true);
        }
    }

    protected override void localResetData()
    {
        //GameManager.gameIsControllable = true; when the roof deactivates - we return control to player
        usingPower = false;

        ResetPowerUsage();
    }
}
