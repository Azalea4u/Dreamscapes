using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_OctoEnemy : MonoBehaviour {
    [SerializeField] int health = 100;
    [SerializeField] GameObject shootFab;
    [SerializeField] GameObject tentacleFab;
    [SerializeField] float attackTimer;

    void Start() {
        
    }

    void Update() {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            switch (Random.Range(0,3))
            {
                case 0:
                    moveLeft();
					attackTimer = 1.0f;
					break;

                case 1:
                    moveRight();
					attackTimer = 0.5f;
					break;

                case 2:
                    gunAttack();
                    attackTimer = 0.5f;
                    break;
            }
        }
    }

	public void moveLeft()
	{
		transform.Translate(new Vector3(-1, 0, 0));
		if (transform.position.x <= -2) transform.position = new Vector3(2, 3, 0); ;
	}

	public void moveRight()
	{
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
		Instantiate(tentacleFab, transform.position, transform.rotation);
	}

    public void gunAttack() {
        Instantiate(shootFab, transform.position, transform.rotation);
    }
}