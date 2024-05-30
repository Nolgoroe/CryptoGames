using System.Collections;
using UnityEngine;

public class BallsOnPrelude : MonoBehaviour, IPrelude
{
    [Header("Needed References")]
    [SerializeField] Transform spawnArea;
    [SerializeField] GameManager gameManager;

    [Header("Spawn Parameters")]
    [SerializeField] int minBalls = 15;
    [SerializeField] int maxBalls = 25;
    [SerializeField] float spawnBallDelay = 0.05f;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>(); //forces aLL preludes to be on game manager.. bad? FLAG
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
            int randomBallIndex = UnityEngine.Random.Range(0, 0);

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
        if (gameManager)
            gameManager.AddToPreludeAcitons(this);
    }

    public void RemoveFromList()
    {
        if (gameManager)
            gameManager.RemoveFromPreludeAcitons(this);
    }

    public void DoAction()
    {
        StartCoroutine(SpawnOnStart());
    }
}
