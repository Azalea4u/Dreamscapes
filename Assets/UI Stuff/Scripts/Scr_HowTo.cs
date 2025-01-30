using UnityEngine;

public class Scr_HowTo : MonoBehaviour {
    [SerializeField] string gameTitle;
    [SerializeField] string gameHow;
    [SerializeField] GameObject howPanel;

    public void howClick() {
        howPanel.SetActive(!howPanel.active);
    }
}
