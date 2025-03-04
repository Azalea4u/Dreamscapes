using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scr_BoardSlot : MonoBehaviour {
    public Image slotImg;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI nameTxt;

	public void changeSlot(Sprite img, int score, string name) {
		slotImg.sprite = img;
		scoreTxt.text = "" + score;
		nameTxt.text = name;
	}

	public void changeSlotTime(Sprite img, int score, string name) {
		slotImg.sprite = img;
		scoreTxt.text = "" + score + "s";
		nameTxt.text = name;
	}

	/*
	 Just controls the ui stuff for the leaderboard slots
	 */
}