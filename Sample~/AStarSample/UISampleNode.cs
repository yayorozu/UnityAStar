using System;
using UnityEngine;
using UnityEngine.UI;

public class UISampleNode : MonoBehaviour
{
	[SerializeField]
	private Button _button;
	[SerializeField]
	private Text _text;
	[SerializeField]
	private Image _image;

	public Vector2Int Position { get; private set; }
	public int Cost { get; private set; }
	
	private Action<Vector2Int> _action;
	
	public void SetUp(Vector2Int position, Action<Vector2Int> action)
	{
		Position = position;
		_action = action;
		Cost = 1;
	}

	private void Awake()
	{
		_button.onClick.AddListener(() => _action.Invoke(Position));
	}

	public void SetText(string text)
	{
		_text.text = text;
	}
	
	public void SetColor(Color color)
	{
		_image.color = color;
	}
	
}
