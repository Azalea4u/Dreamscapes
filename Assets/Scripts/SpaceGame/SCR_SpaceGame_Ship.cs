using UnityEngine;

public class SCR_SpaceGame_Ship : MonoBehaviour
{
	[Header("Ship Visual Stuff")]
	[SerializeField] private SpriteRenderer visuals;
	[SerializeField] private SpriteRenderer withinCloudsVisuals;
	[SerializeField] private Sprite[] damageStates;
	[SerializeField] private int health = 3;
	[SerializeField] private GameObject[] birdVisuals;
	[SerializeField] private int birds = 0;
	[SerializeField] private float birdTimerLength = 5.0f;
    [SerializeField] private AudioSource hitCloud_SFX;
	[SerializeField] private AudioSource collideBird_SFX;
	[SerializeField] private AudioSource collideBird2_SFX;

    private float birdTimer;

	[Header("Ship Movement")]
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
		withinCloudsVisuals.sprite = damageStates[health];
		foreach (var bird in birdVisuals)
        {
			bird.SetActive(false);
        }
    }

    // FIXED UPDATE STAYS AS FIXED UPDATE
	// PREVENTS SHIP FROM JITTERING!!!!
	// MAKE SURE YOU REMEMBER THAT, MAX!!!
	// ok - max
    void FixedUpdate()
    {
        SCR_SpaceGame_Manager.instance.shipAnimator.SetInteger("Health", health);

        if (health <= 0)
		{
			return;
		}

		if (birds > 0)
		{
			birdTimer -= Time.fixedDeltaTime;
			if (birdTimer <= 0.0f)
			{
				birdTimer = birdTimerLength;
				RemoveBird();
			}
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

		usedAscentSpeed = Mathf.Lerp(usedAscentSpeed, (1.0f - (((float)birds/3.0f) * 0.5f)) * desiredAscentSpeed * SCR_SpaceGame_Manager.instance.difficultyScale, 3.0f * Time.fixedDeltaTime);

		shipVelocity = Mathf.Lerp(shipVelocity, 0.0f, Time.fixedDeltaTime * 5.0f);
		shipVelocity = Mathf.Clamp(shipVelocity, -shipMovementSpeed * 0.9f, shipMovementSpeed * 0.9f);

		shipPosition += shipVelocity * Time.fixedDeltaTime;
		shipPosition = Mathf.Clamp(shipPosition, -shipBounds, shipBounds);

		visuals.transform.rotation = Quaternion.Euler(0,0,shipVelocity/shipMovementSpeed * -45.0f);

		transform.position = new Vector3(shipPosition, transform.position.y + (Time.fixedDeltaTime * usedAscentSpeed), 0.0f);

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
        collideBird2_SFX.Play();
        health -= 1;
		if (health <= 0)
		{
			health = 0;
			ShipDeath();
			withinCloudsVisuals.gameObject.SetActive(false);
			foreach(var bird in birdVisuals)
			{
				bird.SetActive(false);
			}
		}
		withinCloudsVisuals.sprite = damageStates[health];
		visuals.sprite = damageStates[health];
	}

	public void AddBird()
	{
		collideBird_SFX.Play();
		birds = Mathf.Clamp(birds + 1,0,3);
		birdVisuals[birds - 1].SetActive(true);
		if (birds == 3)
		{
			DamageShip();		
		}
	}

	public void RemoveBird()
	{
		birdVisuals[birds - 1].SetActive(false);
		birds = Mathf.Clamp(birds - 1, 0, 3);
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


            if (!obstacle.birdSlowdown && !obstacle.doesDamage)
            {
                hitCloud_SFX.Play();
            }

            if (!obstacle.beenHit)
			{
				usedAscentSpeed -= obstacle.bounce;
				shipVelocity *= -1.0f;
				if (obstacle.birdSlowdown)
				{
					birdTimer = birdTimerLength;
					AddBird();
				}
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
