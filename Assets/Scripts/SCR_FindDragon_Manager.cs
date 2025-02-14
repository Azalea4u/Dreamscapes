using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_FindDragon_Manager : MonoBehaviour
{
    public static SCR_FindDragon_Manager instance { get; private set; }

    [SerializeField] private SCR_FindDragon_Dragon[] dragons;
    [SerializeField] private Sprite[] dragonSprites;
    [SerializeField] private Vector2 _gamebounds;
    [SerializeField] private Image wantedDragonVisual;
    [SerializeField] private List<Vector2> spawnGrid;
	[SerializeField] private List<Vector2> spawnGridAvailable;

    [SerializeField] private bool useTimer = false;
    [SerializeField] private float timeLeft = 15;
    [SerializeField] private int dragonsFound = 0;

    [Header("Game State")]
    [SerializeField] private GameObject WinScreen_Panel;
    [SerializeField] private Button StartOver_BTN;
    [SerializeField] private Button Exit_BTN;
    [SerializeField] private TextMeshProUGUI findDragon_TXT;
    [SerializeField] private TextMeshProUGUI Score_TXT;
    [SerializeField] private GameObject leaderBoard;
    
    // using a vector 4 because the bound values of the walls could all be different
    /// <summary>
    /// x = left bound
    /// y = top bound
    /// z = right bound
    /// w = bottom bound
    /// </summary>
    [SerializeField] public Vector4 gameBounds { 
        get { 
            return new Vector4(-_gamebounds.x + transform.position.x, _gamebounds.y + transform.position.y, _gamebounds.x + transform.position.x, -_gamebounds.y + transform.position.y); 
        } 
    }

    [SerializeField] private List<dragonGroup> dragonGroups = new List<dragonGroup>();

    public enum spawnPattern
    {
        RANDOM = 0,
        GRID = 1
    }

    // helper struct for organizing groups of dragons
    public struct dragonGroup
    {
        public Sprite sprite;
        public Vector2 speed;
        public bool copySpeed;
        public spawnPattern pattern;
        public SCR_FindDragon_Dragon.edgeType edgeType;
    }

	private void Start()
	{
		instance = this;

        WinScreen_Panel.SetActive(false);
        //leaderBoard.SetActive(false);

        // create the grid of spawn positions within the bounds of the game
        int gridDensity = 6;
		Vector4 bounds = gameBounds;
		for (int x = 1; x < gridDensity; x++)
        {
            for (int y = 1; y < gridDensity; y++)
            {
                spawnGrid.Add(new Vector2(Mathf.Lerp(bounds.x, bounds.z, (float)x / gridDensity),
                                          Mathf.Lerp(bounds.y, bounds.w, (float)y / gridDensity)));
			}
        }

        useTimer = true;

        resetGame();
	}

	private void Update()
	{
        if (useTimer)
        {
			timeLeft -= Time.deltaTime;
			if (timeLeft <= 0)
			{
				EndGame();
			}
		}

        Score_TXT.text = "Found \n" + dragonsFound;
	}

	private void resetGame()
    {
        spawnGridAvailable = new List<Vector2>(spawnGrid);

		createDragonGroups();
		setupDragons();
	}

    private void createDragonGroups()
    {
        dragonGroups.Clear();
        int start = Random.Range(0,dragonSprites.Length);
        for (int i = 0; i < 3; i++)
        {
			dragonGroup newgroup = new dragonGroup();
            newgroup.sprite = dragonSprites[(start + i)%dragonSprites.Length];
			newgroup.copySpeed = (Random.Range(0, 2)==0);
            // (Random.Range(0, 2) * 2 - 1) gets a random number of -1 or 1
            newgroup.speed = new Vector2((Random.Range(0, 2) * 2 - 1) * Random.Range(0.5f, 1.0f), (Random.Range(0, 2) * 2 - 1) * Random.Range(0.5f, 1.0f));
            newgroup.edgeType = (SCR_FindDragon_Dragon.edgeType)Random.Range(0, 2);
            newgroup.pattern = (spawnPattern)Random.Range(0, 2);

			dragonGroups.Add(newgroup);
		}
    }

	private void setupDragons()
    {
		Vector4 bounds = gameBounds;
		foreach (var dragon in dragons)
        {
            int selectedGroup = Random.Range(1, dragonGroups.Count);
            int gridpos = Random.Range(0, spawnGridAvailable.Count);

            switch (dragonGroups[selectedGroup].pattern)
            {
				case spawnPattern.RANDOM:
                    dragon.transform.position = new Vector3(Random.Range(bounds.x, bounds.z), Random.Range(bounds.y, bounds.w), 0);
					break;

				case spawnPattern.GRID:
					dragon.transform.position = (Vector3)spawnGridAvailable[gridpos];
					spawnGridAvailable.RemoveAt(gridpos);
					break;
            }

            assignDragonGroup(dragon, selectedGroup);
		}

        assignDragonGroup(dragons[0], 0);
        wantedDragonVisual.sprite = dragonGroups[0].sprite;
    }

    private void assignDragonGroup(SCR_FindDragon_Dragon dragon, int group)
    {
		dragonGroup usedgroup = dragonGroups[group];
		if (usedgroup.copySpeed || usedgroup.pattern == spawnPattern.GRID)
		{
			dragon.speed = usedgroup.speed;
		}
		else
		{
			Vector2 s = usedgroup.speed;
			dragon.speed = new Vector2(Random.Range(-s.x, s.x), Random.Range(-s.y, s.y));
		}
        dragon.SetWanted(group == 0);
		dragon.SetSprite(usedgroup.sprite);
		dragon.edgeInteraction = usedgroup.edgeType;
        dragon.active = true;
	}

    public void DragonPressed(bool isWanted, SCR_FindDragon_Dragon dragon)
    {
        if (isWanted)
        {
            foreach (var drag in dragons)
            {
				drag.speed = Vector2.zero;
				drag.active = false;
				if (drag != dragon)
                {
					drag.transform.position = Vector3.one * 10;
				}
            }

            // would put here stuff I want to happen after the dragon is found
            dragonsFound += 1;
            timeLeft += 1;
            useTimer = false;
			StartCoroutine(resetGameCoroutine());
        }
        else
        {
            dragon.active = false;
            dragon.speed = Vector2.zero;
            dragon.transform.position = Vector3.one * 10;
			//timeLeft -= 0.5f;
		}
    }

    IEnumerator resetGameCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        resetGame();
        useTimer = true;

	}

	private void EndGame()
    {
        useTimer = false;

        // Do whatever here to make the game end
        findDragon_TXT.text = "You found " + dragonsFound + "!";
        //WinScreen_Panel.SetActive(true);
        leaderBoard.SetActive(true);
        leaderBoard.GetComponent<Scr_LeaderBoard>().endGame(dragonsFound);
        StartOver_BTN.interactable = false;
        Exit_BTN.interactable = false;
        StartCoroutine(WaitBeforeInput());
    }

    private IEnumerator WaitBeforeInput()
    {
        yield return new WaitForSeconds(0.5f);
        StartOver_BTN.interactable = true;
        Exit_BTN.interactable = true;

        GameManager.instance.PauseGame(true);
    }


}
