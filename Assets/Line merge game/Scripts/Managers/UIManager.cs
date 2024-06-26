using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Timers")]
    [SerializeField] TMP_Text gameTimerText;

    [Header("Balls left counter")]
    [SerializeField] TMP_Text ballsLeftText;

    [Header("Balls")]
    [SerializeField] Image nextBallDisplay;

    [Header("Score")]
    [SerializeField] TMP_Text scoreText;

    [Header("Score Multi")]
    [SerializeField] TMP_Text multiplierText;

    [Header("Game over trigger")]
    [SerializeField] TMP_Text gameOverTriggerText;


    private void Awake()
    {
        instance = this;
    }

    public void UpdateScoreText(int amount)
    {
        scoreText.text = amount.ToString();
    }
    public void UpdateScoreMultiText(float amount)
    {
        multiplierText.text =  "X" + amount.ToString("F1");
    }

    public void SetNextBallDisplay(BallBase nextBall)
    {
        //balls are sprite renderers.

        nextBall.TryGetComponent<SpriteRenderer>(out SpriteRenderer nextBallSR);
        if (nextBallSR == null) return;

        nextBallDisplay.sprite = nextBallSR.sprite;
        //nextBallDisplay.transform.localScale = nextBall.transform.localScale;
    }

    public void UpdateTimerText(float time)
    {
        int fullNum = Mathf.RoundToInt(time);
        gameTimerText.text = fullNum.ToString();
    }
    public void UpdateBallsLeftText(int amount)
    {
        ballsLeftText.text = amount.ToString();
    }
    public void UpdateGameOverTimerTextText(bool active, float time)
    {
        gameOverTriggerText.gameObject.SetActive(active);
        int fullNum = Mathf.RoundToInt(time);
        gameOverTriggerText.text = fullNum.ToString();
    }

}
