using UnityEngine;

public class SCR_SpaceshipDamager : MonoBehaviour
{
	// speed of the object in a direction
	[SerializeField] private Vector3 speed;
	// if the obstacle should damage the ship
	public bool doesDamage;
	// how much the obstacle slows down the movement of the ship
	[Range(0.0f,0.95f)]public float slowDownMovement;
	// how much the obstacle slows down the ascent of the ship
	[Range(0.0f,0.95f)]public float slowDownAscent;
	// instantaneous pushback from the obstacle
	public float bounce;
	// has obstacle already been hit
	public bool beenHit = false;

	private void FixedUpdate()
	{
		transform.position += speed * Time.fixedDeltaTime;
	}
}
