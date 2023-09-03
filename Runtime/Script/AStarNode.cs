using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
	public class AStarNode
	{
		private readonly Vector2Int _position;
		
		private AStarNode _rootNode;
		
		/// <summary>
		/// 移動コスト
		/// </summary>
		public int MoveCost { get; }

		/// <summary>
		/// ここまで来るのに必要なコスト
		/// </summary>
		public int MoveTotalCost;
		
		/// <summary>
		/// ゴールまでの推定コスト
		/// </summary>
		private int _eCost;

		public int Score => MoveCost + _eCost + MoveTotalCost;

		public AStarNode(int cost, Vector2Int position)
		{
			MoveCost = cost;
			_position = position;
		}

		/// <summary>
		/// ゴールまでのコストを計算
		/// </summary>
		public void SetEstimateCost(Vector2Int position, Vector2Int goal, bool isDiagonally)
		{
			var dx = Mathf.Abs(position.x - goal.x);
			var dy = Mathf.Abs(position.y - goal.y);
			_eCost = isDiagonally ? Mathf.Max(dx, dy) : dx + dy;
		}
		
		public void Open(AStarNode rootNode)
		{
			_rootNode = rootNode;
			MoveTotalCost = 0;
			if (_rootNode == null)
				return;

			MoveTotalCost = _rootNode.MoveTotalCost + MoveCost;
		}

		public List<Vector2Int> ToList()
		{
			var list = new List<Vector2Int>();
			list.Insert(0, _position);

			var node = _rootNode;
			while (node != null)
			{
				list.Insert(0, node._position);
				node = node._rootNode;
			}

			return list;
		}
	}    
}