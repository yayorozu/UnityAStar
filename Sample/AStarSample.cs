using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
	    int[,] map = new int[5, 5];
	    for (int x = 0; x < 5; x++)
	    {
		    for (int y = 0; y < 5; y++)
		    {
			    map[x, y] = Random.Range(1, 4);
		    }
	    }
	    var a = AStar.AStar.Instance(map);
	    var start = a.GetRandomPosition();
	    var goal = a.GetRandomPosition();
	    a.Calc(start, goal);
    }

}
