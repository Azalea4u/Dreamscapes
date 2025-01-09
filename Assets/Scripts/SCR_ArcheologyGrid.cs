using System.Collections.Generic;
using UnityEngine;

public class SCR_ArcheologyGrid : MonoBehaviour
{
    public static SCR_ArcheologyGrid Instance { get; private set; }

    [SerializeField] private Vector2Int gridSize = Vector2Int.one;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private List<SCR_ArcheologyTile> tileGrid = new List<SCR_ArcheologyTile>();
    [SerializeField] private GameObject canvas;
    

    void Start()
    {
		Instance = this;

		for (int ypos = 0; ypos < gridSize.y; ypos++)
        {
            for (int xpos = 0; xpos < gridSize.x; xpos++)
            {
                SCR_ArcheologyTile tile = Instantiate(tilePrefab, canvas.transform).GetComponent<SCR_ArcheologyTile>();
                tile.position = tileGrid.Count;
				//tile.depth = Random.Range(0, 7);
                tile.SetupVisuals();
				tileGrid.Add(tile);
                tile.transform.position = new Vector2(transform.position.x + xpos, transform.position.y - ypos);
            }
        }
    }

    public void HitGrid(int position)
    {
        tileGrid[position].depth -= 2;

		// Above Tile Check
		//Debug.Log(position - gridSize.y + " >= 0");
		if (position - gridSize.y >= 0)
        {
			tileGrid[position - gridSize.y].depth -= 1;
			tileGrid[position - gridSize.y].SetupVisuals();
		}

		// Below Tile Check
		//Debug.Log(position + gridSize.y + " < " + tileGrid.Count);
		if (position + gridSize.y < tileGrid.Count)
		{
			tileGrid[position + gridSize.y].depth -= 1;
			tileGrid[position + gridSize.y].SetupVisuals();
		}

		// Right Tile Check
		//Debug.Log(((position + 1) % gridSize.x) + " != 0");
        if ((position + 1) % gridSize.x != 0)
        {
			tileGrid[position + 1].depth -= 1;
			tileGrid[position + 1].SetupVisuals();
		}

		// Left Tile Check
		//Debug.Log(((position - 1) % gridSize.x) + " < " + (gridSize.x - 1));
		if ((position != 0) && (position - 1) % gridSize.x < gridSize.x - 1)
		{
			tileGrid[position - 1].depth -= 1;
			tileGrid[position - 1].SetupVisuals();
		}
	}
}
