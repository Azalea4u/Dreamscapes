using UnityEngine;

public class SCR_FindDragon_Dragon : MonoBehaviour
{
    public Vector2 speed = Vector2.one;
    public edgeType edgeInteraction = edgeType.BOUNCE;

    // helps with defining how the dragon should act when contacting an edge
    public enum edgeType
    {
        BOUNCE,
        SCROLL
    }

	private void Start()
	{
		speed = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f));
	}

	void Update()
    {
        transform.Translate(new Vector3(speed.x, speed.y) * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > SCR_FindDragon_Manager.instance.gameBounds.x)
        {
			switch (edgeInteraction)
			{
                case edgeType.BOUNCE:
                    speed.x *= -1;
                    break;
                case edgeType.SCROLL:
                    transform.position.Scale(new Vector3(-1, 1, 0));
                    break;
			}
		}

		if (Mathf.Abs(transform.position.y) > SCR_FindDragon_Manager.instance.gameBounds.y)
		{
			switch (edgeInteraction)
			{
				case edgeType.BOUNCE:
					speed.y *= -1;
					break;
				case edgeType.SCROLL:
					transform.position.Scale(new Vector3(1, -1, 0));
					break;
			}
		}
	}
}
