using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerButtonData : MonoBehaviour
{
    [SerializeField] Sprite emptyImage;
    [SerializeField] Sprite FilledImage;
    [SerializeField] Image baseImage;
    [SerializeField] Image fillImage;

    public void ChangeFillAmount(float _fill)
    {
        fillImage.fillAmount = _fill;

        if(fillImage.fillAmount != 1)
        {
            baseImage.sprite = emptyImage;
        }
        else
        {
            baseImage.sprite = FilledImage;
            fillImage.fillAmount = 0;
        }
    }
}
