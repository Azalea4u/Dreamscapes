using UnityEngine;
using UnityEngine.UI;

public class SCR_ToggleGameDone : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button StartOver_BTN;
    [SerializeField] private Button ExitToMenu_BTN;

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            AddWait_GameDone();
            GameManager.instance.PauseGame(true);
        }
    }

    public void AddWait_GameDone()
    {
        StartOver_BTN.interactable = false;
        ExitToMenu_BTN.interactable = false;

        GameManager.instance.Add_WaitTime(0.5f);

        StartOver_BTN.interactable = true;
        ExitToMenu_BTN.interactable = true;
    }
}
