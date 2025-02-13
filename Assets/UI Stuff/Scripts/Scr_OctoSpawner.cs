using UnityEngine;

public class Scr_OctoSpawner : MonoBehaviour {
    [SerializeField] GameObject prefab;

    public void Spawn() {
        Instantiate(prefab);
    }
}
