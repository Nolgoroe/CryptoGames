using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;

    //[SerializeField] Button powerupButtonPrefab;
    [SerializeField] Transform powerupButtonsParent;

    [SerializeField] PowerupBase[] currentPowerupsAllowed;

    public bool anyPowerBeingUsed = false;

    private void Awake()
    {
        instance = this; //FLAG - I hate the usage on Singletons in this project - think of a better way.
    }

    private void Start()
    {
        for (int i = 0; i < currentPowerupsAllowed.Length; i++)
        {
            Button button = Instantiate(currentPowerupsAllowed[i].PublicPowerButtonPrefab, powerupButtonsParent);

            // VARIABLE capture - not VALUE capture - so we create a new variable to capture each iteration.
            int currentIndex = i;
            button.onClick.AddListener(() => PowerButtonClicked(currentPowerupsAllowed[currentIndex]));

            currentPowerupsAllowed[i].InitPower(button);
        }
    }

    public void PowerButtonClicked(PowerupBase power)
    {
        if (anyPowerBeingUsed) return;

        power?.UsePower();
    }

    public void UpdateCurrentPowerAmount(float amount)
    {
        //temp
        if (!GameManager.gameIsRunning) return;

        foreach (PowerupBase power in currentPowerupsAllowed)
        {
            power?.AddToPower(amount);
        }
    }


    public void ToggleContainterAboveSixty(bool isAbove)
    {
        foreach (PowerupBase power in currentPowerupsAllowed)
        {
            power?.ChangePowerNeeded(isAbove);
        }
    }
}
