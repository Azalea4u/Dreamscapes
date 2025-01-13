using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_AI_itemName_Data", menuName = "Scriptable Objects/Archeology Item Data")]
public class SO_ArcheologyItem_Data : ScriptableObject
{
	public Sprite sprite;
	public Vector2Int size = Vector2Int.one;
	public List<Vector2Int> Volume;
}
