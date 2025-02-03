using UnityEngine;

public class Scr_Bomb : MonoBehaviour {
    [SerializeField] int dmg;
    Rigidbody2D rb;
	[SerializeField] float deathTimer = 5.0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
		rb.linearVelocityY = -3;
		deathTimer = 5.0f;
	}

    void Update() {
        deathTimer -= Time.deltaTime;
		if (deathTimer <= 0)
		{
			Destroy(gameObject);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.GetComponent<Scr_OctoPlayer>()) {
			collision.gameObject.GetComponent<Scr_OctoPlayer>().damage(dmg);
			Destroy(gameObject);
		}
	}
}
