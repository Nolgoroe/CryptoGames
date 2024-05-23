using System.Collections.Generic;
using UnityEngine;

public class BombPower : PowerupBase
{
    public override void UsePower()
    {
        // find most of same color balls in world.

        List<BallBase> relaventBallLists = GeneralStatsManager.instance.ReturnSpecificBallList(0);
        if (relaventBallLists.Count <= 0) return;

        int randomNum = Random.Range(0, relaventBallLists.Count);


        foreach (BallBase ball in relaventBallLists)
        {
            if (ball)
                Destroy(ball.gameObject);
        }

        ResetPowerUsage();
    }
}
