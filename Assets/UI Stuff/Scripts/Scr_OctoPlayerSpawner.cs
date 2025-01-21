using UnityEngine;

public class Scr_OctoSpawner : MonoBehaviour {
    [SerializeField] GameObject octoPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Instantiate(octoPlayer);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
