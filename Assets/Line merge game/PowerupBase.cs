using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerupBase : MonoBehaviour
{
    [SerializeField] float amountNeededToUsePower;
    [SerializeField] float currentAmountHas;

    private void Start()
    {
        currentAmountHas = 0;
    }

    protected abstract void UsePower();
    protected virtual void AddToPower(float amount)
    {
        currentAmountHas += amount;
    }
}
