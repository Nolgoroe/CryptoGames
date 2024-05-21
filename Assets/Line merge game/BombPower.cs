using System.Collections.Generic;
using UnityEngine;

public class BombPower : PowerupBase
{
    public override void UsePower()
    {
        // find most of same color balls in world.

        List<List<Ball>> relaventBallLists = GeneralStatsManager.instance.returnMostCommonBallLists();
        if (relaventBallLists.Count <= 0) return;

        int randomNum = Random.Range(0, relaventBallLists.Count);


        foreach (Ball ball in relaventBallLists[randomNum])
        {
            if (ball)
                Destroy(ball.gameObject);
        }

        ResetPowerUsage();
    }
}
