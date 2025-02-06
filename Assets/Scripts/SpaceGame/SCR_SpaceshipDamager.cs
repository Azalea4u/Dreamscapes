using UnityEngine;
using UnityEngine.InputSystem.Controls;
public class SCR_SpaceshipDamager : MonoBehaviour
{
	// speed of the object in a direction
	[SerializeField] private Vector3 speed;
	[SerializeField] private Vector3 usedSpeed;
	// if horizontal speed should be modified on spawn to face towards the center
	[SerializeField] private bool faceTowardsCenter;
	// if the obstacle should move towards the player
	[SerializeField] private bool moveTowardsPlayer;
	// speed that obstacle should move towards the player
	[SerializeField] private float moveSpeed;
	// if the obstacle should damage the ship
	public bool doesDamage;
	// how much the obstacle slows down the movement of the ship
	[Range(0.0f,0.95f)]public float slowDownMovement;
	// how much the obstacle slows down the ascent of the ship
	// [Range(0.0f,0.95f)]public float slowDownAscent;
	// instantaneous pushback from the obstacle
	public float bounce;
	// has obstacle already been hit
	public bool beenHit { get; private set; }

	private void Start()
	{
        if (faceTowardsCenter)
        {
			speed.x = Mathf.Abs(speed.x) * -Mathf.Sign(transform.position.x);
			//speed.y = Mathf.Abs(speed.y) * -1;
        }

		usedSpeed = speed;
    }

	private void FixedUpdate()
	{
		if (moveTowardsPlayer)
		{
			usedSpeed = Vector3.RotateTowards(usedSpeed, (SCR_SpaceGame_Manager.instance.GetSpaceship().transform.position - transform.position).normalized * speed.magnitude, moveSpeed * Time.fixedDeltaTime, speed.magnitude);
		}

		transform.position += usedSpeed * Time.fixedDeltaTime;
		if (transform.position.y <= Camera.main.transform.position.y - 7.0f)
		{
			Destroy(gameObject);
		}
	}

	public void HitShip()
	{
		beenHit = true;
	}
}
