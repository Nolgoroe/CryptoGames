using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyPower : PowerupBase
{
    [Header("Preset data")]
    [SerializeField] RectTransform spawnArea;
    [SerializeField] GameOverTrigger gameOverTrigger;
    [SerializeField] float spawnAreaOffset;
    [SerializeField] float timeBetweenSpawns = 0.1f;
    [SerializeField] int minAmount = 15;
    [SerializeField] int maxAmount = 20;

    private void OnValidate()
    {
        if(!gameOverTrigger)
        gameOverTrigger = FindObjectOfType<GameOverTrigger>();
    }
    private IEnumerator FrenzySpawn()
    {
        gameOverTrigger.SetInvincibility(true);
        int randomNum = Random.Range(minAmount, maxAmount);
        Vector3 center = spawnArea.transform.position;

        for (int i = 0; i < randomNum; i++)
        {
            BallBase toSpawn = GameManager.staticBallDatabase.ReturnRandomBallInIndex(0);
            float randomX = Random.Range((-4) + spawnAreaOffset,
                (4) - spawnAreaOffset);

            Vector3 newPos = center + new Vector3(randomX, 0, 0);

            Instantiate(toSpawn, newPos, Quaternion.identity);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        localResetData();
    }


    public override void UsePower()
    {
        StartCoroutine(FrenzySpawn());
    }

    protected override void localResetData()
    {
        ResetPowerUsage();

        gameOverTrigger.SetInvincibility(false);
    }
}
