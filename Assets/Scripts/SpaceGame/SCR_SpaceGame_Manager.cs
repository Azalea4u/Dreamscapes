using System;
using UnityEngine;

public class SCR_SpaceGame_Manager : MonoBehaviour
{
    public static SCR_SpaceGame_Manager instance { get; private set; }

    [SerializeField] private obstacle[] obstaclesToSpawn;
    [SerializeField] private float spawnTimerLength = 2.0f;
	private float spawnTimer;

	[Serializable] struct obstacle
	{
		public SCR_SpaceshipDamager prefabToSpawn;
		public float[] spawnPositions; 
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
            spawnTimer = spawnTimerLength;

			obstacle tospawn = getObstacleToSpawn();

			Vector3 spawnPosition = new Vector3(tospawn.spawnPositions[UnityEngine.Random.Range(0, tospawn.spawnPositions.Length)], Camera.main.transform.position.y + 10.0f, 0.0f);
			Instantiate(tospawn.prefabToSpawn.gameObject, spawnPosition, Quaternion.identity);

			//SCR_SpaceshipDamager newob = Instantiate(obstaclePrefabs[Random.Range(0,obstaclePrefabs.Length)].gameObject).GetComponent<SCR_SpaceshipDamager>();
			//newob.transform.position = new Vector3(Random.Range(-shipBounds, shipBounds), Camera.main.transform.position.y + 7.0f, 0.0f);
			//foreach (var obstacle in obstaclePrefabs)
			//{
			//	SCR_SpaceshipDamager newob = Instantiate(obstacle.gameObject, transform).GetComponent<SCR_SpaceshipDamager>();
			//	newob.transform.position = new Vector3(Random.Range(-shipBounds, shipBounds), Camera.main.transform.position.y + 7.0f, 0.0f);
			//}
		}
	}

	obstacle getObstacleToSpawn()
	{
		return obstaclesToSpawn[0];
	}
}
