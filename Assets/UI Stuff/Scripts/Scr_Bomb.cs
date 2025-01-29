using UnityEngine;

public class Scr_Bomb : MonoBehaviour {
    [SerializeField] int dmg;
    Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.GetComponent<Scr_OctoPlayer>()) {
			collision.gameObject.GetComponent<Scr_OctoPlayer>().damage(dmg);
		}
	}
}
