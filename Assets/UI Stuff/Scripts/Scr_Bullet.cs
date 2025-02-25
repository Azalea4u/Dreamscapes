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
		if (transform.position.y >= 6.0f)
		{
            //StartCoroutine(WaitForParticles());
            Destroy(gameObject);
            //Instantiate(ps, visuals.position, Quaternion.identity);
        }
        visuals.Rotate(Vector3.forward, Time.deltaTime * -100);
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

            StartCoroutine(WaitForParticles());
            Destroy(gameObject);
        }

        //ps.Play();
        //if (other.GetComponent<Scr_Bomb>())
        //{
        //    //StartCoroutine(WaitForParticles());
        //    Destroy(gameObject);
        //}
        //if (other.GetComponents<Scr_OctoEnemy>().Length > 0) {
        //
        //    //StartCoroutine(WaitForParticles());
        //    other.GetComponent<Scr_OctoEnemy>().damage(1);
        //
        //    Destroy(gameObject);
        //}
	}

    private IEnumerator WaitForParticles()
    {
        //ps.Play();
        yield return new WaitForSeconds(0.2f);
    }

    //not used so don't worry =)
}