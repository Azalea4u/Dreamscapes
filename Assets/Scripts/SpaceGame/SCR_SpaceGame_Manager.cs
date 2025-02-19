using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SCR_SpaceGame_Manager : MonoBehaviour
{
    public static SCR_SpaceGame_Manager instance { get; private set; }

	[SerializeField] private GameObject spaceShip_Object;
	[SerializeField] private SCR_SpaceGame_Ship spaceship;
    [SerializeField] private Obstacle[] obstaclesToSpawn;
    [SerializeField] private float spawnTimerLength = 2.0f;
	private float spawnTimer;
	[SerializeField] private int shipDistance;
	public float difficultyScale = 1.0f;

	[Header("Death State")]
	[SerializeField] public Animator shipAnimator;
	[SerializeField] private AudioSource explosion_SFX;
	[SerializeField] private GameObject gameOver_Panel;
    [SerializeField] private Button StartOver_BTN;
    [SerializeField] private Button Exit_BTN;

    [Serializable] struct Obstacle
	{
		public int startSpawningHeight;
		public int stopSpawningHeight; // if the stop heigh it 0, then it will always spawn
		public GameObject prefabToSpawn;
		public float[] spawnPositions;
		public float spawnTimeDelay;
	}

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI distance_TXT;
	[SerializeField] private AudioSource hitCloud_SFX;

    private void Start()
	{
        instance = this;
		spawnTimer = spawnTimerLength;
		gameOver_Panel.SetActive(false);
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
			difficultyScale = Mathf.Clamp(1.0f + (0.05f * (shipDistance / 100)), 1.0f, 2.0f);

			Obstacle tospawn = getObstacleToSpawn();

			Vector3 spawnPosition = new Vector3(tospawn.spawnPositions[UnityEngine.Random.Range(0, tospawn.spawnPositions.Length)], Camera.main.transform.position.y + 10.0f, 0.0f);
			Instantiate(tospawn.prefabToSpawn, spawnPosition, Quaternion.identity);

            spawnTimer = (spawnTimerLength / difficultyScale) + tospawn.spawnTimeDelay;
		}
	}

	Obstacle getObstacleToSpawn()
	{
		List<Obstacle> spawnables = new List<Obstacle>();

		foreach (var obstacle in obstaclesToSpawn)
		{
			if (obstacle.startSpawningHeight <= shipDistance &&
				(obstacle.stopSpawningHeight == 0 || obstacle.stopSpawningHeight > shipDistance))
			{
				spawnables.Add(obstacle);
			}
		}

		if (spawnables.Count == 0) {
			return obstaclesToSpawn[0];
		}

		return spawnables[UnityEngine.Random.Range(0, spawnables.Count)];
	}

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
		gameOver_Panel.SetActive(true);
        StartCoroutine(WaitBeforeInput());
    }

    private IEnumerator WaitBeforeInput()
    {
        yield return new WaitForSeconds(0.5f);
        StartOver_BTN.interactable = true;
        Exit_BTN.interactable = true;

        GameManager.instance.PauseGame(true);
    }

    private void UpdateDistanceText()
	{
        shipDistance = (int)spaceShip_Object.transform.position.y;
		distance_TXT.text = shipDistance.ToString();
    }
}
