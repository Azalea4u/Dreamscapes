using UnityEngine;

public class Scr_Bullet : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;

    void Start() {
        transform.position = FindAnyObjectByType<Scr_OctoPlayer>().transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = 5;
    }

    void Update() {
        
    }

	private void OnTriggerEnter(Collider other) {
        if (other.GetComponents<Scr_OctoEnemy>().Length > 0) {
            other.GetComponent<Scr_OctoEnemy>().damage(10);
            Destroy(gameObject);
        }
	}
}