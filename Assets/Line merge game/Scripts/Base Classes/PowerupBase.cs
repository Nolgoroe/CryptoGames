using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerupBase : MonoBehaviour
{
    [Header("needed refs")]
    [SerializeField] Image powerupImage;
    [SerializeField] protected Button powerupButton;

    [Header("Base data")]
    [SerializeField] float amountNeededToUsePower = 1;
    [SerializeField] float lowerOnContainerOverSixty = 1;

    [Header("Live data")]
    [SerializeField] float currentPowerAmount;


    private void Start()
    {
        currentPowerAmount = 0;
    }


    public void InitPower(Button InpowerButton)
    {
        powerupButton = InpowerButton;
        powerupImage = InpowerButton.image;
    }

    protected virtual void ResetPowerUsage()
    {
        currentPowerAmount = 0;
        powerupButton.interactable = false;
        UpdatePowerupImage(currentPowerAmount, amountNeededToUsePower);
    }



    public abstract void UsePower(); // public since called from button
    protected abstract void localResetData();

    public virtual void AddToPower(float amount)
    {
        currentPowerAmount += amount;

        if (currentPowerAmount >= amountNeededToUsePower)
            currentPowerAmount = amountNeededToUsePower;

        if (currentPowerAmount / amountNeededToUsePower == 1)
        {
            powerupButton.interactable = true;
        }
        else
        {
            powerupButton.interactable = false;
        }

        UpdatePowerupImage(currentPowerAmount, amountNeededToUsePower);
    }
    public void ChangePowerNeeded(bool isOver)
    {
        amountNeededToUsePower = isOver == true ? amountNeededToUsePower - lowerOnContainerOverSixty : amountNeededToUsePower + lowerOnContainerOverSixty;

        UpdatePowerupImage(currentPowerAmount, amountNeededToUsePower);
    }

    private void UpdatePowerupImage(float current, float max)
    {
        float currentFill = current / max;
        powerupImage.fillAmount = currentFill;
    }



    public float ReturnAmountNeededForPower()
    {
        return amountNeededToUsePower;
    }
}
