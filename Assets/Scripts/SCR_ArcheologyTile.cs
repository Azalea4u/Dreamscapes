using UnityEngine;

public class SCR_ArcheologyTile : MonoBehaviour
{
	[SerializeField] private SpriteRenderer visuals;
	public Vector2Int position = Vector2Int.zero;
	public bool hasItem = false;

	private int _layers = 6;
	/// <summary>
	/// How many layers of stuff are covering this tile
	/// </summary>
	public int layers { get { return _layers; } set { _layers = Mathf.Clamp(value,0,6); } }

	// this function is used by the grid because I don't want every instantiated tile to have a reference to every sprite it can be
	/// <summary>
	/// Changes the sprite of the tile to the given one
	/// </summary>
	/// <param name="newsprite">The new sprite</param>
	public void ChangeSprite(Sprite newsprite)
	{
		visuals.sprite = newsprite;

		// if there are now layers, move the tile below everything so the items are fully visible
		if (layers == 0)
		{
			visuals.sortingOrder = -2;
		}
	}
}
