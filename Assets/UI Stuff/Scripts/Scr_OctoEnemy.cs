using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_OctoEnemy : MonoBehaviour {
	public static Scr_OctoEnemy instance { get; private set; }

	[SerializeField] Scr_OctoPlayer player;
    [SerializeField] int health = 100;
	[SerializeField] int _health;
    [SerializeField] GameObject shootFab;
    [SerializeField] GameObject tentacleFab;
    [SerializeField] float moveTimer;
	float timeSpentMoving;
	[SerializeField] spaces[] availableSpaces = new spaces[5];
	[SerializeField] int position = 2;
	int prevPos;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Scr_Tentacle tentacleRef;

    [Header("Game States")]
    [SerializeField] private GameObject GameWin_Panel;
    [SerializeField] private GameObject LeaderboardUI;

	float overallTime;
	float freeTime;

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
		overallTime = 100;
		freeTime = 22;

		transform.position = availableSpaces[position].spacePosition;

		player = FindFirstObjectByType<Scr_OctoPlayer>();
	}

	void Update() {
		if (_health <= 0)
		{
			return;
		}

		spriteRenderer.material.SetFloat("_Strength", GetHealthPercent());
		if (tentacleRef != null)
		{
			tentacleRef.UpdateGreyscale(GetHealthPercent());
		}

		moveTimer -= Time.deltaTime;

		if (freeTime > 0) {
			freeTime -= Time.deltaTime;
		} else if (overallTime > 0) {
			overallTime -= Time.deltaTime;
		} else {
			//added this in case deltaTime makes overallTime less than 0
			overallTime = 0;
		}

		transform.position = Vector3.Lerp(availableSpaces[position].spacePosition, availableSpaces[prevPos].spacePosition, Mathf.Clamp01((moveTimer - (timeSpentMoving * 0.25f)) / (timeSpentMoving - (timeSpentMoving * 0.25f))));

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

		//transform.position = availableSpaces[position].spacePosition;
	}

	public void moveRight()
	{
		position += 1;
		if (position > availableSpaces.Length - 1)
		{
			position = availableSpaces.Length - 1;
		}

		//transform.position = availableSpaces[position].spacePosition;
	}

	public float GetHealthPercent()
	{
		return 1.0f - Mathf.Sqrt((float)_health / health);
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
			damageFlicker();
			if (tentacleRef != null)
			{
				tentacleRef.DamageFlicker();
			}
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

	private void GameWin()
	{
		//GameWin_Panel.SetActive(true);
		LeaderboardUI.SetActive(true);
		LeaderboardUI.GetComponent<Scr_LeaderBoard>().endGame((int)overallTime);
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