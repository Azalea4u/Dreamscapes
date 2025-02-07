using UnityEngine;

public class SCR_SpaceGame_Ship : MonoBehaviour
{
	[SerializeField] private SpriteRenderer visuals;
	[SerializeField] private Sprite[] damageStates;
	[SerializeField] private int health = 3;
	private float shipPosition;
	[SerializeField] private float shipBounds;
	[SerializeField] private float shipAscentSpeed;
	[SerializeField] private float shipMovementSpeed;
	private float desiredAscentSpeed;
	private float usedAscentSpeed = 0;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		desiredAscentSpeed = shipAscentSpeed;
		visuals.sprite = damageStates[health];
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		usedAscentSpeed = Mathf.Lerp(usedAscentSpeed, desiredAscentSpeed, 3.0f * Time.fixedDeltaTime);

		transform.position = new Vector3(shipPosition, transform.position.y + (Time.fixedDeltaTime * usedAscentSpeed), 0.0f);
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
		if (health <= 0)
		{
			health = 0;
			SCR_SpaceGame_Manager.instance.ShipHasDied();
		}
		visuals.sprite = damageStates[health];
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Debug.Log("Boombaby");
		SCR_SpaceshipDamager obstacle = collision.GetComponent<SCR_SpaceshipDamager>();

		if (obstacle != null)
		{
			desiredAscentSpeed *= 1.0f - obstacle.slowDownMovement;
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
			desiredAscentSpeed /= 1.0f - obstacle.slowDownMovement;
			shipMovementSpeed /= 1.0f - obstacle.slowDownMovement;
		}
	}
}
