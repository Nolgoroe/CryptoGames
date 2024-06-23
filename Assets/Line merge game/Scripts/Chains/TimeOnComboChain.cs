using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOnComboChain : MonoBehaviour, IChainAction
{
    [Header("Chain Action Data")]
    [SerializeField] int bonusThreshold = 5;
    [SerializeField] float timeToAddOnCombo = 3;

    ChainManager chainManager;

    private void Start()
    {
        chainManager = GetComponent<ChainManager>(); // this will force all chain logic to sit on chain manager object... FLAG

        if(chainManager)
        chainManager.AddObserver(this);
    }

    private void OnDestroy()
    {
        if (chainManager)
            chainManager.RemoveObserver(this);
    }





    public void NotifyObserver(BallBase ball, int currentComboReached)
    {
        if (currentComboReached % bonusThreshold == 0)
        {
            GameManager.instance.SendAddToTimer(timeToAddOnCombo);
        }
    }

    public void NotifyObserverReset()
    {
        // some effect on reset?
    }
}
