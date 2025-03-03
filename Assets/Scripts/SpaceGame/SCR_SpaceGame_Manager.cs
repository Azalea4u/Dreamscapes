using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SpaceGame_Manager : MonoBehaviour
{
    public static SCR_SpaceGame_Manager instance { get; private set; }

	[SerializeField] private GameObject spaceShip_Object;
	[SerializeField] private SCR_SpaceGame_Ship spaceship;
    [SerializeField] private Obstacle[] obstaclesToSpawn;
    [SerializeField] private float spawnTimerLength = 2.0f;
	[SerializeField] private int shipDistance;
	public float difficultyScale = 1.0f;
	private float spawnTimer;
	// the index of obstaclesToSpawn[] that was spawned last so you don't get a bunch of repeats
	private int prevSpawnIndex;

	[Header("Death State")]
	[SerializeField] public Animator shipAnimator;
	[SerializeField] private AudioSource explosion_SFX;
	[SerializeField] private GameObject gameOver_Panel;
	[SerializeField] private GameObject leaderboard;
    [SerializeField] private Button StartOver_BTN;
    [SerializeField] private Button Exit_BTN;

    [Serializable] struct Obstacle
	{
		public int startSpawningHeight;
		public int stopSpawningHeight; // if the stop height is 0, then the obstacle will always be able to spawn
		public GameObject prefabToSpawn;
		public float[] spawnPositions;
		public float spawnTimeDelay;
	}

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI distance_TXT;

    private void Start()
	{
        instance = this;
		spawnTimer = spawnTimerLength;
		gameOver_Panel.SetActive(false);
		leaderboard.SetActive(false);
	}

	void FixedUpdate()
    {
        if (spaceShip_Object == null)
        {
			return;
        }

		UpdateDistanceText();

        spawnTimer -= Time.fixedDeltaTime;
        if (spawnTimer <= 0)
        {
			// difficulty is updated when an entity spawns because I don't want things to change mid obstacle encounter.
			difficultyScale = Mathf.Clamp(1.0f + (0.05f * (shipDistance / 100)), 1.0f, 2.0f);

			Obstacle tospawn = getObstacleToSpawn();

			// tospawn.prefabToSpawn is only null when the gotten obstacle is the same as the previously spawned one
			if (tospawn.prefabToSpawn == null)
			{
				return;
			}

			int spawnIndex = UnityEngine.Random.Range(0, tospawn.spawnPositions.Length);

			Vector3 spawnPosition = new Vector3(tospawn.spawnPositions[spawnIndex], Camera.main.transform.position.y + 10.0f, 0.0f);
			Instantiate(tospawn.prefabToSpawn, spawnPosition, Quaternion.identity);

            spawnTimer = (spawnTimerLength / difficultyScale) + tospawn.spawnTimeDelay;
		}
	}

	/// <summary>
	/// Gets an obstacle from the obstaclesToSpawn list that is valid to spawn at the spaceships current height
	/// </summary>
	/// <returns>A spawnable obstacle<br/>Will return an empty obstacle if there are no valid spawns</returns>
	Obstacle getObstacleToSpawn()
	{
		List<int> spawnables = new List<int>();

		for (int i = 0; i < obstaclesToSpawn.Length; i++)
		{
			var obstacle = obstaclesToSpawn[i];

			if (obstacle.startSpawningHeight <= shipDistance &&
				(obstacle.stopSpawningHeight == 0 || obstacle.stopSpawningHeight > shipDistance))
			{
				spawnables.Add(i);
			}
		}

		if (spawnables.Count == 0) {
			return new Obstacle();
		}

		int tospawn = UnityEngine.Random.Range(0, spawnables.Count);
		if (tospawn == prevSpawnIndex)
		{
			return new Obstacle();
		}
		prevSpawnIndex = tospawn;

		return obstaclesToSpawn[spawnables[tospawn]];
	}

	/// <summary>
	///	Getter function for the spaceship MonoBehaviour
	/// </summary>
	/// <returns>The spaceship MonoBehaviour</returns>
	public SCR_SpaceGame_Ship GetSpaceship()
	{
		return spaceship;
	}

	public void ShipHasDied()
	{
		// handle the ship death and minigame ending here
		shipAnimator.Play("Explosion");
		explosion_SFX.Play();
		StartCoroutine(ShowGameOverScreen());
    }

    private IEnumerator ShowGameOverScreen()
	{
        yield return new WaitForSeconds(0.75f);
        Destroy(spaceShip_Object.gameObject);
		yield return new WaitForSeconds(0.5f);
		//gameOver_Panel.SetActive(true);
		leaderboard.SetActive(true);
		leaderboard.GetComponent<Scr_LeaderBoard>().endGame(shipDistance);
		StartCoroutine(WaitBeforeInput());
    }

    private IEnumerator WaitBeforeInput()
    {
        yield return new WaitForSeconds(0.5f);
        StartOver_BTN.interactable = true;
        Exit_BTN.interactable = true;
    }

    private void UpdateDistanceText()
	{
        shipDistance = (int)spaceShip_Object.transform.position.y;
		distance_TXT.text = shipDistance.ToString();
    }
}