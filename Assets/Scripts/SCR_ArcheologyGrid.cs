using System.Collections.Generic;
using UnityEngine;

public class SCR_ArcheologyGrid : MonoBehaviour
{
    public static SCR_ArcheologyGrid Instance { get; private set; }

	[Header("Prefabs")]
    [SerializeField] private Transform tilePrefab;
	[SerializeField] private Transform itemPrefab;
	[SerializeField] private List<Sprite> depthSprites = new List<Sprite>();
	[SerializeField] private List<SO_ArcheologyItem_Data> itemsData = new List<SO_ArcheologyItem_Data>();

	[Header("Grid Making")]
	[SerializeField] private SCR_ArcheologyTile[,] tileGrid;
    [SerializeField] private Vector2Int gridSize = Vector2Int.one;
	[SerializeField] private Vector2 tileSize = Vector2Int.one;
	[SerializeField] private int itemAmount = 3;


	[Header("Runtime Grid Values")]
	[SerializeField] private List<SCR_ArcheologyItem> items = new List<SCR_ArcheologyItem>();
	[SerializeField] private Transform player;
	[SerializeField] private Vector2Int playerPosition;


    void Start()
    {
		Instance = this;

		tileGrid = new SCR_ArcheologyTile[gridSize.x,gridSize.y];

		transform.position -= new Vector3(gridSize.x / (2.0f / tileSize.x), -gridSize.y / (2.0f / tileSize.y), 0.0f) - new Vector3(tileSize.x / 2.0f, -tileSize.y / 2.0f, 0);

		// instantiate and fill out the grid tiles
		for (int ypos = 0; ypos < gridSize.y; ypos++)
        {
            for (int xpos = 0; xpos < gridSize.x; xpos++)
            {
                SCR_ArcheologyTile tile = Instantiate(tilePrefab, transform).GetComponent<SCR_ArcheologyTile>();
				tileGrid[xpos, ypos] = tile;
				tile.position.Set(xpos, ypos);
				tile.layers = Random.Range(1, 7);
                tile.ChangeSprite(depthSprites[tile.layers]);

				tile.transform.localScale = tileSize;
				tile.transform.position = new Vector2(transform.position.x + (tileSize.x * xpos), transform.position.y - (tileSize.y * ypos));
            }
        }

		// create items and place them in the grid
		for (int i = 0; i < itemAmount; i++)
		{
			SCR_ArcheologyItem item = Instantiate(itemPrefab, transform).GetComponent<SCR_ArcheologyItem>();

			item.InitializeItemData(itemsData[Random.Range(0,itemsData.Count)]);

			item.position = generateRandomItemPosition(item);
			int cycles = gridSize.x * gridSize.y;
			while (itemOverlapCheck(item) && cycles > 0)
			{
				item.position = generateRandomItemPosition(item);
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
			item.SetupItem(tileSize);
			items.Add(item);
			item.transform.position = tileGrid[item.position.x,item.position.y].transform.position;
		}

		playerPosition = gridSize / 2;
		updatePlayer();
	}

	/// <summary>
	/// using the given item's size and position, generates a random value that will fit within the grid
	/// </summary>
	/// <param name="item"></param>
	/// <returns>Position within the grid</returns>
	private Vector2Int generateRandomItemPosition(SCR_ArcheologyItem item)
	{
		return new Vector2Int(Random.Range(0, gridSize.x - (item.size.x - 1)), Random.Range(0, gridSize.y - (item.size.y - 1)));
	}

	/// <summary>
	/// Check for if a given item is overlapping any others
	/// If it is not overlapping any, the tiles it covers will be marked as having an item.
	/// </summary>
	/// <returns>Returns true if the item is overlapping another item, otherwise returns false</returns>
	private bool itemOverlapCheck(SCR_ArcheologyItem item)
	{
		List<Vector2Int> itemVolume = item.GetItemGridPositions();

		foreach (var pos in itemVolume)
        {
			if (tileGrid[pos.x,pos.y].hasItem)
			{
				return true;
			}
		}

		foreach (var pos in itemVolume)
		{
			tileGrid[pos.x, pos.y].hasItem = true;
		}

		return false;
	}

	/// <summary>
	/// UI button function for moving the player up
	/// </summary>
	public void MoveUp()
	{
		playerPosition.y -= 1;
		if (playerPosition.y < 0)
		{
			playerPosition.y = gridSize.y - 1;
		}
		updatePlayer();
	}

	/// <summary>
	/// UI button function for moving the player down
	/// </summary>
	public void MoveDown()
	{
		playerPosition.y += 1;
		if (playerPosition.y >= gridSize.y)
		{
			playerPosition.y = 0;
		}
		updatePlayer();
	}

	/// <summary>
	/// UI button function for moving the player left
	/// </summary>
	public void MoveLeft()
	{
		playerPosition.x -= 1;
		if (playerPosition.x < 0)
		{
			playerPosition.x = gridSize.x - 1;
		}
		updatePlayer();
	}

	/// <summary>
	/// UI button function for moving the player right
	/// </summary>
	public void MoveRight()
	{
		playerPosition.x += 1;
		if (playerPosition.x >= gridSize.x)
		{
			playerPosition.x = 0;
		}
		updatePlayer();
	}

	private void updatePlayer()
	{
		player.transform.position = tileGrid[playerPosition.x, playerPosition.y].transform.position;
	}

	/// <summary>
	/// Function for the mining UI button
	/// </summary>
	public void HitTile()
	{
		hitGrid(playerPosition);
	}

	private void hitGrid(Vector2Int position)
    {
		// Vectors for which tiles to check about the position pressed
		// z value is how much tile damage it should deal
		Vector3Int[] checks = { 
			new Vector3Int(0,0,2),
			new Vector3Int(1,0,1), 
			new Vector3Int(0,1,1),
			new Vector3Int(-1,0,1),
			new Vector3Int(0,-1,1)
		};
		SCR_ArcheologyTile tile;

		for (int i = 0; i < checks.Length; i++) {
			if (position.x + checks[i].x < gridSize.x && position.y + checks[i].y < gridSize.y && position.x + checks[i].x >= 0 && position.y + checks[i].y >= 0)
			{
				tile = tileGrid[position.x + checks[i].x, position.y + checks[i].y];
				if (tile != null)
				{
					tile.layers -= checks[i].z;
					tile.ChangeSprite(depthSprites[tile.layers]);
				}
			}
		}

		// after breaking tiles, check for uncovered items
		checkItemsUncovered();
	}

	/// <summary>
	/// Check items for if items in the grid are uncovered
	/// removes items that are fully uncovered
	/// </summary>
	private void checkItemsUncovered()
	{
		// removes all null items in the items list
		items.RemoveAll(x => !x);

		// loop through all items on the grid
		foreach (var item in items)
		{
			bool uncovered = true;
			// loop through all the grid positions the item covers
			foreach(var pos in item.GetItemGridPositions())
			{
				if (tileGrid[pos.x, pos.y].layers > 0)
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
