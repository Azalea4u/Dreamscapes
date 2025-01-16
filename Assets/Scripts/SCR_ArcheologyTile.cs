using UnityEngine;

public class SCR_ArcheologyTile : MonoBehaviour
{
	[SerializeField] private SpriteRenderer visuals;
	public Vector2Int position = Vector2Int.zero;
	private int _layers = 6;
	public bool hasItem = false;
	/// <summary>
	/// How many layers are covering this tile
	/// </summary>
	public int layers { get { return _layers; } set { _layers = Mathf.Clamp(value,0,6); } }

	// testing visual setup for the tile
	public void ChangeSprite(Sprite newsprite)
	{
		visuals.sprite = newsprite;
	}
}
