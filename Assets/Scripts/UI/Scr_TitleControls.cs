using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scr_TitleControls : MonoBehaviour {
    [SerializeField] Button[] buttons;
    [SerializeField] Vector2[] btnTransforms;
	[SerializeField] Image gameImg;

    void Start() {
		gameImg.sprite = buttons[0].GetComponent<Scr_HowTo>().gameImg;

		UpdateBTNSize();
	}

    public void leftClick() {
		List<Button> newBtns = new List<Button>();
		for (int i = 0; i < buttons.Length; i++) {
			if (i == 0) {
				newBtns.Add(buttons[buttons.Length - 1]);
				continue;
			}
			newBtns.Add(buttons[i - 1]);
		}
		buttons = newBtns.ToArray();

		for (int i = 0; i < buttons.Length; i++) {
			buttons[i].GetComponent<RectTransform>().anchoredPosition = btnTransforms[i];
		}

		gameImg.sprite = buttons[0].GetComponent<Scr_HowTo>().gameImg;
		UpdateBTNSize();
	}

	public void rightClick()
	{
		List<Button> newBtns = new List<Button>();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (i == buttons.Length - 1)
			{
				newBtns.Add(buttons[0]);
				continue;
			}
			newBtns.Add(buttons[i + 1]);
		}
		buttons = newBtns.ToArray();

		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].GetComponent<RectTransform>().anchoredPosition = btnTransforms[i];
		}

		gameImg.sprite = buttons[0].GetComponent<Scr_HowTo>().gameImg;
		UpdateBTNSize();
	}

    public void playClick() {
        buttons[0].onClick.Invoke();
    }

	private void UpdateBTNSize()
	{
		buttons[0].gameObject.transform.localScale = new Vector3(2, 2, 2);
		buttons[1].gameObject.transform.localScale = new Vector3(1, 1, 1);
		buttons[2].gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		buttons[3].gameObject.transform.localScale = new Vector3(1, 1, 1);
	}
}