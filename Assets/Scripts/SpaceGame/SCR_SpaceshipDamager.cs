using System.Collections;
using UnityEngine;

public class SCR_SpaceshipDamager : MonoBehaviour
{
	// speed of the object in a direction
	[SerializeField] private Vector3 speed;
	private Vector3 usedSpeed;
	// if horizontal speed should be modified on spawn to face towards the center
	[SerializeField] private bool faceTowardsCenter;
	// if the obstacle should move towards the player
	[SerializeField] private bool moveTowardsPlayer;
	// an object to move towards instead of the player
	[SerializeField] private Transform moveTowards;
	// speed that the obstacle should track towards the player or obstacle
	[SerializeField] private float moveSpeed;
	// moves with another spacehsip damager
	[SerializeField] private SCR_SpaceshipDamager moveWith;
	// if the obstacle should damage the ship
	public bool doesDamage;
	// if the object should add a bird to slow down the ship
	public bool birdSlowdown;
	// how much the obstacle slows down the movement of the ship
	[Range(0.0f,0.95f)]public float slowDownMovement;
	// instantaneous pushback from the obstacle
	public float bounce;
	// has obstacle already been hit
	public bool beenHit { get; private set; }
	// if obstacle should destroy itself after hiting the ship
	public bool destroyOnHit;
	// offset above the obstacle that, once the camera passes, will destroy the obstacle.
	// allows for really long obstacles without worrying about the real position of the obstacle.
	public float destroyHeight;

	private void Start()
	{
        if (faceTowardsCenter)
        {
			speed.x = Mathf.Abs(speed.x) * -Mathf.Sign(transform.position.x);
        }

		usedSpeed = speed * SCR_SpaceGame_Manager.instance.difficultyScale;
    }

	private void FixedUpdate()
	{
		if (SCR_SpaceGame_Manager.instance.GetSpaceship() == null)
		{
			return;
		}

		if (moveTowards != null)
		{
			usedSpeed = Vector3.RotateTowards(usedSpeed, (moveTowards.position - transform.position).normalized * speed.magnitude, moveSpeed * Time.fixedDeltaTime, speed.magnitude);
		}
		else if (moveTowardsPlayer)
		{
			usedSpeed = Vector3.RotateTowards(usedSpeed, (SCR_SpaceGame_Manager.instance.GetSpaceship().transform.position - transform.position).normalized * speed.magnitude, moveSpeed * Time.fixedDeltaTime, speed.magnitude);
		}

		transform.position += usedSpeed * Time.fixedDeltaTime;
		if (moveWith != null)
		{
			transform.position += moveWith.GetSpeed() * Time.fixedDeltaTime;
		}

		if (transform.position.y + destroyHeight <= Camera.main.transform.position.y - 7.0f)
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Getter for the obstacles speed
	/// </summary>
	/// <returns>The obstacles speed</returns>
	public Vector3 GetSpeed()
	{
		return speed;
	}

	/// <summary>
	/// Notifies the obstacle that it has hit the ship, and to change things accordingly
	/// </summary>
	public void HitShip()
	{
		beenHit = true;

		if (destroyOnHit)
		{
			Destroy(gameObject);
		}
	}

	private void OnDrawGizmosSelected()
	{
		// Draws a reference for the height at which the obstacle will destroy itself
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + destroyHeight), 0.1f);
	}
}
