using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBootsPopup : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 2;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] float minX = -200;
    [SerializeField] float maxX = 200;
    [SerializeField] float minY = -730;
    [SerializeField] float maxY = 730;

    private void OnEnable()
    {
        transform.localPosition = new Vector3(Random.Range(minX, 200), Random.Range(minY, maxY), 0);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
        Destroy(gameObject, timeToDestroy);
    }
    public void InitPopup(int scoreAmount)
    {
        scoreText.text = scoreAmount.ToString();
    }
}
