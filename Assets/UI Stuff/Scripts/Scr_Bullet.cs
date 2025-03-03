using System.Collections;
using UnityEngine;

public class Scr_Bullet : MonoBehaviour {
	[SerializeField] Transform visuals;
	Rigidbody2D rb;
    [SerializeField] ParticleSystem ps;
    float time;

    private string layer;

    void Start() {
        transform.position = FindAnyObjectByType<Scr_OctoPlayer>().transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocityY = 5;
    }

    void Update() {
        // 6.0f is off the screen on the top
		if (transform.position.y >= 6.0f)
		{
            Destroy(gameObject);
        }
        visuals.Rotate(Vector3.forward, Time.deltaTime * -200);
	}

	private void OnTriggerEnter2D(Collider2D other) 
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Instantiate(ps, other.transform);
            ps.Play();

            if (other.gameObject.tag == "Enemy")
            {
                other.GetComponent<Scr_OctoEnemy>().damage(1);
            }

            Destroy(gameObject);
        }
	}
}