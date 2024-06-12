using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseNextBallPower : PowerupBase
{
    [Header("Needed references")]
    [SerializeField] private Spawner player;

    [Header("Ball data")]
    [SerializeField] private int amountBallsToChooseFrom = 5;

    [Header("Screen")]
    [SerializeField] private GameObject screenPrefab;
    [SerializeField] private Transform screenParent;

    [Header("Buttons")]
    [SerializeField] private Button ballButtonPrefab;
    [SerializeField] private Transform ballButtonsParent;


    GameObject spawnedScreen;
    public override void UsePower()
    {
        // show UI screen.
        spawnedScreen = Instantiate(screenPrefab, screenParent);
        ballButtonsParent = spawnedScreen.GetComponentInChildren<GridLayoutGroup>().transform;

        // block user controls
        GameManager.gameIsControllable = false;

        // spawn buttons in ui screen under layout group - by loop of X in the ball SO data list
        for (int i = 0; i < amountBallsToChooseFrom; i++)
        {
            Button button = Instantiate(ballButtonPrefab, ballButtonsParent);
            button.image.sprite = GameManager.staticBallDatabase.balls[i].ReturnBallSpriteRenderer().sprite;


            // bank the current int in memory or else the last i num will always be the number chosen
            // VARIABLE capture - not VALUE capture - so we create a new variable to capture each iteration.
            int currentIndex = i;

            // add on click event to those buttons that return an int
            button.onClick.AddListener(() => ChooseBallAction(currentIndex));
        }
    }

    private void ChooseBallAction(int ballIndex)
    {
        Debug.Log(ballIndex);

        BallBase ball = GameManager.staticBallDatabase.balls[ballIndex];
        player.SwapBalls(ball);

        localResetData();
    }
    protected override void localResetData()
    {
        GameManager.gameIsControllable = true;

        Destroy(spawnedScreen.gameObject);
        ResetPowerUsage();
    }
}
