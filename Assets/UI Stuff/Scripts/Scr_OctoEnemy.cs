using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_OctoEnemy : MonoBehaviour {
    [SerializeField] int health = 100;
    [SerializeField] GameObject shootFab;
    [SerializeField] GameObject tentacleFab;
    [SerializeField] float attackTimer;
    [SerializeField] float moveTimer;
	[SerializeField] bool[] spacesAvailable = new bool[5];
	[SerializeField] int position = 2;

	GameObject tentacleRef;



	void Start() {
		attackTimer = 1.0f;
		moveTimer = 1.0f;
	}

	void Update() {
		attackTimer -= Time.deltaTime;
		moveTimer -= Time.deltaTime;

		if (attackTimer <= 0)
		{
			if (tentacleRef != null || Random.Range(0,3) != 0) {
				attackTimer = 1.1f;
				gunAttack();
			}	else { 
				attackTimer = 0.7f;
				TentacleAttack();
			}
		} else if (moveTimer <= 0) {
			switch (Random.Range(0, 2))
			{
				case 0:
					moveTimer = 2.0f;
					moveLeft();
					break;

				case 1:
					moveTimer = 1.5f;
					moveRight();
					break;
			}
			attackTimer += 0.5f;
		}
	}

	public void moveLeft()
	{
		position -= 1;
		if (position < 0)
		{
			position = spacesAvailable.Length - 1;
		}
		transform.Translate(new Vector3(-1, 0, 0));
		if (transform.position.x <= -2) transform.position = new Vector3(2, 3, 0);
	}

	public void moveRight()
	{
		position += 1;
		if (position >= spacesAvailable.Length)
		{
			position = 0;
		}
		transform.Translate(new Vector3(1, 0, 0));
		if (transform.position.x >= 2) transform.position = new Vector3(-2, 3, 0);
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
		tentacleRef = Instantiate(tentacleFab, transform.position, transform.rotation);
	}

    public void gunAttack() {
        Instantiate(shootFab, transform.position, transform.rotation);
    }
}