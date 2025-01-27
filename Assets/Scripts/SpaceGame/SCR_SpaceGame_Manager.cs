using UnityEngine;

public class SCR_SpaceGame_Manager : MonoBehaviour
{
    public static SCR_SpaceGame_Manager instance { get; private set; }

    [SerializeField] private SpriteRenderer visuals;
    [SerializeField] private Sprite[] damageStates;
    [SerializeField] private int health = 3;
    [SerializeField] private float shipPosition;
    [SerializeField] private float shipBounds;
    [SerializeField] private float shipMovementSpeed;
    [SerializeField] private float shipAscentSpeed;
	[SerializeField] private float desiredAscentSpeed;
	[SerializeField] private float usedAscentSpeed = 0;
    [SerializeField] private SCR_SpaceshipDamager[] obstaclePrefabs;
    [SerializeField] private float spawnTimer = 2.0f;

	private void Start()
	{
        instance = this;
        desiredAscentSpeed = shipAscentSpeed;
        visuals.sprite = damageStates[health];
	}

	void FixedUpdate()
    {
        usedAscentSpeed = Mathf.Lerp(usedAscentSpeed, desiredAscentSpeed, 3.0f * Time.fixedDeltaTime);

        transform.position = new Vector3(shipPosition, transform.position.y + (Time.fixedDeltaTime * usedAscentSpeed), 0.0f);

        spawnTimer -= Time.fixedDeltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = 2.0f;
			SCR_SpaceshipDamager newob = Instantiate(obstaclePrefabs[Random.Range(0,obstaclePrefabs.Length)].gameObject).GetComponent<SCR_SpaceshipDamager>();
			newob.transform.position = new Vector3(Random.Range(-shipBounds, shipBounds), Camera.main.transform.position.y + 7.0f, 0.0f);
			//foreach (var obstacle in obstaclePrefabs)
			//{
			//	SCR_SpaceshipDamager newob = Instantiate(obstacle.gameObject, transform).GetComponent<SCR_SpaceshipDamager>();
			//	newob.transform.position = new Vector3(Random.Range(-shipBounds, shipBounds), Camera.main.transform.position.y + 7.0f, 0.0f);
			//}
		}
	}

    public void MoveLeft()
    {
        shipPosition -= shipMovementSpeed * Time.fixedDeltaTime;
        shipPosition = Mathf.Clamp(shipPosition, -shipBounds, shipBounds);
    }

	public void MoveRight()
	{
		shipPosition += shipMovementSpeed * Time.fixedDeltaTime;
        shipPosition = Mathf.Clamp(shipPosition, -shipBounds, shipBounds);
	}

    public void DamageShip()
    {
        health -= 1;
		if (health < 0)
		{
            // test stuff for now
            Debug.Log("Ship Has Died");
            health = 0;
		}
        visuals.sprite = damageStates[health];
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        // Debug.Log("Boombaby");
        SCR_SpaceshipDamager obstacle = collision.GetComponent< SCR_SpaceshipDamager>();

        if (obstacle != null)
        {
            desiredAscentSpeed *= 1.0f - obstacle.slowDownAscent;
            shipMovementSpeed *= 1.0f - obstacle.slowDownMovement;
            if (!obstacle.beenHit)
            {
                usedAscentSpeed -= obstacle.bounce;
                if (obstacle.doesDamage)
                {
                    DamageShip();
                }
            }
            obstacle.HitShip();
        }
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		//Debug.Log("Goodbye");
		SCR_SpaceshipDamager obstacle = collision.GetComponent<SCR_SpaceshipDamager>();

		if (obstacle != null)
		{
            desiredAscentSpeed /= 1.0f - obstacle.slowDownAscent;
			shipMovementSpeed /= 1.0f - obstacle.slowDownMovement;
		}
	}
}
