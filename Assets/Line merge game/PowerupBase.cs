using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupBase : MonoBehaviour
{
    [Header("Base data")]
    [SerializeField] float amountNeededToUsePower = 1;

    [Header("Live data")]
    [SerializeField] float currentPowerAmount;


    private void Start()
    {
        currentPowerAmount = 0;
    }

    public abstract void UsePower();
    public virtual void AddToPower(float amount)
    {
        currentPowerAmount += amount;

        if (currentPowerAmount > amountNeededToUsePower)
            currentPowerAmount = amountNeededToUsePower;

        //Update power display in UI manager
        UIManager.instance.UpdatePowerupImage(currentPowerAmount, amountNeededToUsePower);
    }

    protected virtual void ResetPowerUsage()
    {
        currentPowerAmount = 0;
        UIManager.instance.UpdatePowerupImage(currentPowerAmount, amountNeededToUsePower);
    }
}
