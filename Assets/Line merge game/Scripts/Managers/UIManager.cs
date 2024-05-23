using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Power ups")]
    [SerializeField] Image powerupImage;
    [SerializeField] Button powerupButton;

    [Header("Timers")]
    [SerializeField] TMP_Text gameTimerText;

    [Header("Balls")]
    [SerializeField] Image nextBallDisplay;

    [Header("Score")]
    [SerializeField] TMP_Text scoreText;


    private void Awake()
    {
        instance = this;
    }

    public void UpdateScoreText(int amount)
    {
        scoreText.text = amount.ToString();
    }

    public void SetNextBallDisplay(BallBase nextBall)
    {
        //balls are sprite renderers.

        nextBall.TryGetComponent<SpriteRenderer>(out SpriteRenderer nextBallSR);
        if (nextBallSR == null) return;

        nextBallDisplay.sprite = nextBallSR.sprite;
        nextBallDisplay.color = nextBallSR.color; //temp since we use colors and simple circle sprites for now - FLAG
        nextBallDisplay.transform.localScale = nextBall.transform.localScale;
    }

    public void UpdateTimerText(float time)
    {
        int fullNum = Mathf.RoundToInt(time);
        gameTimerText.text = fullNum.ToString();
    }

    public void UpdatePowerupImage(float current, float max)
    {
        float currentFill = current / max;
        powerupImage.fillAmount = currentFill;
        
        if(currentFill == 1)
        {
            powerupButton.interactable = true;
        }
        else
        {
            powerupButton.interactable = false;
        }
    }
}
