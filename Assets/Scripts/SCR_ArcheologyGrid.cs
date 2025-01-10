using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UIElements;

public class SCR_ArcheologyGrid : MonoBehaviour
{
    public static SCR_ArcheologyGrid Instance { get; private set; }

	[Header("Grid Making")]
    [SerializeField] private Vector2Int gridSize = Vector2Int.one;
	[SerializeField] private int itemAmount = 3;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tilePrefab;
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private List<Sprite> depthSprites = new List<Sprite>();
	[SerializeField] private List<SO_ArcheologyItem_Data> itemsData = new List<SO_ArcheologyItem_Data>();

	[Header("Runtime Grid Values")]
   //[SerializeField] private List<SCR_ArcheologyTile> tileGrid = new List<SCR_ArcheologyTile>();
	[SerializeField] private List<SCR_ArcheologyItem> items = new List<SCR_ArcheologyItem>();
	[SerializeField] private int toolType;

	[SerializeField] private SCR_ArcheologyTile[,] tileGrid;

    void Start()
    {
		Instance = this;

		tileGrid = new SCR_ArcheologyTile[gridSize.x,gridSize.y];

		for (int ypos = 0; ypos < gridSize.y; ypos++)
        {
            for (int xpos = 0; xpos < gridSize.x; xpos++)
            {
                SCR_ArcheologyTile tile = Instantiate(tilePrefab, canvas.transform).GetComponent<SCR_ArcheologyTile>();
				tileGrid[xpos, ypos] = tile;
				//tileGrid.Add(tile);

                //tile.position = tileGrid.Count;
				tile.position.Set(xpos, ypos);
				tile.depth = Random.Range(2, 7);
                tile.ChangeSprite(depthSprites[tile.depth]);

				tile.transform.position = new Vector2(transform.position.x + xpos, transform.position.y - ypos);
            }
        }

		//Destroy(tileGrid[0, 0].gameObject);

		for (int i = 0; i < itemAmount; i++)
		{
			SCR_ArcheologyItem item = Instantiate(itemPrefab, canvas.transform).GetComponent<SCR_ArcheologyItem>();
			item.InitializeItemData(itemsData[Random.Range(0,itemsData.Count)]);
			//item.size = new Vector2Int(Random.Range(1,4), Random.Range(1, 4));
			item.position = generateRandomItemPosition(item);
			int cycles = gridSize.x * gridSize.y;
			while (itemOverlapCheck(item) && cycles > 0)
			{
				item.position.x += 1;
				if (item.position.x + (item.size.x-1) >= gridSize.x)
				{
					item.position.x = 0;
					item.position.y += 1;
					if (item.position.y + (item.size.y - 1) >= gridSize.y)
					{
						item.position.Set(0, 0);
					}
				}
				cycles--;
				if (cycles <= 0)
				{
					Destroy(item.gameObject);
				}
			}
			if (item == null)
			{
				continue;
			}
			item.SetupItem();
			items.Add(item);
			item.transform.position = tileGrid[item.position.x,item.position.y].transform.position;
		}
	}

	//private int vec2toGridPosition(Vector2Int pos)
	//{
	//	return pos.x + (gridSize.x * pos.y);
	//}

	private Vector2Int generateRandomItemPosition(SCR_ArcheologyItem item)
	{
		Vector2Int randpos = new Vector2Int(Random.Range(0, gridSize.x - (item.size.x - 1)), Random.Range(0, gridSize.y - (item.size.y - 1)));
		//Debug.Log(pos);
		//gridPositionCheck check = checkGridPosition(pos, size);

		//Debug.Log((check.up && check.down && check.left && check.right));
		//while (!(check.up && check.down && check.left && check.right))
		//{
		//	pos = Random.Range(0, tileGrid.Count);
		//	check = checkGridPosition(pos, size);
		//}

		return randpos;
	}

	/// <summary>
	/// Check for if an item is overlapping any others when generating the board
	/// </summary>
	/// <returns>Returns true if the item is overlapping another item, otherwise returns false</returns>
	private bool itemOverlapCheck(SCR_ArcheologyItem item)
	{
        foreach (var pos in item.GetItemVolumePositions())
        {
			//Debug.Log(item.name + " _ " + pos);
			if (tileGrid[pos.x,pos.y].hasItem)
			{
				//Debug.Log("Item overlapped another");
				return true;
			}
		}

		foreach (var pos in item.GetItemVolumePositions())
		{
			tileGrid[pos.x, pos.y].hasItem = true;
		}

		//Debug.Log("Item is not overlapping");
		return false;
	}

