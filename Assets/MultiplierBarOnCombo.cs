using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierBarOnCombo : MonoBehaviour, IChainAction
{
    [SerializeField] Image multiplierBarFill;
    [SerializeField] float amountToAddtoMulti = 0.2f;
    [SerializeField] float minimumFillSize = 0.15f;
    [SerializeField] float barEmptySpeed = 0.1f;

    bool isGoingDown = false;
    ChainManager chainManager;

    private void Start()
    {

        chainManager = GetComponent<ChainManager>(); // this will force all chain logic to sit on chain manager object... FLAG

        if (chainManager)
            chainManager.AddObserver(this);
    }

    private void OnDestroy()
    {
        if (chainManager)
            chainManager.RemoveObserver(this);
    }

    private void Update()
    {
        if (isGoingDown && ChainManager.currentMultiplierReached >= 1)
        {
            float X = multiplierBarFill.transform.localScale.x;
            float Y = multiplierBarFill.transform.localScale.y;
            float Z = multiplierBarFill.transform.localScale.z;

            multiplierBarFill.transform.localScale = new Vector3(X - (Time.deltaTime * barEmptySpeed), Y, Z);

            if(multiplierBarFill.transform.localScale.x <= minimumFillSize)
            {
                if(ChainManager.currentMultiplierReached > 1)
                {
                    multiplierBarFill.transform.localScale = new Vector3(1, Y, Z);

                    chainManager.ChangeMulti(-amountToAddtoMulti);
                }
                else
                {
                    isGoingDown = false;
                }
            }
        }
    }

    public void NotifyObserver(BallBase ball, int currentComboReached)
    {
        isGoingDown = false;

        float X = multiplierBarFill.transform.localScale.x;
        float Y = multiplierBarFill.transform.localScale.y;
        float Z = multiplierBarFill.transform.localScale.z;

        multiplierBarFill.transform.localScale = new Vector3(X + ball.ReturnMultiplierToAdd(), Y,Z);

        if(multiplierBarFill.transform.localScale.x >= 1)
        {
            multiplierBarFill.transform.localScale = new Vector3(minimumFillSize, Y, Z);

            chainManager.ChangeMulti(amountToAddtoMulti);
        }
    }

    public void NotifyObserverReset()
    {
        // some effect on reset?

        isGoingDown = true;
    }
}
