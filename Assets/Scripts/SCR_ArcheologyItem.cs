using System.Collections.Generic;
using UnityEngine;

public class SCR_ArcheologyItem : MonoBehaviour
{
	[SerializeField] private SpriteRenderer sprite;
	public Vector2Int size = Vector2Int.one;
	public Vector2Int position = Vector2Int.zero;

	public void SetupItem()
	{
		sprite.transform.localScale = new Vector3(size.x, size.y, 1);
		sprite.transform.position += new Vector3(((float)size.x - 1) / 2, -((float)size.y - 1) / 2, 0);
	}

	/// <summary>
	/// A getter for the positions that the item covers
	/// </summary>
	/// <returns>A list of every position in the grid the item takes up</returns>
	public List<Vector2Int> GetItemVolumePositions() {
		List<Vector2Int> positions = new List<Vector2Int>();

		for (int iy = 0; iy < size.y; iy++)
		{
			for (int ix = 0; ix < size.x; ix++)
			{
				positions.Add(new Vector2Int(ix + position.x, iy + position.y));
			}
		}

		return positions;
	}

}
