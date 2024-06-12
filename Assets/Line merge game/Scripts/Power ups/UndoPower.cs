using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoPower : PowerupBase
{
    [SerializeField] Spawner player;

    public override void UsePower()
    {
        player.UndoAction();

        localResetData();
    }

    protected override void localResetData()
    {
        ResetPowerUsage();
    }
}
