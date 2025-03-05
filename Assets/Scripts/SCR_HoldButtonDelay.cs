using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SCR_HoldButtonDelay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	[Header("Button Hold Event")]
	public UnityEvent buttonHold;
	private bool held;
	private float delay = 0.3f;
	private float time;

	private void Awake() {
		if (buttonHold == null) {
			buttonHold = new UnityEvent();
		}
	}

	private void Update() {
		if (held) {
			time += Time.deltaTime;

			if (time >= delay) {
				buttonHold.Invoke();
				time = 0;
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData) {
		held = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		held = false;
		time = 0;
	}
}