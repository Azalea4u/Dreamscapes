using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scr_BoardSlot : MonoBehaviour {
    [SerializeField] Image slotImg;
    [SerializeField] TextMeshProUGUI scoreTxt;

    public void changeSlot(Image img, int score) {
		slotImg = img;
		scoreTxt.text = "" + score;
	}
}