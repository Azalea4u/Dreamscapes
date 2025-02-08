using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_AI_itemName_Data", menuName = "Scriptable Objects/Archeology Item Data")]
public class SO_ArcheologyItem_Data : ScriptableObject
{
	public string Name;
	public Sprite sprite;
	public int points;
	public Vector2Int size = Vector2Int.one;

	/// <summary>
	/// List of grid positions that the item covers.<br/>
	/// Assumes that a value of (0,0) is the top left corner of the item.<br/>
	/// An empty list means that the item covers all tiles within its size.
	/// </summary>
	public List<Vector2Int> itemVolume;
}
