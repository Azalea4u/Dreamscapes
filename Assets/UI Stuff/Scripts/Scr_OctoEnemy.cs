using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_OctoEnemy : MonoBehaviour {
	public static Scr_OctoEnemy instance { get; private set; }

	[SerializeField] Scr_OctoPlayer player;
    [SerializeField] int health = 100;
    [SerializeField] GameObject shootFab;
    [SerializeField] GameObject tentacleFab;
    [SerializeField] float moveTimer;
	[SerializeField] spaces[] availableSpaces = new spaces[5];
	[SerializeField] int position = 2;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] Scr_Tentacle tentacleRef;

    [Header("Game States")]
    [SerializeField] private GameObject GameWin_Panel;

    [Serializable] public struct spaces
	{
		public GameObject somethingHereRef;
		public Vector3 spacePosition;
	}

	void Start()
	{
		instance = this;

		GameWin_Panel.SetActive(false);

		moveTimer = 1.0f;

		transform.position = availableSpaces[position].spacePosition;

		player = FindFirstObjectByType<Scr_OctoPlayer>();
	}

	void Update()
	{
		//transform.position = Vector3.Lerp(transform.position, availableSpaces[position].spacePosition, Time.deltaTime);
		spriteRenderer.material.SetFloat("_Strength", 1.0f - (float)health / 100.0f);
		if (tentacleRef != null)
		{
			tentacleRef.UpdateGreyscale(1.0f - (float)health / 100.0f);
		}


		moveTimer -= Time.deltaTime;

		if (moveTimer <= 0)
		{
			moveTimer = UnityEngine.Random.Range(0.5f, 1.5f);
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

		transform.position = availableSpaces[position].spacePosition;
	}

	public void moveRight()
	{
		position += 1;
		if (position > availableSpaces.Length - 1)
		{
			position = availableSpaces.Length - 1;
		}

		transform.position = availableSpaces[position].spacePosition;
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

	public void damage(int d)
	{
        health -= d;
		health = Mathf.Clamp(health, 0, 100);
        if (d > 0)
        {
			damageFlicker();
			if (tentacleRef != null)
			{
				tentacleRef.DamageFlicker();
			}
		}
		if (health <= 0) {
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
        GameWin_Panel.SetActive(true);
        Destroy(gameObject);

       // StartCoroutine(ShowScreen());
    }

    private IEnumerator ShowScreen()
    {
        yield return new WaitForSeconds(0.75f);

        GameWin_Panel.SetActive(true);
    }
}