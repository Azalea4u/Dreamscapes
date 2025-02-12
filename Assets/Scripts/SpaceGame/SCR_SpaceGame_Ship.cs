using UnityEngine;

public class SCR_SpaceGame_Ship : MonoBehaviour
{
	[SerializeField] private SpriteRenderer visuals;
	[SerializeField] private Sprite[] damageStates;
	[SerializeField] private int health = 3;
	[SerializeField] private float shipPosition;
	[SerializeField] private float shipVelocity;
	[SerializeField] private float shipBounds;
	[SerializeField] private float shipAscentSpeed;
	[SerializeField] private float shipMovementSpeed;
	private float desiredAscentSpeed;
	[SerializeField]private float usedAscentSpeed = 0;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		desiredAscentSpeed = shipAscentSpeed;
		visuals.sprite = damageStates[health];
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		if (health <= 0)
		{
			return;
		}

		for (int i = 0; i < Input.touchCount; i++)
		{
			Vector3 touchpos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
			if (touchpos.x < 0)
			{
				MoveLeft();
			} else
			{
				MoveRight();
			}
		}

		usedAscentSpeed = Mathf.Lerp(usedAscentSpeed, desiredAscentSpeed * SCR_SpaceGame_Manager.instance.difficultyScale, 3.0f * Time.fixedDeltaTime);

		shipVelocity = Mathf.Lerp(shipVelocity, 0.0f, Time.deltaTime * 5.0f);
		shipVelocity = Mathf.Clamp(shipVelocity, -shipMovementSpeed * 0.9f, shipMovementSpeed * 0.9f);

		shipPosition += shipVelocity * Time.deltaTime;
		shipPosition = Mathf.Clamp(shipPosition, -shipBounds, shipBounds);

		visuals.transform.rotation = Quaternion.Euler(0,0,shipVelocity/shipMovementSpeed * -45.0f);

		transform.position = new Vector3(shipPosition, transform.position.y + (Time.fixedDeltaTime * usedAscentSpeed), 0.0f);

		SCR_SpaceGame_Manager.instance.shipAnimator.SetInteger("Health", health);
	}

	public void MoveLeft()
	{
		shipVelocity -= shipMovementSpeed * Time.deltaTime * 5.0f;
	}

	public void MoveRight()
	{
		shipVelocity += shipMovementSpeed * Time.deltaTime * 5.0f;
	}

	public void DamageShip()
	{
		health -= 1;
		if (health <= 0)
		{
			health = 0;
		}
		visuals.sprite = damageStates[health];
	}

	public void ShipDeath()
	{
		visuals.transform.rotation = Quaternion.identity;
		SCR_SpaceGame_Manager.instance.ShipHasDied();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		SCR_SpaceshipDamager obstacle = collision.GetComponent<SCR_SpaceshipDamager>();

		if (obstacle != null)
		{
			desiredAscentSpeed *= 1.0f - obstacle.slowDownMovement;
			shipMovementSpeed *= 1.0f - obstacle.slowDownMovement;
			if (!obstacle.beenHit)
			{
				usedAscentSpeed -= obstacle.bounce;
				shipVelocity *= -1.0f;
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
		SCR_SpaceshipDamager obstacle = collision.GetComponent<SCR_SpaceshipDamager>();

		if (obstacle != null)
		{
			desiredAscentSpeed /= 1.0f - obstacle.slowDownMovement;
			shipMovementSpeed /= 1.0f - obstacle.slowDownMovement;
		}
	}
}
