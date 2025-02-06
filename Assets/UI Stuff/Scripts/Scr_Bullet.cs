using UnityEngine;

public class Scr_Bullet : MonoBehaviour {
    Rigidbody2D rb;

    void Start() {
        transform.position = FindAnyObjectByType<Scr_OctoPlayer>().transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = 5;
    }

    void Update() {
        
    }

	private void OnTriggerEnter2D(Collider2D other) {

        if (other.GetComponent<Scr_Bomb>())
        {
            Destroy(gameObject);
        }
        if (other.GetComponents<Scr_OctoEnemy>().Length > 0) {
            other.GetComponent<Scr_OctoEnemy>().damage(2);
            Destroy(gameObject);
        }
	}
}