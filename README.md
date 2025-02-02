# UnityAStar

UnityAStar は、Unity プロジェクト向けの軽量な A* パスファインディングライブラリです。  

<img src="https://cdn-ak.f.st-hatena.com/images/fotolife/h/hacchi_man/20200827/20200827005808.gif" width="300">

# サンプルコード: UnityAStar の基本的な使い方

以下は、A* アルゴリズムを利用してグリッド上でパスを計算し、UI 上に経路を表示するサンプルコードの必要な部分です。

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStarSample : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;             // シミュレーション開始ボタン
    [SerializeField]
    private Vector2Int _size;                // グリッドのサイズ (例: 横×縦)
    [SerializeField]
    private GameObject _nodeObj;             // ノード表示用のプレハブ
    [SerializeField]
    private GridLayoutGroup _gridLayout;     // UI のグリッドレイアウト

    private Dictionary<Vector2Int, UISampleNode> _nodes; // 座標とノードのマッピング
    private Vector2Int _start;               // 開始位置
    private Vector2Int _goal;                // 目標位置
    private AStar.AStar _instance;           // A* アルゴリズムのインスタンス

    private void Awake()
    {
        // グリッドレイアウトの列数を設定
        _gridLayout.constraintCount = _size.x;
        _nodes = new Dictionary<Vector2Int, UISampleNode>();

        // グリッド内の各ノードを生成して初期化
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                var obj = Instantiate(_nodeObj, transform);
                var node = obj.GetComponent<UISampleNode>();
                var pos = new Vector2Int(x, y);
                node.SetUp(pos, ClickNode);
                _nodes.Add(pos, node);
            }
        }
        
        // シミュレーション開始ボタンにリスナーを登録
        _startButton.onClick.AddListener(StartSimulate);

        // グリッド上の各ノードのコスト情報を 2 次元配列に変換
        int[,] map = new int[_size.x, _size.y];
        foreach (var pair in _nodes)
        {
            map[pair.Key.x, pair.Key.y] = pair.Value.Cost;
        }
        
        // A* インスタンスを生成
        _instance = AStar.AStar.Instance(map);
        
        // ランダムな開始位置と目標位置を設定
        _start = _instance.GetRandomPosition(); 
        _goal = _instance.GetRandomPosition();
        _nodes[_start].SetText("S");
        _nodes[_goal].SetText("G");
        
        // プレハブはシーン上に表示しない
        _nodeObj.SetActive(false);
    }

    // ノードがクリックされた際の処理（必要に応じて実装）
    private void ClickNode(Vector2Int pos)
    {
        // 例: ユーザーがノードをクリックした場合の処理をここに記述
    }
    
    // シミュレーション開始: パスを計算して経路上のノードの色を変更
    private void StartSimulate()
    {
        var route = _instance.Calc(_start, _goal);
        foreach (var pos in route)
        {
            _nodes[pos].SetColor(Color.red);
        }
    }
}
```
## 解説:

Awake メソッド内で、グリッドの各セルに対応するノードを生成し、コスト情報をもとに A* のグリッドを構築します。
ランダムな開始位置と目標位置を設定し、それぞれ "S" (Start) と "G" (Goal) として表示します。
シミュレーション開始ボタンをクリックすると、StartSimulate メソッドが呼ばれ、計算された経路上のノードの色を赤に変更します。
このサンプルコードを参考に、UnityAStar ライブラリを利用したパスファインディング機能の実装が可能です。

# ライセンス

本プロジェクトは [MIT License](LICENSE) の下でライセンスされています。  
詳細については、LICENSE ファイルをご覧ください。
