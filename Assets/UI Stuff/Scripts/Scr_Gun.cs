using UnityEngine;

public class Scr_Gun : MonoBehaviour {
    [SerializeField] int dmg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        int r = Random.Range(0, 2);

        switch (r) {
            case 1:
                ]]
break;
             case 2:
                break
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Scr_OctoPlayer>()) {
            collision.gameObject.GetComponent<Scr_OctoPlayer>().damage(dmg);
		}
	}
}
