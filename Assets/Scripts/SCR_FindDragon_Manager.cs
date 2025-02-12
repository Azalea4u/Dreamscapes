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

    [SerializeField] private bool useTimer = false;
    [SerializeField] private float timeLeft = 15;
    [SerializeField] private int dragonsFound = 0;

    [Header("Game State")]
    [SerializeField] private GameObject WinScreen_Panel;
    [SerializeField] private TextMeshProUGUI findDragon_TXT;
    [SerializeField] private GameObject LeaderBoardUI;
    [SerializeField] public TextMeshProUGUI Timer_TXT;
    
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
    
    //[SerializeField] private int difficulty;

    [SerializeField] private List<dragonGroup> dragonGroups = new List<dragonGroup>();

    // helper struct for organizing groups of dragons
    public struct dragonGroup
    {
        public Sprite sprite;
        public Vector2 speed;
        public bool copySpeed;
        public SCR_FindDragon_Dragon.edgeType edgeType;
    }

	private void Start()
	{
		instance = this;

        WinScreen_Panel.SetActive(false);
        LeaderBoardUI.SetActive(false);

        createDragonGroups();
		setupDragons();
        useTimer = true;
	}

	private void Update()
	{
        if (useTimer)
        {
			timeLeft -= Time.deltaTime;
            Timer_TXT.text = "Timer: " + (int)timeLeft;
			if (timeLeft <= 0)
			{
				EndGame();
			}
		}
	}
	private void resetGame()
    {
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
            dragonGroups.Add(newgroup);
		}
    }

	private void setupDragons()
    {
        foreach (var dragon in dragons)
        {
            dragon.transform.position = new Vector3(Random.Range(gameBounds.x, gameBounds.z), Random.Range(gameBounds.y, gameBounds.w), 0);
            assignDragonGroup(dragon, Random.Range(1, dragonGroups.Count));
		}

        assignDragonGroup(dragons[dragons.Length - 1], 0);
        wantedDragonVisual.sprite = dragonGroups[0].sprite;
    }

    private void assignDragonGroup(SCR_FindDragon_Dragon dragon, int group)
    {
		dragonGroup usedgroup = dragonGroups[group];
		if (usedgroup.copySpeed)
		{
			dragon.speed = usedgroup.speed;
		}
		else
		{
			Vector2 s = usedgroup.speed;
			dragon.speed = new Vector2(Random.Range(-s.x, s.x), Random.Range(-s.y, s.y));
		}
        dragon.isWanted = (group == 0);
		dragon.SetSprite(usedgroup.sprite);
		dragon.edgeInteraction = usedgroup.edgeType;
	}

    public void DragonPressed(bool isWanted, SCR_FindDragon_Dragon dragon)
    {

        if (isWanted)
        {
            foreach (var drag in dragons)
            {
				drag.speed = Vector2.zero;
				if (drag != dragon)
                {
					drag.transform.position = Vector3.one * 10;
				}
            }

            // would put here stuff I want to happen after the dragon is found
            dragonsFound += 1;
            timeLeft += 2;
            useTimer = false;
			StartCoroutine(resetGameCoroutine());
        }
        else
        {
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
        // Do whatever here to make the game end
        useTimer = false;
        findDragon_TXT.text = "You found " + dragonsFound + "!";
        WinScreen_Panel.SetActive(true);

        //leaderboard stuff
        LeaderBoardUI.SetActive(true);
        LeaderBoardUI.GetComponent<Scr_LeaderBoard>().endGame(dragonsFound);
    }
}