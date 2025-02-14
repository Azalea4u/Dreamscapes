using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SCR_Tutorial : MonoBehaviour {
	[SerializeField] GameObject tutorialContainer;

	[Header("Pause")]
	[SerializeField] private GameObject Pause_BTN;
	[SerializeField] private GameObject Tutorial_BTN;

	void Start()
	{
        GameManager.instance.PauseGame(false);
		GameManager.instance.IsGamePaused = true;
        StartCoroutine(WaitToClose());
	}

	// Attached to the Tutorial_BTN's OnClick to close Tutorial gameobject
	public void close()
	{
		tutorialContainer.SetActive(false);
        Pause_BTN.GetComponent<Button>().interactable = true;
        GameManager.instance.PauseGame(false);
	}

	// To prevent the screen to close when someone is spamming the screen
	private IEnumerator WaitToClose()	{

        Pause_BTN.GetComponent<Button>().interactable = false;
		Tutorial_BTN.GetComponent<Button>().interactable = false;

        yield return new WaitForSeconds(0.5f);

        Tutorial_BTN.GetComponent<Button>().interactable = true;

        GameManager.instance.PauseGame(true);
    }
}
