using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Ball Database", menuName = "ScriptableObjects/Create Ball Database", order = 1)]
public class BallDatabaseSO : ScriptableObject
{
    public Ball[] balls;
    public GameObject[] NonPhysicBalls;

    public Ball ReturnRandomBallInIndex(int maxIndex)
    {
        return balls[Random.Range(0, maxIndex + 1)];
    }
}