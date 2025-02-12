using UnityEngine;

public class Scr_Bomb : MonoBehaviour {
	[SerializeField] float speed;
    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
		rb.linearVelocityY = -speed;
	}

    void Update() {
		if (transform.position.y <= -6.0f)
		{
			Destroy(gameObject);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.GetComponent<Scr_OctoPlayer>()) {
			Scr_OctoEnemy.instance.damage(-10);
			Destroy(gameObject);
		}
	}
}
