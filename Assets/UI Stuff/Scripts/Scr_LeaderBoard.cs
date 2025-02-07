using UnityEngine;
using UnityEngine.UI;

public class Scr_LeaderBoard : MonoBehaviour {
    [SerializeField] GameObject createPanel;
    [SerializeField] GameObject continueBtn;
    [SerializeField] GameObject loserpanel;
    [SerializeField] Scr_BoardSlot[] slots;

    int newScore;

    void Start() {
        
    }

    void Update() {
        
    }

    public void continueClick() {
        
    }

    public void createClick(Image img, int pos) {
        slots[pos].changeSlot(img, newScore);
    }

    public void highScore() {
        createPanel.SetActive(true);
    }

    public void loser() {
        loserpanel.SetActive(true);
    }
}