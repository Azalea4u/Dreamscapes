using UnityEngine;

public class Scr_OctopusUI : MonoBehaviour {
    Scr_OctoPlayer player;

    void Start() {
    }

    void Update() {
        
    }

    public void setPlayer() {
        player = FindAnyObjectByType<Scr_OctoPlayer>();
    }

    public void moveClick(bool left) {
        if (left) player.moveLeft(); else player.moveRight();
    }

    public void shootClick() {
        player.shoot();
    }
}