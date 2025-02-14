using System.Collections;
using UnityEngine;

public class Scr_OctoPlayer : MonoBehaviour {
    [SerializeField] GameObject projPrefab;
    Rigidbody2D rb;
    public int position = 0;
    bool lastMovedLeft = false;
	[SerializeField] float shootDelay = 0.5f;
	float shootTimer;

    [Header("Game States")]
    [SerializeField] private GameObject GameOver_Panel;

    void Start() {
        GameOver_Panel.SetActive(false);

        FindAnyObjectByType<Scr_OctopusUI>().setPlayer();
        rb = GetComponent<Rigidbody2D>();

		position = 0;
    }

	private void Update()
	{
		shootTimer -= Time.deltaTime;
		if (shootTimer <= 0)
		{
			shootTimer = shootDelay;
			shoot();
		}
	}

	public void moveLeft() {
        position -= 1;
		if (position < -2)
		{
			position = -2;
			return;
		}

        lastMovedLeft = true;

        rb.transform.Translate(new Vector3(-1, 0, 0));

		CheckTentacleOverlap();
	}

    public void moveRight() {
        position += 1;
		if (position > 2)
		{
			position = 2;
			return;
		}

		lastMovedLeft = false;

		rb.transform.Translate(new Vector3(1, 0, 0));

		CheckTentacleOverlap();
	}

	private void CheckTentacleOverlap()
	{
		Collider2D[] collisions = Physics2D.OverlapPointAll(rb.transform.position);

		foreach (var collision in collisions)
		{
			if (collision.attachedRigidbody.GetComponent<Scr_Tentacle>())
			{
				if (lastMovedLeft)
				{
					moveRight();
				}
				else
				{
					moveLeft();
				}
				return;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision);

		if (collision.attachedRigidbody.GetComponent<Scr_Tentacle>())
		{
			if (lastMovedLeft)
			{
				moveRight();
			}
			else
			{
				moveLeft();
			}
		}
	}

	public void shoot() { 
        Instantiate(projPrefab);
    }

    private void GameOver()
    {
        GameOver_Panel.SetActive(true);

        Destroy(gameObject);
        StartCoroutine(ShowScreen());
    }

    private IEnumerator ShowScreen()
    {
        yield return new WaitForSeconds(0.75f);

        GameOver_Panel.SetActive(true);
    }
}