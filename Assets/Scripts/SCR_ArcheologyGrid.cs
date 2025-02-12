using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SCR_ArcheologyGrid : MonoBehaviour
{
    public static SCR_ArcheologyGrid Instance { get; private set; }

	[Header("Prefabs")]
    [SerializeField] private Transform tilePrefab;
	[SerializeField] private Transform undertile;
	[SerializeField] private Transform itemPrefab;
	[SerializeField] private List<Sprite> depthSprites = new List<Sprite>();
	[SerializeField] private List<SO_ArcheologyItem_Data> itemsData = new List<SO_ArcheologyItem_Data>();

	[Header("Grid Making")]
	[SerializeField] private SCR_ArcheologyTile[,] tileGrid;
	[SerializeField] private List<Vector2Int> highestSpots = new List<Vector2Int>();
    [SerializeField] private Vector2Int gridSize = Vector2Int.one;
	[SerializeField] private Vector2 tileSize = Vector2Int.one;
	[SerializeField] private int itemAmount = 3;

	[Header("Runtime Grid Values")]
	[SerializeField] private List<SCR_ArcheologyItem> items = new List<SCR_ArcheologyItem>();
	[SerializeField] private Transform player;
	[SerializeField] private Vector2Int playerPosition;
	[SerializeField] private int points;
	[SerializeField] private TextMeshProUGUI TextScore;
	[SerializeField] private float timeValue;
	[SerializeField] private TextMeshProUGUI TextTimer;
	private bool running = true;

	[Header("Audio")]
	[SerializeField] private AudioSource hitStoneSFX;
	[SerializeField] private AudioSource collectSFX;

	[Header("Pop-Up")]
	[SerializeField] private GameObject Popup_Panel;
	[SerializeField] private GameObject TurnOffPopup_BTN;
	[SerializeField] private Image Artifact_IMG;
	[SerializeField] private TextMeshProUGUI ArtifactName_TXT;

	[Header("Game State")]
	[SerializeField] private GameObject GameWin_Panel;
	[SerializeField] private GameObject GameTimeOut_Panel;

    void Start()
    {
		Instance = this;
		Popup_Panel.SetActive(false);
        GameWin_Panel.SetActive(false);

        tileGrid = new SCR_ArcheologyTile[gridSize.x,gridSize.y];

		highestSpots.Add(new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)));
		highestSpots.Add(new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)));
		highestSpots.Add(new Vector2Int(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)));

		// puts the grid in the middle of the screen
		transform.position -= new Vector3(gridSize.x / (2.0f / tileSize.x), -gridSize.y / (2.0f / tileSize.y), 0.0f) - new Vector3(tileSize.x / 2.0f, -tileSize.y / 2.0f, 0);

		// instantiate and fill out the grid tiles with tile prefabs
		for (int ypos = 0; ypos < gridSize.y; ypos++)
        {
            for (int xpos = 0; xpos < gridSize.x; xpos++)
            {
				SCR_ArcheologyTile tile = Instantiate(tilePrefab, transform).GetComponent<SCR_ArcheologyTile>();
				tileGrid[xpos, ypos] = tile;
				tile.position.Set(xpos, ypos);
				tile.layers = Random.Range(1, 7); //GetTileDepth( xpos, ypos); //

				tile.ChangeSprite(depthSprites[tile.layers]);

				tile.transform.localScale = tileSize;
				tile.transform.position = new Vector2(transform.position.x + (tileSize.x * xpos), transform.position.y - (tileSize.y * ypos));
            }
        }

		// create items and place them in the grid
		for (int i = 0; i < itemAmount; i++)
		{
			SCR_ArcheologyItem item = Instantiate(itemPrefab, transform).GetComponent<SCR_ArcheologyItem>();

			// item data is initialized first for the use of size and volume when positioning the item
			item.InitializeItemData(itemsData[Random.Range(0,itemsData.Count)]);

			item.position = generateRandomItemPosition(item);

			// if the item position generated overlaps with another, generate another one
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

			// item setup and list additioons should only happen after a viable spot is found
			item.SetupItem(tileSize);
			items.Add(item);
			item.transform.position = tileGrid[item.position.x,item.position.y].transform.position;
		}

		playerPosition = gridSize / 2;
		updatePlayer();

	}
	void Update()
	{
		if (running)
		{
			timeValue -= Time.deltaTime;
			TextTimer.text = "Timer: " + (int)timeValue;

			if (timeValue <= 0)
			{
				running = false;
				ShowGameTimeOutScreen();
			}
		}
    }

	private int GetTileDepth(int x, int y) {
		Vector2Int usedSpot = highestSpots[0];
		foreach (var spot in highestSpots)
		{
			if (Vector2Int.Distance(new Vector2Int(x, y), spot) < Vector2Int.Distance(new Vector2Int(x, y), usedSpot))
			{
				usedSpot = spot;
			}
		}

		int toret = (int)Vector2Int.Distance(new Vector2Int(x, y), usedSpot);

		return 7 - Mathf.Clamp(toret, 0, 7);
	}

	private Vector2Int generateRandomItemPosition(SCR_ArcheologyItem item)
	{
		return new Vector2Int(Random.Range(0, gridSize.x - (item.size.x - 1)), Random.Range(0, gridSize.y - (item.size.y - 1)));
	}

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

    #region MOVEMENT_CONTROLS
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
    #endregion

    /// <summary>
    /// Update position of player visuals to be in line with player position
    /// </summary>
    private void updatePlayer()
	{
		player.transform.position = tileGrid[playerPosition.x, playerPosition.y].transform.position;
	}

	/// <summary>
	/// Function for the mining UI button
	/// </summary>
	public void HitTile()
	{
		Vector2Int hitPos = playerPosition;

		// Changes the pitch in SFX
        float minPitch = -0.15f;
		float maxPitch = 0.5f;
        hitStoneSFX.pitch = Random.Range(minPitch, maxPitch);
		hitStoneSFX.Play();

		// Vectors for which tiles to check about the hit position
		// z value is how much tile damage it should deal
		Vector3Int[] checks = {
			new Vector3Int(0,0,2),
			new Vector3Int(1,0,1),
			new Vector3Int(0,1,1),
			new Vector3Int(-1,0,1),
			new Vector3Int(0,-1,1)
		};
		SCR_ArcheologyTile tile;

		for (int i = 0; i < checks.Length; i++)
		{
			// if the checked hit position is within the grid's bounds
			if (hitPos.x + checks[i].x < gridSize.x && hitPos.y + checks[i].y < gridSize.y && hitPos.x + checks[i].x >= 0 && hitPos.y + checks[i].y >= 0)
			{
				tile = tileGrid[hitPos.x + checks[i].x, hitPos.y + checks[i].y];
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

	private void checkItemsUncovered()
	{
		// removes null items from a list, because right now they are destroyed
		items.RemoveAll(x => !x);

		foreach (var item in items)
		{
			bool uncovered = true;
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
                // Something would happen to the item once it is gotten

                RemoveItem(item);
            }
		}
	}

    private void RemoveItem(SCR_ArcheologyItem item)
    {		
		// Pop-Up Panel
		running = false;
        Artifact_IMG.sprite = item.GetSprite();
		ArtifactName_TXT.text = item.GetItemName() + "!";
		Popup_Panel.SetActive(true);
        collectSFX.Play(); 

		TurnOffPopup_BTN.SetActive(false);
		StartCoroutine(StopInput());

		points += item.GetPointValue();
		TextScore.text = "Score: " + (int)points;
		items.Remove(item);
        Destroy(item.gameObject);

		
	}

	private IEnumerator StopInput()
	{
		yield return new WaitForSeconds(0.5f);
		Debug.Log("Can now remove popup");
		TurnOffPopup_BTN.SetActive(true);
        GameManager.instance.PauseGame(true);
    }

    public void ClosePopUp()
	{
        // Remove Items
        Popup_Panel.SetActive(false);
        // check for if all the items are gone
        if (items.Count == 0)
		{
			// Do the win screen stuff
			running = false;
			ShowGameWinScreen();
		}
		running = true;
		GameManager.instance.PauseGame(false);
    }

    private void ShowGameWinScreen()
    {
        GameWin_Panel.SetActive(true);
        GameManager.instance.PauseGame(true);
		SRC_AudioManager.instance.GameWon_SFX();
    }

    private void ShowGameTimeOutScreen()
    {
        GameTimeOut_Panel.SetActive(true);
        GameManager.instance.PauseGame(true);
    }
}
