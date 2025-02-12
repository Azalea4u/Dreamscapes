using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scr_BoardSlot : MonoBehaviour {
    public Image slotImg;
    public TextMeshProUGUI scoreTxt;

    public void changeSlot(Image img, int score) {
		slotImg.sprite = img.sprite;
		scoreTxt.text = "" + score;
	}
}