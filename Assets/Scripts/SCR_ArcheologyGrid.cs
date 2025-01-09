using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SCR_ArcheologyGrid : MonoBehaviour
{
    public static SCR_ArcheologyGrid Instance { get; private set; }

	[Header("Grid Making")]
    [SerializeField] private Vector2Int gridSize = Vector2Int.one;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tilePrefab;
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] private List<Sprite> depthSprites = new List<Sprite>();

	[Header("Runtime Grid Values")]
    [SerializeField] private List<SCR_ArcheologyTile> tileGrid = new List<SCR_ArcheologyTile>();
	[SerializeField] private List<SCR_ArcheologyItem> items = new List<SCR_ArcheologyItem>();

    void Start()
    {
		Instance = this;

		for (int ypos = 0; ypos < gridSize.y; ypos++)
        {
            for (int xpos = 0; xpos < gridSize.x; xpos++)
            {
                SCR_ArcheologyTile tile = Instantiate(tilePrefab, canvas.transform).GetComponent<SCR_ArcheologyTile>();
                tile.position = tileGrid.Count;
				tile.depth = Random.Range(0, 7);
                tile.ChangeSprite(depthSprites[tile.depth]);
				tileGrid.Add(tile);
                tile.transform.position = new Vector2(transform.position.x + xpos, transform.position.y - ypos);
            }
        }

		for (int i = 0; i < 20; i++)
		{
			SCR_ArcheologyItem item = Instantiate(itemPrefab, canvas.transform).GetComponent<SCR_ArcheologyItem>();
			item.size = new Vector2Int(Random.Range(2,4), Random.Range(2, 4));
			item.position = generateItemPosition(item.size - Vector2Int.one);
			item.SetupItem();
			items.Add(item);
			item.transform.position = tileGrid[item.position].transform.position;
		}
	}

	private int generateItemPosition(Vector2Int size)
	{
		int pos = Random.Range(0, tileGrid.Count);
		gridPositionCheck check = checkGridPosition(pos, size);

		Debug.Log((check.up && check.down && check.left && check.right));
		while (!(check.up && check.down && check.left && check.right))
		{
			pos = Random.Range(0, tileGrid.Count);
			check = checkGridPosition(pos, size);
		}

		return pos;
	}

	/// <summary>
	/// Bools will be true if the position is available
	/// </summary>
	struct gridPositionCheck
	{
		public bool up;
		public bool down;
		public bool left;
		public bool right;
	}

	private gridPositionCheck checkGridPosition(int pos, Vector2Int size)
	{
		gridPositionCheck check = new gridPositionCheck();

		// Above Tile Check
		//Debug.Log(pos - gridSize.y + " >= 0");
		check.up = pos - gridSize.y >= 0;

		// Below Tile Check
		//Debug.Log((pos + size.y) + gridSize.y + " < " + tileGrid.Count);
		check.down = (pos + size.y) + gridSize.y < tileGrid.Count;

		// Right Tile Check
		//Debug.Log(((pos + size.x) + 1) % gridSize.x + " != 0");
		check.right = ((pos + size.x) + 1) % gridSize.x != 0;

		// Left Tile Check
		//Debug.Log(((pos - 1) % gridSize.x) + " < " + (gridSize.x - 1));
		check.left = (pos != 0) && (pos - 1) % gridSize.x < gridSize.x - 1;

		return check;
	}

	public void HitGrid(int position)
    {
		SCR_ArcheologyTile tile = tileGrid[position];
		gridPositionCheck check = checkGridPosition(position, Vector2Int.zero);

		tile.depth -= 2;
		tile.ChangeSprite(depthSprites[tile.depth]);

		if (check.up)
        {
			tile = tileGrid[position - gridSize.y];
			tile.depth -= 1;
			tile.ChangeSprite(depthSprites[tile.depth]);
		}

		if (check.down)
		{
			tile = tileGrid[position + gridSize.y];
			tile.depth -= 1;
			tile.ChangeSprite(depthSprites[tile.depth]);
		}

		if (check.left)
		{
			tile = tileGrid[(position - 1)];
			tile.depth -= 1;
			tile.ChangeSprite(depthSprites[tile.depth]);
		}

        if (check.right)
        {
			tile = tileGrid[(position + 1)];
			tile.depth -= 1;
			tile.ChangeSprite(depthSprites[tile.depth]);
		}

	}
}
