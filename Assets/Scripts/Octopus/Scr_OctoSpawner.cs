using UnityEngine;

public class Scr_OctoSpawner : MonoBehaviour {
    [SerializeField] GameObject prefab;

    void Start() {
        
    }

    void Update() {
        
    }

    public void Spawn() {
        Instantiate(prefab);
    }
}
