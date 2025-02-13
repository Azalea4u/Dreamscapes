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
		StartCoroutine(WaitToClose());
	}

	public void close()
	{
		tutorialContainer.SetActive(false);
        Pause_BTN.GetComponent<Button>().interactable = true;
        GameManager.instance.PauseGame(false);
	}

	private IEnumerator WaitToClose()
	{
        Pause_BTN.GetComponent<Button>().interactable = false;
		Tutorial_BTN.GetComponent<Button>().interactable = false;

        tutorialContainer.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        Tutorial_BTN.GetComponent<Button>().interactable = true;

        GameManager.instance.PauseGame(true);
    }
}
