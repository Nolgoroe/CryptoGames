using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct FruitCombos
{
    public Fruit originFruit;
    public Fruit combinedFruit;
}

[CreateAssetMenu(fileName = "Fruit Database", menuName = "ScriptableObjects/Create Fruit Database", order = 1)]
public class FruitDatabaseSO : ScriptableObject
{
    public Fruit[] fruits;
    public GameObject[] NonPhysicFruits;
    public FruitCombos[] fruitCombos;


    public Fruit ReturnCombinedFruit(Fruit inFruit)
    {
        FruitCombos combo = fruitCombos.Where(x => x.originFruit.ReturnFruitIndex() == inFruit.ReturnFruitIndex()).FirstOrDefault();

        return combo.combinedFruit;
    }
}
