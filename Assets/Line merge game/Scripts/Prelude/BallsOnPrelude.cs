using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsOnPrelude : MonoBehaviour, IPrelude
{
    [Header("Needed References")]
    [SerializeField] Transform spawnArea;

    [Header("Spawn Parameters")]
    [SerializeField] int minBalls = 15;
    [SerializeField] int maxBalls = 25;
    [SerializeField] float spawnBallDelay = 0.05f;

    private void Start()
    {
        AddToList();
    }

    private void OnDestroy()
    {
        RemoveFromList();
    }

    private IEnumerator SpawnOnStart()
    {
        int randomNum = UnityEngine.Random.Range(minBalls, maxBalls);
        Vector3 center = spawnArea.transform.position;

        for (int i = 0; i < randomNum; i++)
        {
            int randomBallIndex = UnityEngine.Random.Range(0, GameManager.staticBallDatabase.balls.Length);

            BallBase toSpawn = GameManager.staticBallDatabase.ReturnRandomBallInIndex(randomBallIndex);
            float randomX = UnityEngine.Random.Range((-spawnArea.localScale.x / 2),
                (spawnArea.localScale.x / 2));

            Vector3 newPos = center + new Vector3(randomX, 0, 0);

            BallBase go = Instantiate(toSpawn, newPos, Quaternion.identity);

            yield return new WaitForSeconds(spawnBallDelay);
        }
    }




    public void AddToList()
    {
        GameManager.instance.AddToPreludeAcitons(this);
    }

    public void RemoveFromList()
    {
        GameManager.instance.RemoveFromPreludeAcitons(this);
    }

    public void DoAction()
    {
        StartCoroutine(SpawnOnStart());
    }
}
