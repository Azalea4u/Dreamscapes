using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Scr_LowhudBtn : MonoBehaviour {
    bool clicked = false;
    GameObject[] gameObjects;
    public List<RectTransform> rects;

	void Start() {
        
	}

    public void onClick() {
        clicked = !clicked;

        foreach (RectTransform r in rects) {

            if ( r!= this) {
                if (clicked) {
                    r.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                } else {
                    r.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
                    transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 75);
                }
			}
		}
    }
}