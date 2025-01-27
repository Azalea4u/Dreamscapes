using System.Collections.Generic;
using UnityEngine;

public class SCR_FindDragon_Manager : MonoBehaviour
{
    public static SCR_FindDragon_Manager instance { get; private set; }

    [SerializeField] private SCR_FindDragon_Dragon[] dragons;
    [SerializeField] private Sprite[] dragonSprites;
    [SerializeField] private Vector2 _gamebounds;
    
    // using a vector 4 because the bound values of the walls could all be different
    /// <summary>
    /// x = left bound
    /// y = top bound
    /// z = right bound
    /// w = bottom bound
    /// </summary>
    [SerializeField] public Vector4 gameBounds { 
        get { 
            return new Vector4(-_gamebounds.x + transform.position.x, _gamebounds.y + transform.position.y, _gamebounds.x + transform.position.x, -_gamebounds.y + transform.position.y); 
        } 
    }
    
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
        for (int i = 0; i < 4; i++)
        {
			dragonGroup newgroup = new dragonGroup();
            newgroup.sprite = dragonSprites[(start + i)%dragonSprites.Length];
			newgroup.copySpeed = (Random.Range(0, 2)==0);
			// (Random.Range(0, 2) * 2 - 1) gets a random number of -1 or 1
		    newgroup.speed = new Vector2((Random.Range(0, 2) * 2 - 1) * Random.Range(1.0f, 2.0f), (Random.Range(0, 2) * 2 - 1) * Random.Range(1.0f, 2.0f));
            newgroup.edgeType = (SCR_FindDragon_Dragon.edgeType)Random.Range(0, 2);
            dragonGroups.Add(newgroup);
		}
    }

	private void setupDragons()
    {
        foreach (var dragon in dragons)
        {
            dragon.transform.position = new Vector3(Random.Range(gameBounds.x, gameBounds.z), Random.Range(gameBounds.y, gameBounds.w), 0);
            assignDragonGroup(dragon, Random.Range(1, dragonGroups.Count));
		}

        assignDragonGroup(dragons[0], 0);
    }

    private void assignDragonGroup(SCR_FindDragon_Dragon dragon, int group)
    {
		dragonGroup usedgroup = dragonGroups[group];
		if (usedgroup.copySpeed)
		{
			dragon.speed = usedgroup.speed;
		}
		else
		{
			Vector2 s = usedgroup.speed;
			dragon.speed = new Vector2(Random.Range(-s.x, s.x), Random.Range(-s.y, s.y));
		}
		dragon.SetSprite(usedgroup.sprite);
		dragon.edgeInteraction = usedgroup.edgeType;
        dragon.isWanted = (group == 0);
	}

    public void DragonPressed(bool isWanted, SCR_FindDragon_Dragon dragon)
    {

        if (isWanted)
        {
            foreach (var drag in dragons)
            {
				drag.speed = Vector2.zero;
				if (drag != dragon)
                {
					drag.transform.position = Vector3.one * 10;
				}
            }

            // would put here stuff I want to happen after the dragon is found

        }
        else
        {
            dragon.speed = Vector2.zero;
            dragon.transform.position = Vector3.one * 10;
        }
    }
}
