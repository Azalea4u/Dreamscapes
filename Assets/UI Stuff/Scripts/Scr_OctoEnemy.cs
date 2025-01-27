using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_OctoEnemy : MonoBehaviour {
    [SerializeField] int health = 100;
    [SerializeField] GameObject shootFab;
    [SerializeField] float attackTimer;

    void Start() {
        
    }

    void Update() {
        
    }

    public void damage(int d) {
        health -= d;
        if (health <= 0) {
            Destroy(gameObject); 
            SceneManager.LoadScene("SceneUI");
        }
    }

    public void gunAttack() {
        Instantiate(shootFab);
    }
}