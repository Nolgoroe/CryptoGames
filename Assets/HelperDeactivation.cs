using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperDeactivation : MonoBehaviour
{
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
