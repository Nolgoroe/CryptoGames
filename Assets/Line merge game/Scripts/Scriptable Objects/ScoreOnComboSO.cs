using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreComboata
{
    public int comboIndex = 0;
    public int scoreBonus = 100;
}

[CreateAssetMenu(fileName = "Bonus score on combo", menuName = "ScriptableObjects/Create combo preset", order = 1)]
public class ScoreOnComboSO : ScriptableObject
{
    public ScoreComboata[] scoreComboDataArray;

    private void OnValidate()
    {
        for (int i = 0; i < scoreComboDataArray.Length; i++)
        {
            scoreComboDataArray[i].comboIndex = i;
        }
    }
}
