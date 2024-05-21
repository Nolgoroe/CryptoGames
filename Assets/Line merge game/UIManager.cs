using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Image nextBallDisplay;
    [SerializeField] TMP_Text gameTimerText;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScoreText(int amount)
    {
        scoreText.text = amount.ToString();
    }

    public void SetNextBallDisplay(Ball nextBall)
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
}
