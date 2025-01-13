using UnityEngine;

public class SCR_FindDragon_Dragon : MonoBehaviour
{
    public Vector2 speed = Vector2.one;
    public edgeType edgeInteraction = edgeType.BOUNCE;
	[SerializeField] private SpriteRenderer sprite;

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
        transform.Translate(new Vector3(speed.x, speed.y) * Time.deltaTime);

		switch (edgeInteraction)
		{
			case edgeType.BOUNCE:
				if (Mathf.Abs(transform.position.x) > SCR_FindDragon_Manager.instance.gameBounds.x)
				{
					speed.x *= -1;
				}
				if (Mathf.Abs(transform.position.y) > SCR_FindDragon_Manager.instance.gameBounds.y)
				{
					speed.y *= -1;
				}
				break;
			case edgeType.SCROLL:
				if (transform.position.x > SCR_FindDragon_Manager.instance.gameBounds.x)
				{
					transform.position -= new Vector3(SCR_FindDragon_Manager.instance.gameBounds.x * 2.0f, 0, 0);
				}
				if (transform.position.x < -SCR_FindDragon_Manager.instance.gameBounds.x)
				{
					transform.position += new Vector3(SCR_FindDragon_Manager.instance.gameBounds.x * 2.0f, 0, 0);
				}
				if (transform.position.y > SCR_FindDragon_Manager.instance.gameBounds.y)
				{
					transform.position -= new Vector3(0, SCR_FindDragon_Manager.instance.gameBounds.y * 2.0f, 0);
				}
				if (transform.position.y < -SCR_FindDragon_Manager.instance.gameBounds.y)
				{
					transform.position += new Vector3(0, SCR_FindDragon_Manager.instance.gameBounds.y * 2.0f, 0);
				}
				break;
		}
	}

	public void SetSprite(Sprite sprite)
	{
		this.sprite.sprite = sprite;
	}
}
