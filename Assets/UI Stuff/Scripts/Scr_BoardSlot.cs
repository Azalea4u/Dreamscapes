using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scr_BoardSlot : MonoBehaviour {
    public Image slotImg;
    public TextMeshProUGUI scoreTxt;

    public void changeSlot(Sprite img, int score) {
		slotImg.sprite = img;
		scoreTxt.text = "" + score;
	}
}