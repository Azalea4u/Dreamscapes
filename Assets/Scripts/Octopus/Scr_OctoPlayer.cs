using System.Collections;
using UnityEngine;

public class Scr_OctoPlayer : MonoBehaviour {
    Rigidbody2D rb;
	// the player is limited to 5 different positions -2, -1, 0, 1, 2
    public int position = 0;
	// tracks which direction the player has moved for when they get hit by a tentacle. they need to move back
    bool lastMovedLeft = false;

	[Header("Projectile Values")]
    [SerializeField] GameObject projPrefab;
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
		if (Scr_OctoEnemy.instance.GetHealthPercent() <= 0.0f)
		{
			return;
		}

		for (int i = 0; i < Input.touchCount; i++)
		{
			if (Input.GetTouch(i).phase == TouchPhase.Began)
			{
				Vector3 touchpos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				if (touchpos.x < 0)
				{
					moveLeft();
				}
				else
				{
					moveRight();
				}
			}
		}

		// move and rotate the ship towards where it is going
		transform.position = Vector3.Lerp(transform.position, getDesiredPosition(), Time.deltaTime * 10);
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, (transform.position.x - getDesiredPosition().x) * 30), Time.deltaTime * 10);

		shootTimer -= Time.deltaTime;
		if (shootTimer <= 0)
		{
			shootTimer = shootDelay;
			shoot();
		}

		CheckTentacleOverlap();
	}

	public void moveLeft() {
        position -= 1;
		if (position < -2)
		{
			position = -2;
			return;
		}

        lastMovedLeft = true;
	}

    public void moveRight() {
        position += 1;
		if (position > 2)
		{
			position = 2;
			return;
		}

		lastMovedLeft = false;
	}

	private Vector3 getDesiredPosition()
	{
		return new Vector3(position, transform.position.y, 0);
	}

	private void CheckTentacleOverlap()
	{
		Collider2D[] collisions = Physics2D.OverlapPointAll(getDesiredPosition());

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