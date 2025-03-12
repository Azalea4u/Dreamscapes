using UnityEngine;

public class Scr_Bomb : MonoBehaviour {
	[SerializeField] float speed;
	[SerializeField] Transform visuals;
    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
		rb.linearVelocityY = -speed;
	}

    void Update() {
		// -6.0f is off the screen on the bottom
		if (transform.position.y <= -6.0f)
		{
			Destroy(gameObject);
		}
		visuals.Rotate(Vector3.forward, Time.deltaTime * 200);
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.GetComponent<Scr_OctoPlayer>()) {
			Scr_OctoEnemy.instance.damage(-3);
			Destroy(gameObject);
		}
	}
}
