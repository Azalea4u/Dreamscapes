using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Scr_OctoPlayer : MonoBehaviour {
    [SerializeField] GameObject projPrefab;
    Rigidbody2D rb;

    [SerializeField] int health = 5;

    void Start() {
        FindAnyObjectByType<Scr_OctopusUI>().setPlayer();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        
    }

    public void moveLeft() {
        rb.transform.Translate(new Vector3(-1, 0, 0));
        if (rb.position.x <= -2) transform.position = new Vector3(2, -3, 0); ;
    }

    public void moveRight() {
        rb.transform.Translate(new Vector3(1, 0, 0));
		if (rb.position.x >= 2) transform.position = new Vector3(-2, -3, 0);
	}

    public void shoot() { 
        Instantiate(projPrefab);
    }

    public void damage(int d) {
        health -= d;
        if (health <= 0) {
            Destroy(gameObject);
            SceneManager.LoadScene("SceneUI");
        }
    }
}