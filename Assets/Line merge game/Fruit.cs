using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] int fruitIndex;
    [SerializeField] int fruitScoreSpawn;
    [SerializeField] int fruitScoreMerge;
    int layerIndex;

    private void Awake()
    {
        layerIndex = gameObject.layer;

        ScoreManager.instance.AddScore(fruitScoreSpawn);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerIndex)
        {
            collision.gameObject.TryGetComponent<Fruit>(out Fruit otherFruit);

            if (!otherFruit) return;

            if (fruitIndex == otherFruit.ReturnFruitIndex())
            {
                int thisID = gameObject.GetInstanceID();
                int otherID = otherFruit.gameObject.GetInstanceID();

                if (thisID > otherID)
                {
                    if (fruitIndex < GameManager.instance.ReturnCurrentFruitDatabase().fruits.Length) //temp hardcoded
                    {
                        Vector3 MidPos = (transform.position + collision.transform.position) / 2;
                        Fruit newFruit = GetCombinedFruit();

                        if (newFruit)
                        {
                            Instantiate(newFruit.gameObject, MidPos, Quaternion.identity);
                            ScoreManager.instance.AddScore(fruitScoreMerge);

                            Destroy(collision.gameObject);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }
    private Fruit GetCombinedFruit()
    {
        Fruit combined = GameManager.instance.ReturnCurrentFruitDatabase().ReturnCombinedFruit(this);
        return combined;
    }

    public int ReturnFruitIndex()
    {
        return fruitIndex;
    }
    public int ReturnScore()
    {
        return fruitScoreSpawn;
    }
}
