using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyPower : PowerupBase
{
    [Header("Preset data")]
    [SerializeField] Transform spawnArea;
    [SerializeField] float spawnAreaOffset;
    [SerializeField] float timeBetweenSpawns = 0.1f;
    [SerializeField] int minAmount = 15;
    [SerializeField] int maxAmount = 20;


    public override void UsePower()
    {
        StartCoroutine(FrenzySpawn());
    }

    private IEnumerator FrenzySpawn()
    {
        int randomNum = Random.Range(minAmount, maxAmount);
        Vector3 center = spawnArea.transform.position;

        for (int i = 0; i < randomNum; i++)
        {
            Ball toSpawn = GameManager.staticBallDatabase.ReturnRandomBall();
            float randomX = Random.Range((-spawnArea.localScale.x / 2) + spawnAreaOffset,
                (spawnArea.localScale.x / 2) - spawnAreaOffset);

            Vector3 newPos = center + new Vector3(randomX, 0, 0);

            Instantiate(toSpawn, newPos, Quaternion.identity);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        ResetPowerUsage();
    }
}
