using UnityEngine;
using UnityEngine.UI;

public class SCR_ArcheologyTile : MonoBehaviour
{
	[SerializeField] private Image sprite;
	public int position = 0;
	public int depth = 6;

	// testing the function for being hit by a tool
	public void TestHit(){
        if (depth <= 0)
        {
			return;
        }
        SCR_ArcheologyGrid.Instance.HitGrid(position);
		SetupVisuals();
	}

	// testing visual setup for the tile
	public void SetupVisuals()
	{
		switch (depth)
		{
			case 1:
				sprite.color = new Color(0, 0, 1, 0.5f);
				break;
			case 2:
				sprite.color = new Color(0, 0, 1, 1);
				break;
			case 3:
				sprite.color = new Color(0, 1, 0, 0.5f);
				break;
			case 4:
				sprite.color = new Color(0, 1, 0, 1);
				break;
			case 5:
				sprite.color = new Color(1, 0, 0, 0.5f);
				break;
			case 6:
				sprite.color = new Color(1, 0, 0, 1);
				break;
			default:
				sprite.color = new Color(0, 0, 0, 0);
				break;
		}
	}
}
