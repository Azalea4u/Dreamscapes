using System.Collections.Generic;
using UnityEngine;

public class SCR_ArcheologyItem : MonoBehaviour
{
	[SerializeField] private SpriteRenderer visuals;
	public Vector2Int size { get { return itemData.size; } }
	public Vector2Int position = Vector2Int.zero;
	private SO_ArcheologyItem_Data itemData;

	/// <summary>
	/// Initialize item data from scriptable object
	/// </summary>
	/// <param name="data">The item's data</param>
	public void InitializeItemData(SO_ArcheologyItem_Data data)
	{
		itemData = data;
	}

	/// <summary>
	/// Sets up the item to be properly displayed within the grid
	/// </summary>
	/// <param name="tileScale">Scaling for tiles for modifying screen space usage</param>
	public void SetupItem(Vector2 tileScale)
	{
		visuals.sprite = itemData.sprite;
		visuals.transform.localScale = new Vector3(tileScale.x * size.x, tileScale.y * size.y, 1);
		visuals.transform.position += new Vector3(((float)size.x - 1) / (2 / tileScale.x), -((float)size.y - 1) / (2 / tileScale.y), 0);
	}

	/// <summary>
	/// A getter for all the grid positions that the item covers
	/// </summary>
	/// <returns>A list of every position in the grid the item takes up</returns>
	public List<Vector2Int> GetItemGridPositions() {
		List<Vector2Int> positions = new List<Vector2Int>();

		if (itemData.itemVolume.Count > 0)
		{
			for (int i = 0; i < itemData.itemVolume.Count; i++)
			{
				// the position of the item should be added to the returned positions
				// this correctly offsets the returned grid positions to the position of the item
				positions.Add(itemData.itemVolume[i] + position);
			}
		}
		else 
		{
			for (int iy = 0; iy < size.y; iy++)
			{
				for (int ix = 0; ix < size.x; ix++)
				{
					// same offset rule here as above
					positions.Add(new Vector2Int(ix + position.x, iy + position.y));
				}
			}
		}

		return positions;
	}

    public Sprite GetSprite()
    {
        return itemData.sprite;
    }

    public string GetItemName()
    {
		return itemData.Name;
    }

}
