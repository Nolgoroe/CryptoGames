using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;

    private PowerupBase currentPowerupAllowed;


    private void Awake()
    {
        instance = this; //FLAG - I hate the usage on Singletons in this project - think of a better way.
    }

    private void Start()
    {
        transform.TryGetComponent<PowerupBase>(out currentPowerupAllowed);
    }





    public void PowerButtonClicked()
    {
        //called from button
        currentPowerupAllowed?.UsePower();
    }

    public void UpdateCurrentPowerAmount(float amount)
    {
        //temp
        if (!GameManager.gameIsRunning) return;

        currentPowerupAllowed?.AddToPower(amount);
    }
}
