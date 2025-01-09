using UnityEngine;

public class SCR_ArcheologyItem : MonoBehaviour
{
	[SerializeField] private SpriteRenderer sprite;
	public Vector2Int size = Vector2Int.one;
	public int position = 0;

	public void SetupItem()
	{
		sprite.transform.localScale = new Vector3(size.x, size.y, 1);
		sprite.transform.position += new Vector3(((float)size.x - 1) / 2, -((float)size.y - 1) / 2, 0);
	}
}
