using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_OctoEnemy : MonoBehaviour {
    [SerializeField] int health = 100;
    [SerializeField] GameObject shootFab;
    [SerializeField] GameObject tentacleFab;
    [SerializeField] float attackTimer;
    [SerializeField] float moveTimer;
	[SerializeField] spaces[] availableSpaces = new spaces[5];
	[SerializeField] int position = 2;

	[SerializeField] int tentaclePos = 0;

	[Serializable] public struct spaces
	{
		public GameObject somethingHereRef;
		public Vector3 spacePosition;
	}


	void Start() {
		attackTimer = 1.0f;
		moveTimer = 1.0f;

		transform.position = availableSpaces[position].spacePosition;
	}

	void Update() {
		//transform.position = Vector3.Lerp(transform.position, availableSpaces[position].spacePosition, Time.deltaTime);


		moveTimer -= Time.deltaTime;

		if (moveTimer <= 0)
		{
			moveTimer = 1.0f;
			if (availableSpaces[position].somethingHereRef == null)
			{
				if (availableSpaces[tentaclePos].somethingHereRef == null)
				{
					TentacleAttack();
				} else
				{
					gunAttack();
				}
			}

			spaces leftspace = getLeftSpace();
			spaces rightspace = getRightSpace();

			if (leftspace.somethingHereRef == null && rightspace.somethingHereRef == null)
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
			else if (leftspace.somethingHereRef == null)
			{
				moveLeft();
			} else if (rightspace.somethingHereRef == null)
			{
				moveRight();
			} else
			{
				if (UnityEngine.Random.Range(0,2) == 0)
				{
					moveLeft();
				} else
				{
					moveRight();
				}
			}
		}

		//attackTimer -= Time.deltaTime;


		//if (attackTimer <= 0)
		//{
		//	if (UnityEngine.Random.Range(0, 3) != 0)
		//	{
		//		attackTimer = 1.1f;
		//		gunAttack();
		//	}
		//	else
		//	{
		//		attackTimer = 0.7f;
		//		TentacleAttack();
		//	}
		//}
		//else if (moveTimer <= 0)
		//{
		//	switch (UnityEngine.Random.Range(0, 2))
		//	{
		//		case 0:
		//			moveTimer = 2.0f;
		//			moveLeft();
		//			break;

		//		case 1:
		//			moveTimer = 1.5f;
		//			moveRight();
		//			break;
		//	}
		//	attackTimer += 0.5f;
		//}
	}

	public spaces getLeftSpace()
	{
		int pos = position - 1;
		if (pos < 0)
		{
			pos = availableSpaces.Length - 1;
		}

		return availableSpaces[pos];
	}

	public spaces getRightSpace()
	{
		int pos = position + 1;
		if (pos >= availableSpaces.Length)
		{
			pos = 0;
		}

		return availableSpaces[pos];
	}

	public void moveLeft()
	{
		position -= 1;
		if (position < 0)
		{
			position = availableSpaces.Length - 1;
		}
		//transform.Translate(new Vector3(-1, 0, 0));
		//if (transform.position.x <= -2) transform.position = new Vector3(2, 3, 0);
		transform.position = availableSpaces[position].spacePosition;
	}

	public void moveRight()
	{
		position += 1;
		if (position >= availableSpaces.Length)
		{
			position = 0;
		}
		//transform.Translate(new Vector3(1, 0, 0));
		//if (transform.position.x >= 2) transform.position = new Vector3(-2, 3, 0);
		transform.position = availableSpaces[position].spacePosition;
	}

	public void damage(int d) {
        health -= d;
        if (health <= 0) {
            Destroy(gameObject); 
            SceneManager.LoadScene("SceneUI");
        }
    }

    public void TentacleAttack()
	{
		tentaclePos = position;
		availableSpaces[position].somethingHereRef = Instantiate(tentacleFab, transform.position, transform.rotation);
	}

    public void gunAttack() {
		availableSpaces[position].somethingHereRef = Instantiate(shootFab, transform.position, transform.rotation);
    }
}