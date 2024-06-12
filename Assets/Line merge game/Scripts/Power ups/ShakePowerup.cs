using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePowerup : PowerupBase
{
    [SerializeField] ShakeObject shakeObject;
    public override void UsePower()
    {
        shakeObject.CallShake();

        localResetData();
    }


    protected override void localResetData()
    {
        ResetPowerUsage();
    }
}
