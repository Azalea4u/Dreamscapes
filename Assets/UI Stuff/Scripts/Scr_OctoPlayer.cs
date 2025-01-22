using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public class Scr_OctoPlayer : MonoBehaviour {
    [SerializeField] GameObject projPrefab;
    Rigidbody2D rb;

    void Start() {
        FindAnyObjectByType<Scr_OctopusUI>().setPlayer();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        
    }

    public void moveLeft() {
        rb.transform.Translate(new Vector3(-1, 0, 0));
        if (rb.position.x < -2) transform.position = new Vector3(2, -3, 0); ;
    }

    public void moveRight() {
        rb.transform.Translate(new Vector3(1, 0, 0));
		if (rb.position.x > 2) transform.position = new Vector3(-2, -3, 0);
	}

    public void shoot() { 
        Instantiate(projPrefab);

    }
}