	/// <summary>
	/// Tiles will be null if the adjacent tile is off the grid
	/// </summary>
	struct gridPositionCheck
	{
		public SCR_ArcheologyTile upTile;
		public SCR_ArcheologyTile downTile;
		public SCR_ArcheologyTile leftTile;
		public SCR_ArcheologyTile rightTile;
	}

	private gridPositionCheck checkAdjacentGridTiles(Vector2Int pos, Vector2Int size)
	{
		gridPositionCheck check = new gridPositionCheck();

		// Above Tile Check
		if (pos.y > 0)
		{
			check.upTile = tileGrid[pos.x, pos.y - 1];
		}

		// Below Tile Check
		if (pos.y < gridSize.y - 1)
		{
			check.downTile = tileGrid[pos.x, pos.y + 1];
		}

		// Right Tile Check
		if (pos.x < gridSize.x - 1)
		{
			check.rightTile = tileGrid[pos.x + 1, pos.y];
		}

		// Left Tile Check
		if (pos.x > 0)
		{
			check.leftTile = tileGrid[pos.x - 1, pos.y];
		}

		return check;
	}

	public void ChangeTool(int tool)
	{
		// 0 = pick
		// 1 = hammer
		toolType = tool;
	}

	public void HitGrid(Vector2Int position)
    {
		SCR_ArcheologyTile tile = tileGrid[position.x, position.y];
		gridPositionCheck check = checkAdjacentGridTiles(position, Vector2Int.zero);

		tile.depth -= 2;
		tile.ChangeSprite(depthSprites[tile.depth]);

		switch (toolType)
		{
			default:
				if (check.upTile)
				{
					check.upTile.depth -= 1;
					check.upTile.ChangeSprite(depthSprites[check.upTile.depth]);
				}

				if (check.downTile)
				{
					check.downTile.depth -= 1;
					check.downTile.ChangeSprite(depthSprites[check.downTile.depth]);
				}

				if (check.leftTile)
				{
					check.leftTile.depth -= 1;
					check.leftTile.ChangeSprite(depthSprites[check.leftTile.depth]);
				}

				if (check.rightTile)
				{
					check.rightTile.depth -= 1;
					check.rightTile.ChangeSprite(depthSprites[check.rightTile.depth]);
				}
				break;
			case 1:
				if (check.upTile)
				{
					check.upTile.depth -= 2;
					check.upTile.ChangeSprite(depthSprites[check.upTile.depth]);
				}

				if (check.downTile)
				{
					check.downTile.depth -= 2;
					check.downTile.ChangeSprite(depthSprites[check.downTile.depth]);
				}

				if (check.leftTile)
				{
					check.leftTile.depth -= 2;
					check.leftTile.ChangeSprite(depthSprites[check.leftTile.depth]);
				}

				if (check.rightTile)
				{
					check.rightTile.depth -= 2;
					check.rightTile.ChangeSprite(depthSprites[check.rightTile.depth]);
				}

				if (check.upTile && check.rightTile)
				{
					tile = tileGrid[position.x + 1 , position.y - 1];
					tile.depth -= 1;
					tile.ChangeSprite(depthSprites[tile.depth]);
				}

				if (check.downTile && check.leftTile)
				{
					tile = tileGrid[position.x - 1, position.y + 1];
					tile.depth -= 1;
					tile.ChangeSprite(depthSprites[tile.depth]);
				}

				if (check.leftTile && check.upTile)
				{
					tile = tileGrid[position.x - 1, position.y - 1];
					tile.depth -= 1;
					tile.ChangeSprite(depthSprites[tile.depth]);
				}

				if (check.rightTile && check.downTile)
				{
					tile = tileGrid[position.x + 1, position.y + 1];
					tile.depth -= 1;
					tile.ChangeSprite(depthSprites[tile.depth]);
				}
				break;
		}

		checkItemsUncovered();
	}

	private void checkItemsUncovered()
	{
		items.RemoveAll(x => !x);
		foreach (var item in items)
		{
			List<Vector2Int> volume = item.GetItemVolumePositions();
			bool uncovered = true;
			foreach(var pos in volume)
			{
				if (tileGrid[pos.x, pos.y].depth > 0)
				{
					uncovered = false;
					break;
				}
			}
				
			if (uncovered)
			{
				// this is where the item would be marked as gotten.
				Destroy(item.gameObject);
			}
		}
	}
}
