using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScoreText(int amount)
    {
        scoreText.text = amount.ToString();
    }
}
