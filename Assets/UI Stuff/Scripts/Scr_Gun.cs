using UnityEngine;

public class Scr_Gun : MonoBehaviour
{
    [SerializeField] int dmg;
    [SerializeField] float lastAmount;

    void Start()
    {
        float r = Random.Range((float)-1.5, (float)1.5);

        Vector3 v = transform.position;
        v.x = r;
        transform.position = v;
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Scr_OctoPlayer>()) {
            return;
		}
	}
}
