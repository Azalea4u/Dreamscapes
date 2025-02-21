using UnityEngine;

public class Scr_Bullet : MonoBehaviour {
	[SerializeField] Transform visuals;
	Rigidbody2D rb;

    void Start() {
        transform.position = FindAnyObjectByType<Scr_OctoPlayer>().transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = 5;
    }

    void Update() {
		if (transform.position.y >= 6.0f)
		{
			Destroy(gameObject);
		}
		visuals.Rotate(Vector3.forward, Time.deltaTime * -100);
	}

	private void OnTriggerEnter2D(Collider2D other) {

        if (other.GetComponent<Scr_Bomb>())
        {
            Destroy(gameObject);
        }
        if (other.GetComponents<Scr_OctoEnemy>().Length > 0) {
            other.GetComponent<Scr_OctoEnemy>().damage(1);
            Destroy(gameObject);
        }
	}
}