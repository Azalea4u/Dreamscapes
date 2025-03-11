using UnityEngine;

public class SCR_SpaceGame_Ship : MonoBehaviour
{
	[Header("Ship Visual Stuff")]
	[SerializeField] private SpriteRenderer visuals;
	[SerializeField] private SpriteRenderer withinCloudsVisuals;
	[SerializeField] private Sprite[] damageStates;
	[SerializeField] private GameObject[] birdVisuals;
    [SerializeField] private AudioSource hitCloud_SFX;
	[SerializeField] private AudioSource collideBird_SFX;
	[SerializeField] private AudioSource collideBird2_SFX;

	[Header("Runtime Values")]
	[SerializeField] private int health = 3;
	[SerializeField] private int birds = 0;
	// used for shaking birds off if you move left and right rapidly
	private bool shipMoveLeft = false;
	[SerializeField] private float birdTimerLength = 5.0f;
    private float birdTimer;

	[Header("Ship Movement")]
	// left and right position of the ship
	[SerializeField] private float shipPosition;
	// How far left or right the ship can move
	[SerializeField] private float shipBounds;
	// ship's velocity left and right. because it feels better to slow down than stop on a dime
	[SerializeField] private float shipVelocity;
	// How fast the ship moves Horizontally
	[SerializeField] private float shipMovementSpeed;
	// How fast the ship moves Vertically
	[SerializeField] private float shipAscentSpeed;
	// the speed that gets modified by hitting obstacles
	private float desiredAscentSpeed;
	// The used speed moves towards the desired speed
	private float usedAscentSpeed = 0;



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

		// The ship itself checks for touch input instead of getting input from buttons because It makes it feel more consistant
		for (int i = 0; i < Input.touchCount; i++)
		{
			bool prevLeft = shipMoveLeft;
			Vector3 touchpos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
			
			if (touchpos.x < 0)
			{
				MoveLeft();
			} else
			{
				MoveRight();
			}

			if (birdTimer > 0)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began && prevLeft != shipMoveLeft)
				{
					birdTimer -= 0.15f;
				}
			}
		}

		// bird slowdown calculates to be between 1.0 and 0.5 based on amount of birds
		float birdslowdown = 1.0f - ((float)birds / 3.0f * 0.5f);
		usedAscentSpeed = Mathf.Lerp(usedAscentSpeed, birdslowdown * desiredAscentSpeed * SCR_SpaceGame_Manager.instance.difficultyScale, 3.0f * Time.fixedDeltaTime);

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

		shipMoveLeft = true;
	}

	public void MoveRight()
	{
		shipVelocity += shipMovementSpeed * Time.deltaTime * 5.0f;

		shipMoveLeft = false;
	}

	/// <summary>
	/// Damage function for the ship
	/// </summary>
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

	/// <summary>
	/// Adds a Bird to the ship
	/// </summary>
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

	/// <summary>
	/// Removes a Bird from the ship
	/// </summary>
	public void RemoveBird()
	{
		birdVisuals[birds - 1].SetActive(false);
		birds = Mathf.Clamp(birds - 1, 0, 3);
	}

	/// <summary>
	/// Ship death function
	/// </summary>
	public void ShipDeath()
	{
		visuals.transform.rotation = Quaternion.identity;
		SCR_SpaceGame_Manager.instance.ShipHasDied();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		SCR_SpaceshipDamager obstacle = collision.GetComponent<SCR_SpaceshipDamager>();

		// I have the ship modify its own values from public variables in the obstacle because that makes more sense to me
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
