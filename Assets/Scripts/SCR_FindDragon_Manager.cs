using UnityEngine;

public class SCR_FindDragon_Manager : MonoBehaviour
{
    public static SCR_FindDragon_Manager instance { get; private set; }

    [SerializeField] private GameObject[] dragons;
    [SerializeField] public Vector2 gameBounds;

	private void Start()
	{
		instance = this;
		SetupDragons();
	}

	private void SetupDragons()
    {
        foreach (var dragon in dragons)
        {
            dragon.transform.position = new Vector3(Random.Range(-gameBounds.x, gameBounds.x), Random.Range(-gameBounds.y, gameBounds.x), 0);
            
        }
        
        
    }
}
