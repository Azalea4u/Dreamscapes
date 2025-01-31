using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Scr_OctoPlayer : MonoBehaviour {
    [SerializeField] GameObject projPrefab;
    Rigidbody2D rb;

    [SerializeField] int health = 5;
    bool lastMovedLeft = false;

    void Start() {
        FindAnyObjectByType<Scr_OctopusUI>().setPlayer();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        
    }

    public void moveLeft() {
        lastMovedLeft = true;
        rb.transform.Translate(new Vector3(-1, 0, 0));
        if (rb.position.x <= -2) transform.position = new Vector3(2, -3, 0); ;
	}

    public void moveRight() {
		lastMovedLeft = false;
        rb.transform.Translate(new Vector3(1, 0, 0));
		if (rb.position.x >= 2) transform.position = new Vector3(-2, -3, 0);
	}

    public void shoot() { 
        Instantiate(projPrefab);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        Debug.Log(collision);

		if (collision.GetComponent<Scr_Tentacle>())
        {
            if (lastMovedLeft)
            {
                moveRight();
            } else
            {
                moveLeft();
            }
        }
	}

	public void damage(int d) {
        health -= d;
        if (health <= 0) {
            Destroy(gameObject);
            SceneManager.LoadScene("SceneUI");
        }
    }
}