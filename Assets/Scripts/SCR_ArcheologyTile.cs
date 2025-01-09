using UnityEngine;
using UnityEngine.UI;

public class SCR_ArcheologyTile : MonoBehaviour
{
	[SerializeField] private Image sprite;
	public int position = 0;
	private int _depth = 6;
	public int depth { get { return _depth; } set { _depth = Mathf.Clamp(value,0,6); } }

	// testing version of the function for being hit by a tool
	public void TestHit(){
        if (depth <= 0)
        {
			return;
        }
        SCR_ArcheologyGrid.Instance.HitGrid(position);
	}

	// testing visual setup for the tile
	public void ChangeSprite(Sprite newsprite)
	{
		sprite.sprite = newsprite;
	}
}
