using UnityEngine;
using UnityEngine.EventSystems;

public class SCR_FindDragon_Dragon : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] private SpriteRenderer sprite;
	[SerializeField] private Animator animator;
    public Vector2 speed = Vector2.one;
    public edgeType edgeInteraction = edgeType.BOUNCE;
	public bool isWanted = false;

	public bool active = true;

    // helps with defining how the dragon should act when contacting an edge
    public enum edgeType
    {
        BOUNCE = 0,
        SCROLL = 1
    }

	void Update()
	{
		if (!active)
		{
			return;
		}

		transform.Translate(new Vector3(speed.x, speed.y) * Time.deltaTime);

		Vector4 bounds = SCR_FindDragon_Manager.instance.gameBounds;

		switch (edgeInteraction)
		{
			case edgeType.BOUNCE:
				if (transform.position.x < bounds.x)
				{
					speed.x *= -1;
					transform.position = new Vector3(bounds.x, transform.position.y, transform.position.z);
				}
				if (transform.position.x > bounds.z)
				{
					speed.x *= -1;
					transform.position = new Vector3(bounds.z, transform.position.y, transform.position.z);
				}
				if (transform.position.y > bounds.y)
				{
					speed.y *= -1;
					transform.position = new Vector3(transform.position.x, bounds.y, transform.position.z);
				}
				if (transform.position.y < bounds.w)
				{
					speed.y *= -1;
					transform.position = new Vector3(transform.position.x, bounds.w, transform.position.z);
				}
				break;
			case edgeType.SCROLL:
				if (transform.position.x < bounds.x)
				{
					transform.position = new Vector3(bounds.z, transform.position.y, transform.position.z);
				}
				if (transform.position.x > bounds.z)
				{
					transform.position = new Vector3(bounds.x, transform.position.y, transform.position.z);
				}
				if (transform.position.y > bounds.y)
				{
					transform.position = new Vector3(transform.position.x, bounds.w, transform.position.z);
				}
				if (transform.position.y < bounds.w)
				{
					transform.position = new Vector3(transform.position.x, bounds.y, transform.position.z);
				}
				break;
		}
	}

	public void SetSprite(Sprite sprite)
	{
		this.sprite.sprite = sprite;
	}

	public void DeactivateDragon()
	{
		active = false;
		speed = Vector2.zero;
		if (!isWanted)
		{
			transform.position = Vector3.one * 10;
			//animator.SetTrigger("NotWanted");
		} else
		{
			animator.SetTrigger("Wanted");
		}
	}

	public void ActivateDragon()
	{
		active = true;
		animator.SetTrigger("Reset");
	}

	public void SetWanted(bool wanted)
	{
		isWanted = wanted;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (active)
		{
			SCR_FindDragon_Manager.instance.DragonPressed(isWanted, this);
		}
	}
}
