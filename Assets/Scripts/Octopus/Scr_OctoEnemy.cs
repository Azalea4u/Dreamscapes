using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_OctoEnemy : MonoBehaviour {
	public static Scr_OctoEnemy instance { get; private set; }

	Scr_OctoPlayer player;
	
	[Header("Setup Values")]
	[SerializeField] int health = 100;
	int _health;
    [SerializeField] GameObject shootFab;
    [SerializeField] GameObject tentacleFab;
	[SerializeField] spaces[] availableSpaces = new spaces[5];

	[Header("Runtime Values")]
    float moveTimer;
	float timeSpentMoving;
	// prev position is used to allow Poly to move to its new position smoothly
	int prevPos;
	[SerializeField] int position = 2;
	[SerializeField] SpriteRenderer spriteRenderer;
	// to track if a tentacle is spawned in
	// also helpful for updating the material
	Scr_Tentacle tentacleRef;

    [Header("Game States")]
    [SerializeField] private GameObject GameWin_Panel;
    [SerializeField] private GameObject LeaderboardUI;

	// timers used for leaderboard score
	float overallTime;

	// A space tracks what is within it (Tentacle or Bubble) for helping with AI decisions
    [Serializable] public struct spaces
	{
		public GameObject somethingHereRef;
		public Vector3 spacePosition;
	}

	void Start() {
		instance = this;

		_health = health;
		prevPos = position;

		GameWin_Panel.SetActive(false);
		LeaderboardUI.SetActive(false);

		moveTimer = 1.0f;
		overallTime = 0;

		transform.position = availableSpaces[position].spacePosition;

		player = FindFirstObjectByType<Scr_OctoPlayer>();
	}

	void Update() {
		if (_health <= 0)
		{
			return;
		}

		moveTimer -= Time.deltaTime;
		overallTime += Time.deltaTime;

		transform.position = Vector3.Lerp(availableSpaces[position].spacePosition, availableSpaces[prevPos].spacePosition, Mathf.Clamp01((moveTimer - (timeSpentMoving * 0.25f)) / (timeSpentMoving - (timeSpentMoving * 0.25f))));

		// The Enemy AI is decided here upon move timer expiring
		if (moveTimer <= 0)
		{
			prevPos = position;

			moveTimer = UnityEngine.Random.Range(0.5f, 1.5f);
			timeSpentMoving = moveTimer;

			if (availableSpaces[position].somethingHereRef == null)
			{
				if (tentacleRef == null)
				{
					TentacleAttack();
				} else
				{
					gunAttack();
				}
			}
			else
			{
				spaces leftspace = getLeftSpace();
				spaces rightspace = getRightSpace();

				if (leftspace.somethingHereRef == null && rightspace.somethingHereRef == null)
				{
					moveRandomLeftRight();
				}
				else if (leftspace.somethingHereRef == null)
				{
					moveLeft();
				}
				else if (rightspace.somethingHereRef == null)
				{
					moveRight();
				}
				else
				{
					moveRandomLeftRight();
				}
			}
		}
	}

	public spaces getLeftSpace()
	{
		int pos = position - 1;
		if (pos < 0)
		{
			return availableSpaces[position];
		}

		return availableSpaces[pos];
	}

	public spaces getRightSpace()
	{
		int pos = position + 1;
		if (pos >= availableSpaces.Length)
		{
			return availableSpaces[position];
		}

		return availableSpaces[pos];
	}

	public void moveLeft()
	{
		position -= 1;
		if (position < 0)
		{
			position = 0;
		}
	}

	public void moveRight()
	{
		position += 1;
		if (position > availableSpaces.Length - 1)
		{
			position = availableSpaces.Length - 1;
		}
	}

	public float GetHealthPercent()
	{
		return Mathf.Sqrt((float)_health / health);
	}

	public void moveRandomLeftRight()
	{
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			moveLeft();
		}
		else
		{
			moveRight();
		}
	}

    public void TentacleAttack()
	{
		availableSpaces[position].somethingHereRef = Instantiate(tentacleFab, transform.position, transform.rotation);
		tentacleRef = availableSpaces[position].somethingHereRef.GetComponent<Scr_Tentacle>();
	}

    public void gunAttack() {
		availableSpaces[position].somethingHereRef = Instantiate(shootFab, transform.position, transform.rotation);
    }

	public void damage(int d) {
        _health -= d;
		_health = Mathf.Clamp(_health, 0, health);
        if (d > 0)
        {
			// if the damage is above 0, it should flash white to indicate damage
			damageFlicker();
			if (tentacleRef != null)
			{
				tentacleRef.DamageFlicker();
			}
		} else if (d < 0)
		{
			// if the damage is less that 0, it should flash black to indicate dwindling taking over
			damageDark();
			if (tentacleRef != null)
			{
				tentacleRef.DamageDark();
			}
		}

		// sets the strength of the greyscale
		spriteRenderer.material.SetFloat("_Strength", 1.0f - GetHealthPercent());
		if (tentacleRef != null)
		{
			tentacleRef.UpdateGreyscale(1.0f - GetHealthPercent());
		}

		if (_health <= 0) {
			GameWin();
        }
    }
	
	private void damageFlicker()
	{
		spriteRenderer.material.SetInt("_Flash", 1);
		StartCoroutine(Flicker());
	}

	private IEnumerator Flicker()
	{
		yield return new WaitForSeconds(Time.deltaTime * 6);
		spriteRenderer.material.SetInt("_Flash", 0);
	}

	private void damageDark()
	{
		spriteRenderer.material.SetInt("_Dark", 1);
		StartCoroutine(Dark());
	}

	private IEnumerator Dark()
	{
		yield return new WaitForSeconds(Time.deltaTime * 6);
		spriteRenderer.material.SetInt("_Dark", 0);
	}

	private void GameWin()
	{
		//GameWin_Panel.SetActive(true);
		LeaderboardUI.SetActive(true);
		LeaderboardUI.GetComponent<Scr_LeaderBoard>().endGameReverse((int)overallTime);
        //Destroy(gameObject);

       // StartCoroutine(ShowScreen());
       // StartCoroutine(ShowScreen());
    }

    private IEnumerator ShowScreen()
    {
        yield return new WaitForSeconds(0.75f);

        GameWin_Panel.SetActive(true);
    }
}