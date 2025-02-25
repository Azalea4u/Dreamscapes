using UnityEngine;

public class SCR_ButtonFlash : MonoBehaviour {
	private float timer = 1.25f;

	public void PointerEnter() {
		transform.localScale = new Vector2(1.04f, 1.04f);
	}

	public void PointerExit() {
		transform.localScale = new Vector2(1f, 1f);
	}
}
