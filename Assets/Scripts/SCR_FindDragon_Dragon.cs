using UnityEngine;
using UnityEngine.UI;

public class SCR_FindDragon_Dragon : MonoBehaviour
{
    public Vector2 speed = Vector2.one;
    public edgeType edgeInteraction = edgeType.BOUNCE;
	[SerializeField] private Image sprite;
	public bool isWanted = false;

    // helps with defining how the dragon should act when contacting an edge
    public enum edgeType
    {
        BOUNCE = 0,
        SCROLL = 1
    }

	private void Start()
	{
		//speed = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
		//edgeInteraction = (edgeType)Random.Range(0, 2);
	}

	void Update()
    {
		if (speed == Vector2.zero)
		{
			return;
		}

        transform.Translate(new Vector3(speed.x, speed.y) * Time.deltaTime);

		Vector4 bounds = SCR_FindDragon_Manager.instance.gameBounds;

		switch (edgeInteraction)
		{
			case edgeType.BOUNCE:
				if (transform.position.x < bounds.x || transform.position.x > bounds.z)
				{
					speed.x *= -1;
				}
				if (transform.position.y < bounds.y || transform.position.y > bounds.w)
				{
					speed.y *= -1;
				}
				break;
			case edgeType.SCROLL:
				if (transform.position.x < bounds.x)
				{
					transform.position += new Vector3(bounds.x * 2.0f, 0, 0);
				}
				if (transform.position.x > bounds.z)
				{
					transform.position -= new Vector3(bounds.z * 2.0f, 0, 0);
				}
				if (transform.position.y > bounds.y)
				{
					transform.position -= new Vector3(0, bounds.y * 2.0f, 0);
				}
				if (transform.position.y < bounds.w)
				{
					transform.position += new Vector3(0, bounds.w * 2.0f, 0);
				}
				break;
		}
	}

	public void SetSprite(Sprite sprite)
	{
		this.sprite.sprite = sprite;
	}

	public void DragonPressed()
	{
		SCR_FindDragon_Manager.instance.DragonPressed(isWanted, this);
	}
}
