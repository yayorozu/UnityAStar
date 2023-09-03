using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStarSample : MonoBehaviour
{
	[SerializeField]
	private Button _startButton;
	[SerializeField]
	private Vector2Int _size;
	[SerializeField]
	private GameObject _nodeObj;
	[SerializeField]
	private GridLayoutGroup _gridLayout;
	
	private Dictionary<Vector2Int, UISampleNode> _nodes;

	private Vector2Int _start;
	private Vector2Int _goal;
	private AStar.AStar _instance;

	private void Awake()
	{
		_gridLayout.constraintCount = _size.x;
		_nodes = new Dictionary<Vector2Int, UISampleNode>();
		for (int x = 0; x < _size.x; x++)
		{
			for (int y = 0; y < _size.y; y++)
			{
				var obj = GameObject.Instantiate(_nodeObj, transform);
				var node = obj.GetComponent<UISampleNode>();
				var pos = new Vector2Int(x, y);
				node.SetUp(pos, ClickNode);
				_nodes.Add(pos, node);
			}
		}
		
		_startButton.onClick.AddListener(StartSimulate);
		
		int[,] map = new int[_size.x, _size.y];
		foreach (var pair in _nodes)
		{
			map[pair.Key.x, pair.Key.y] = pair.Value.Cost;
		}
		
		_instance = AStar.AStar.Instance(map);
		
		_start = _instance.GetRandomPosition(); 
		_goal = _instance.GetRandomPosition();
		_nodes[_start].SetText("S");
		_nodes[_goal].SetText("G");
		
		_nodeObj.SetActive(false);
	}

	private void ClickNode(Vector2Int pos)
	{
		
	}
	
	private void StartSimulate()
	{
		var route = _instance.Calc(_start, _goal);
		foreach (var pos in route)
		{
			_nodes[pos].SetColor(Color.red);
		}
	}
}
