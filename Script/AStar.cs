using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AStar
{
	public class AStar
	{
		public static AStar Instance(int[,] map)
		{
			return new AStar(map);
		}

		/// <summary>
		/// コストマップ
		/// </summary>
		private Dictionary<Vector2Int, AStarNode> _map;
		private HashSet<Vector2Int> _openHash;
		private HashSet<Vector2Int> _closeHash;
		
		private AStar(int[,] map)
		{
			SetMap(map);
		}

		private void SetMap(int[,] map)
		{
			var length = map.GetLength(0) * map.GetLength(1);
			_openHash = new HashSet<Vector2Int>();
			_closeHash = new HashSet<Vector2Int>();
			_map = new Dictionary<Vector2Int, AStarNode>(length);
			for (var x = 0; x < map.GetLength(0); x++)
			{
				for (var y = 0; y < map.GetLength(1); y++)
				{
					if (map[x, y] <= 0)
						continue;
					
					_map.Add(new Vector2Int(x, y), new AStarNode(map[x, y], new Vector2Int(x, y)));
				}
			}
		}

		/// <summary>
		/// マップデータの更新
		/// </summary>
		public void UpdateMap(int[,] map)
		{
			SetMap(map);
		}

		/// <summary>
		/// 経路計算
		/// </summary>
		public List<Vector2Int> Calc(Vector2Int start, Vector2Int goal, bool isDiagonally = false)
		{
			Debug.LogError(start);
			Debug.LogError(goal);
			foreach (var pair in _map)
				pair.Value.SetEstimateCost(pair.Key, goal, isDiagonally);
			
			_openHash.Clear();
			_closeHash.Clear();

			var current= start;
			_openHash.Add(current);
			_map[current].Open(null);
			var count = 100000;
			while (count-- > 0)
			{
				if (_openHash.Count <= 0)
					throw new Exception("Open Node Empty");

				current = _openHash.OrderBy(p => _map[p].Score)
					.OrderBy(p => _map[p].MoveCost)
					.First();
				
				if (current == goal)
					break;
				
				_openHash.Remove(current);
				_closeHash.Add(current);
				
				OpenAround(current, isDiagonally);
			}

			if (count <= 0)
				throw new Exception("Calc Fail");
			
			return _map[current].ToList();
		}

		/// <summary>
		/// 周りを開ける
		/// </summary>
		private void OpenAround(Vector2Int current, bool isDiagonally)
		{
			var openPos = new List<Vector2Int>
			{
				new Vector2Int(current.x + 1, current.y),
				new Vector2Int(current.x - 1, current.y),
				new Vector2Int(current.x, current.y + 1),
				new Vector2Int(current.x, current.y - 1),
			};
			if (isDiagonally)
			{
				openPos.Add(new Vector2Int(current.x + 1, current.y + 1));
				openPos.Add(new Vector2Int(current.x + 1, current.y - 1));
				openPos.Add(new Vector2Int(current.x - 1, current.y + 1));
				openPos.Add(new Vector2Int(current.x - 1, current.y - 1));
			}

			foreach (var pos in openPos)
			{
				if (!_map.ContainsKey(pos))
					continue;
				
				if (_openHash.Contains(pos))
				{
					if (_map[current].Score <= _map[pos].Score + _map[current].MoveTotalCost)
						continue;
					
					_openHash.Remove(pos);
				}

				if (_closeHash.Contains(pos))
				{
					if (_map[current].Score <= _map[pos].Score + _map[current].MoveTotalCost)
						continue;
					
					_closeHash.Remove(pos);
				}

				_openHash.Add(pos);
				_map[pos].Open(_map[current]);
			}
		}
		

		/// <summary>
		/// ランダムに座標を取得
		/// </summary>
		/// <returns></returns>
		public Vector2Int GetRandomPosition()
		{
			var keys = _map.Keys.ToList();
			if (keys.Count <= 0)
				return Vector2Int.zero;

			return keys.OrderBy(k => Random.Range(0, 100000)).First();
		}
	}
}
