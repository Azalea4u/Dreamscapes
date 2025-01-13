using System.Collections.Generic;
using UnityEngine;

public class SCR_FindDragon_Manager : MonoBehaviour
{
    public static SCR_FindDragon_Manager instance { get; private set; }

    [SerializeField] private SCR_FindDragon_Dragon[] dragons;
    [SerializeField] private Sprite[] dragonSprites;
    [SerializeField] public Vector2 gameBounds;
    //[SerializeField] private int difficulty;

    [SerializeField] private List<dragonGroup> dragonGroups = new List<dragonGroup>();

    // helper struct for organizing groups of dragons
    public struct dragonGroup
    {
        public Sprite sprite;
        public Vector2 speed;
        public bool copySpeed;
        public SCR_FindDragon_Dragon.edgeType edgeType;
    }

	private void Start()
	{
		instance = this;
        createDragonGroups();
		setupDragons();
	}

    private void createDragonGroups()
    {
        int start = Random.Range(0,dragonSprites.Length);
        for (int i = 0; i < 3; i++)
        {
			dragonGroup newgroup = new dragonGroup();
            newgroup.sprite = dragonSprites[(start + i)%dragonSprites.Length];
			newgroup.copySpeed = (Random.Range(0, 2)==0);
            if (newgroup.copySpeed)
            {
                newgroup.speed = new Vector2((Random.Range(0, 2) * 2 - 1) * Random.Range(1.0f, 2.0f), (Random.Range(0, 2) * 2 - 1) * Random.Range(1.0f, 2.0f));
            } else
            {
                newgroup.speed = new Vector2(Random.Range(1.0f, 2.0f), Random.Range(1.0f, 2.0f));
			}
            newgroup.edgeType = (SCR_FindDragon_Dragon.edgeType)Random.Range(0, 2);
            dragonGroups.Add(newgroup);
		}
    }

	private void setupDragons()
    {
        foreach (var dragon in dragons)
        {
            dragon.transform.position = new Vector3(Random.Range(-gameBounds.x, gameBounds.x), Random.Range(-gameBounds.y, gameBounds.x), 0);

            dragonGroup usedgroup = dragonGroups[Random.Range(0, dragonGroups.Count)];
            if (usedgroup.copySpeed)
            {
				dragon.speed = usedgroup.speed;
			} else
            {
                Vector2 s = usedgroup.speed;
                dragon.speed = new Vector2(Random.Range(-s.x, s.x), Random.Range(-s.y, s.y));
			}
            dragon.SetSprite(usedgroup.sprite);
            dragon.edgeInteraction = usedgroup.edgeType;
        }
    }
}
