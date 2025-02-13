using System.Collections;
using UnityEngine;

public class Scr_OctoPlayer : MonoBehaviour
{
    [SerializeField] GameObject projPrefab;
    Rigidbody2D rb;
    public int position = 2;
    bool lastMovedLeft = false;
	[SerializeField] float shootDelay = 0.5f;
	float shootTimer;

    [Header("Game States")]
    [SerializeField] private GameObject GameOver_Panel;

    void Start()
	{
        GameOver_Panel.SetActive(false);

        FindAnyObjectByType<Scr_OctopusUI>().setPlayer();
        rb = GetComponent<Rigidbody2D>();
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

	public void moveLeft()
	{
        lastMovedLeft = true;
        position -= 1;
        rb.transform.Translate(new Vector3(-1, 0, 0));
        if (rb.position.x <= -2) rb.transform.Translate(new Vector3(1, 0, 0));

		Collider2D[] collisions = Physics2D.OverlapPointAll(rb.transform.position);

        foreach (var collision in collisions)
        {
			if (collision.attachedRigidbody.GetComponent<Scr_Tentacle>())
			{
				moveRight();
                return;
			}
		}
	}

    public void moveRight()
	{
		lastMovedLeft = false;
        position += 1;
        rb.transform.Translate(new Vector3(1, 0, 0));
		if (rb.position.x >= 2) rb.transform.Translate(new Vector3(-1, 0, 0));

		Collider2D[] collisions = Physics2D.OverlapPointAll(rb.transform.position);

		foreach (var collision in collisions)
		{
			if (collision.attachedRigidbody.GetComponent<Scr_Tentacle>())
			{
				moveLeft();
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

	public void shoot()
	{ 
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