using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitControlsWhenAlive : MonoBehaviour
{
    // FLAG - THIS SCRIPT CURRENTLY SITS ON ROOF BECAUSE IT ACTIAVES WHEN BOMB IS ACTIVE
    // NOT SCALABLE - THINK OF BETTER OPTION

    [SerializeField] float timeToDeactivate = 3;
    [SerializeField] bool limitsPlayerControl = false;

    private void OnEnable()
    {
        if(limitsPlayerControl)
        GameManager.instance.SetGameIsControllable(false);

        StartCoroutine(DeactivateAfterX());
    }

    IEnumerator DeactivateAfterX()
    {
        yield return new WaitForSeconds(timeToDeactivate);

        if (limitsPlayerControl)
            GameManager.instance.CallReActivateControllable();

        gameObject.SetActive(false);
    }
}
