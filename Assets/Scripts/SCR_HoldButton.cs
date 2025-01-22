using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SCR_HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[Header("Button Hold Event")]
	public UnityEvent buttonHold;
	private bool held;

	private void Awake()
	{
		if (buttonHold == null) {
			buttonHold = new UnityEvent();
		}
	}

	private void Update()
	{
		if (held)
		{
			buttonHold.Invoke();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//Debug.Log("Down");
		held = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		//Debug.Log("Up");
		held = false;
	}
}
