using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Scr_OctoPlayer : MonoBehaviour {
    [SerializeField] GameObject projPrefab;
    Rigidbody2D rb;

    [SerializeField] int health = 5;
    bool lastMovedLeft = false;

    [Header("Game States")]
    [SerializeField] private GameObject GameOver_Panel;

    void Start() {
        GameOver_Panel.SetActive(false);

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

		if (collision.attachedRigidbody.GetComponent<Scr_Tentacle>())
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
            GameOver();
        }
    }

    private void GameOver()
    {
        Destroy(gameObject);
        StartCoroutine(ShowScreen());
    }

    private IEnumerator ShowScreen()
    {
        yield return new WaitForSeconds(0.75f);

        GameOver_Panel.SetActive(true);
    }
}