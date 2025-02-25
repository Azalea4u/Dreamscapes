using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SCR_ButtonGrowCoroutine : MonoBehaviour {
	public Button topLeft, topRight, bottomRight, bottomLeft;
	public float growTime = 0.3f;
	public float waitTimer = 1f;
	public float scaleMultiplier = 1.1f;

	private Button[] buttons;
	private Vector3 originalScale;

	private void Start() {
		buttons = new Button[] { topLeft, topRight, bottomRight, bottomLeft };
		if (buttons.Length > 0) originalScale = buttons[0].transform.localScale;
		StartCoroutine(GrowButtons());
		Debug.Log("Started coroutine");
	}

	IEnumerator GrowButtons() {
		int index = 0;

		while (true) {
			Button currentButton = buttons[index];
			Transform currentTransform = currentButton.transform;

			yield return ScaleButton(currentTransform, originalScale * scaleMultiplier);

			yield return new WaitForSeconds(waitTimer);

			yield return ScaleButton(currentTransform, originalScale);

			index = (index + 1) % buttons.Length;
		}
	}

	IEnumerator ScaleButton(Transform buttonTransform, Vector3 targetScale) {
		Vector3 startScale = buttonTransform.localScale;
		float elapsedTime = 0f;

		while (elapsedTime < growTime) {
			buttonTransform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / growTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		buttonTransform.localScale = targetScale;
	}
}
