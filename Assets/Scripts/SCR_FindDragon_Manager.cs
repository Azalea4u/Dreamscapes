using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_FindDragon_Manager : MonoBehaviour
{
    public static SCR_FindDragon_Manager instance { get; private set; }

    [Header("Setup Values")]
    [SerializeField] private SCR_FindDragon_Dragon[] dragons;
    /// <summary>
    /// available selection of dragons to be found
    /// </summary>
    [SerializeField] private Sprite[] dragonSprites;
	/// <summary>
	/// how many dragon groups should be made when creating groups
	/// </summary>
	[SerializeField] private int groupAmount = 3;
    [SerializeField] private Vector2 _gamebounds;
    [SerializeField] private Image wantedDragonVisual;
    /// <summary>
    /// positions that dragons can spawn at within the bounds of the game
    /// </summary>
    private List<Vector2> spawnGrid;
    /// <summary>
    /// available positions to spawn dragons.
    /// </summary>
	private List<Vector2> spawnGridAvailable;

    [Header("Runtime values")]
    private bool useTimer = false;
    private float timeLeft = 15;
    [SerializeField] private int dragonsFound = 0;

    [Header("Game State")]
    [SerializeField] private GameObject WinScreen_Panel;
    [SerializeField] private Button StartOver_BTN;
    [SerializeField] private Button Exit_BTN;
    [SerializeField] private TextMeshProUGUI findDragon_TXT;
    [SerializeField] private TextMeshProUGUI Score_TXT;
    [SerializeField] private TextMeshProUGUI Timer_TXT;
    [SerializeField] private GameObject leaderBoard;
    [SerializeField] private AudioSource foundCharacter_SFX;
    [SerializeField] private AudioSource newRound_SFX;
    
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

    private List<dragonGroup> dragonGroups = new List<dragonGroup>();

    // helps to define how far in the game the player is
    // as the player finds more dragons, more dragons are spawned, up to the maximum
    public enum difficultyScale {
        FIRST,
		SCALING,
        DONE
    }

    // helps to define how a group of dragons should be spawned
    public enum spawnPattern {
        RANDOM = 0,
        GRID = 1
    }

    // helper struct for organizing groups of dragons
    public struct dragonGroup
    {
        public Sprite sprite;
        public Vector2 speed;
        // if randomzieSpeed is true, speed is used as a range for randomization
        public bool randomizeSpeed;
        public spawnPattern pattern;
        public SCR_FindDragon_Dragon.edgeType edgeType;
    }

	private void Start()
	{
		instance = this;

        WinScreen_Panel.SetActive(false);
        leaderBoard.SetActive(false);

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

		foreach (var drag in dragons)
		{
            drag.DeactivateDragon();
		}

		useTimer = true;

        resetGame();
	}

	private void Update()
	{
        if (useTimer)
        {
			timeLeft -= Time.deltaTime;
            Timer_TXT.text = "" + (int)timeLeft;
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

    /// <summary>
    /// Gets the current difficulty scale of the game.
    /// Difficulty is based on how far the player is in the game.
    /// </summary>
    /// <returns>Current difficultyScale of the game</returns>
    private difficultyScale getCurrentDifficulty()
    {
        if (dragonsFound == 0) {
			return difficultyScale.FIRST;
		} else if (dragonsFound < dragons.Length) {
			return difficultyScale.SCALING;
		} else {
            return difficultyScale.DONE;
        }
    }

    /// <summary>
    /// Creates new dragon groups to be used when setting up the dragons
    /// </summary>
    private void createDragonGroups()
    {
        dragonGroups.Clear();

        // the start and groupAmount variables are designed with larger amount of sprites to choose from in mind

        // when creating groups it starts at a random location in the list
        int start = Random.Range(0,dragonSprites.Length);

		for (int i = 0; i < Mathf.Min(groupAmount, dragonSprites.Length); i++)
        {
			dragonGroup newgroup = new dragonGroup();

            if (getCurrentDifficulty() == difficultyScale.FIRST)
            {
				// if it is the first round of finding, the pattern is set completely on its own
				// so we don't need any of the other values

				// (start + i) % dragonSprites.Length will wrap the count around the bounds of the list
				newgroup.sprite = dragonSprites[(start + i) % dragonSprites.Length];
            }
            else
            {
				newgroup.pattern = (spawnPattern)Random.Range(0, 2);
				newgroup.sprite = dragonSprites[(start + i) % dragonSprites.Length];
                // the grid pattern will not move
                if (newgroup.pattern != spawnPattern.GRID)
				{
				    newgroup.randomizeSpeed = Random.Range(0, 2) == 0;
    				// (Random.Range(0, 2) * 2 - 1) gets a random number of -1 or 1
					newgroup.speed = new Vector2((Random.Range(0, 2) * 2 - 1) * Random.Range(0.25f, 0.75f), (Random.Range(0, 2) * 2 - 1) * Random.Range(0.25f, 0.75f));
				}
				newgroup.edgeType = (SCR_FindDragon_Dragon.edgeType)Random.Range(0, 2);
			}

			dragonGroups.Add(newgroup);
		}
    }

	private void setupDragons()
    {
		Vector4 bounds = gameBounds;
		for (int i = 0; i < Mathf.Min(dragonsFound + 3, dragons.Length); i++)
        {
            SCR_FindDragon_Dragon dragon = dragons[i];
            int selectedGroup = Random.Range(1, dragonGroups.Count);
            int gridpos = Random.Range(0, spawnGridAvailable.Count);


            switch (getCurrentDifficulty())
            {
                case difficultyScale.FIRST:
					selectedGroup = i;

					switch (i)
					{
						case 0:
							dragon.transform.position = transform.position + new Vector3(0, -1.0f, 0);
							break;
						case 1:
							dragon.transform.position = transform.position + new Vector3(-1.0f, 0.5f, 0);
							break;
						case 2:
							dragon.transform.position = transform.position + new Vector3(1.0f, 0.5f, 0);
							break;
					}
					break;
                case difficultyScale.SCALING:
                    // currently is the same as difficultyScale.DONE
                    // I would like to make it different if I have time
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
					break;
                case difficultyScale.DONE:
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
					break;
            }

            assignDragonGroup(dragon, selectedGroup);
		}

        assignDragonGroup(dragons[0], 0);
        wantedDragonVisual.sprite = dragonGroups[0].sprite;
    }

    private void assignDragonGroup(SCR_FindDragon_Dragon dragon, int group) {
		dragonGroup usedgroup = dragonGroups[group];
		if (!usedgroup.randomizeSpeed || usedgroup.pattern == spawnPattern.GRID) {
			dragon.speed = usedgroup.speed;
		} else {
			Vector2 s = usedgroup.speed;
			dragon.speed = new Vector2(Random.Range(-s.x, s.x), Random.Range(-s.y, s.y));
		}
        dragon.SetWanted(group == 0);
		dragon.SetSprite(usedgroup.sprite);
		dragon.edgeInteraction = usedgroup.edgeType;
        dragon.ActivateDragon();
	}

    public void DragonPressed(bool isWanted, SCR_FindDragon_Dragon dragon)
    {
        dragon.DeactivateDragon();
        if (isWanted) {
            foreach (var drag in dragons) {
                drag.DeactivateDragon();
            }
            foundCharacter_SFX.Play();
            dragonsFound += 1;
            timeLeft += 1;
            useTimer = false;
            StartCoroutine(resetGameCoroutine());
        } else {
            timeLeft -= 0.5f;
        }
    }

    IEnumerator resetGameCoroutine() {
        yield return new WaitForSeconds(2.0f);
        newRound_SFX.Play();
        resetGame();
        useTimer = true;
	}

	private void EndGame() {
        useTimer = false;
        findDragon_TXT.text = "You found " + dragonsFound + "!";
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

        GameManager.instance.timesinceTouched = 0;
    }
}