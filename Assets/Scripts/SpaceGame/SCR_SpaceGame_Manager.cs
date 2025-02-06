using System;
using System.Collections.Generic;
using UnityEngine;

public class SCR_SpaceGame_Manager : MonoBehaviour
{
    public static SCR_SpaceGame_Manager instance { get; private set; }

	[SerializeField] private SCR_SpaceGame_Ship spaceship;
    [SerializeField] private Obstacle[] obstaclesToSpawn;
    [SerializeField] private float spawnTimerLength = 2.0f;
	private float spawnTimer;

	[Serializable] struct Obstacle
	{
		public float startSpawningHeight;
		public float stopSpawningHeight; // if the stop heigh it 0, then it will always spawn
		public GameObject prefabToSpawn;
		public float[] spawnPositions;
		public float spawnTimeDelay;
	}

	private void Start()
	{
        instance = this;
		spawnTimer = spawnTimerLength;
	}

	void FixedUpdate()
    {


        spawnTimer -= Time.fixedDeltaTime;
        if (spawnTimer <= 0)
        {

			Obstacle tospawn = getObstacleToSpawn();

			Vector3 spawnPosition = new Vector3(tospawn.spawnPositions[UnityEngine.Random.Range(0, tospawn.spawnPositions.Length)], Camera.main.transform.position.y + 10.0f, 0.0f);
			Instantiate(tospawn.prefabToSpawn, spawnPosition, Quaternion.identity);

            spawnTimer = spawnTimerLength + tospawn.spawnTimeDelay;
			//SCR_SpaceshipDamager newob = Instantiate(obstaclePrefabs[Random.Range(0,obstaclePrefabs.Length)].gameObject).GetComponent<SCR_SpaceshipDamager>();
			//newob.transform.position = new Vector3(Random.Range(-shipBounds, shipBounds), Camera.main.transform.position.y + 7.0f, 0.0f);
			//foreach (var obstacle in obstaclePrefabs)
			//{
			//	SCR_SpaceshipDamager newob = Instantiate(obstacle.gameObject, transform).GetComponent<SCR_SpaceshipDamager>();
			//	newob.transform.position = new Vector3(Random.Range(-shipBounds, shipBounds), Camera.main.transform.position.y + 7.0f, 0.0f);
			//}
		}
	}

	Obstacle getObstacleToSpawn()
	{
		List<Obstacle> spawnables = new List<Obstacle>();

		foreach (var obstacle in obstaclesToSpawn)
		{
			if (obstacle.startSpawningHeight <= spaceship.transform.position.y &&
				(obstacle.stopSpawningHeight == 0 || obstacle.stopSpawningHeight > spaceship.transform.position.y))
			{
				spawnables.Add(obstacle);
			}
		}

		if (spawnables.Count == 0) {
			return obstaclesToSpawn[0];
		}

		return spawnables[UnityEngine.Random.Range(0, spawnables.Count)];
	}
}
