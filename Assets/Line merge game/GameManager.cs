using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool canControlGame;

    [SerializeField] Transform leftBound, rightBound;
    [SerializeField] float boundOffset = 1;
    [SerializeField] FruitDatabaseSO fruitDatabase;
    [SerializeField] GameObject currentNonPhysDisplay;
    [SerializeField] Fruit currentFruit;

    float leftBoundX, rightBoundX;

    private void Awake()
    {
        canControlGame = true;
        instance = this;

        leftBoundX = leftBound.transform.position.x + boundOffset;
        rightBoundX = rightBound.transform.position.x - boundOffset;
    }
    private void Start()
    {
        DecideNextFruit();
    }

    void Update()
    {
        if (!canControlGame) return;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        
        Vector3 newPos = new Vector3(mouse.x, transform.position.y, transform.position.z);
        newPos.x = Mathf.Clamp(newPos.x, leftBoundX, rightBoundX);

        transform.position = newPos;


        if(Input.GetMouseButtonDown(0))
        {
            int randomNum = Random.Range(0, fruitDatabase.fruits.Length);

            Destroy(currentNonPhysDisplay.gameObject);
            Instantiate(currentFruit.gameObject, transform.position, Quaternion.identity);
            currentFruit = null;


            DecideNextFruit();
        }
    }

    private void DecideNextFruit()
    {
        int randomNum = Random.Range(0, 2);

        currentFruit = fruitDatabase.fruits[randomNum];
        currentNonPhysDisplay = Instantiate(fruitDatabase.NonPhysicFruits[randomNum],transform);

    }

    public FruitDatabaseSO ReturnCurrentFruitDatabase()
    {
        return fruitDatabase;
    }

    public void GameOver()
    {
        canControlGame = false;

        Destroy(gameObject);
        currentFruit = null;
    }
}
