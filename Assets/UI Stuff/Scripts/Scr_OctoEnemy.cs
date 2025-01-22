using UnityEngine;

public class Scr_OctoEnemy : MonoBehaviour {
    [SerializeField] int health = 100;

    void Start() {
        
    }

    void Update() {
        
    }

    public void damage(int d) {
        health -= d;
        if (health < 0) Destroy(gameObject);
    }
}