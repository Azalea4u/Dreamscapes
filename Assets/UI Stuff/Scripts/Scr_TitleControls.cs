using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_TitleControls : MonoBehaviour {
    [SerializeField] Button[] buttons;
    [SerializeField] Vector2[] btnTransforms;

    void Start() {
        
    }

    void Update() {
        
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
	}

    public void rightClick() {
		List<Button> newBtns = new List<Button>();
		for (int i = 0; i < buttons.Length; i++) {
			if (i == buttons.Length - 1) {
				newBtns.Add(buttons[0]);
				continue;
			}
			newBtns.Add(buttons[i + 1]);
		}
		buttons = newBtns.ToArray();

		for (int i = 0; i < buttons.Length; i++) {
			buttons[i].GetComponent<RectTransform>().anchoredPosition = btnTransforms[i];
		}
	}

    public void playClick() {
        buttons[0].onClick.Invoke();
    }

	public void howClick() {
		buttons[0].GetComponent<Scr_HowTo>();
	}
}