using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GridCreator : MonoBehaviour
{
	private List<List<Vector2>> tilePositions = new List<List<Vector2>>();
    [SerializeField] private GameObject[] bubblePrefabs;

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
			for (var i = 0; i < 5; i++)
			{
				if (i % 2 == 0)
				{
					var row = new List<Vector2>(10);
					row.AddRange(Enumerable.Repeat(new Vector2(), 10));
					tilePositions.Add(row);
				}
				else
				{
					var row = new List<Vector2>(9);
					row.AddRange(Enumerable.Repeat(new Vector2(), 9));
					tilePositions.Add(row);
				}
			}

			for (var j = 0; j < 5; j++)
			{
				var selectedRow = tilePositions[j];
				for (var i = 0; i < selectedRow.Count; i++)
				{
					float rowOffset;
					if (j % 2 == 0)
						rowOffset = 0;
					else
						rowOffset = 0.5f;

					tilePositions[j][i] = new Vector2(i + rowOffset, -j);
				}
			}

			for (var j = 0; j < 5; j++)
			{
				var selectedRow = tilePositions[j];
				for (var i = 0; i < selectedRow.Count; i++)
				{
					int randomBubble = UnityEngine.Random.Range(0, bubblePrefabs.Length);
                    Instantiate(bubblePrefabs[randomBubble], tilePositions[j][i], Quaternion.identity);
				}
			}
		}
	}
}
