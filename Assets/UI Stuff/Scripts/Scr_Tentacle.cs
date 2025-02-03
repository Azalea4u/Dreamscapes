using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Scr_Tentacle : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Animator anims;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anims.Play("TentacleAttack");
    }

    void Update()
    {
  //      if (timeExisting <= -0.5f)
  //      {
  //          return;
  //      }
        
  //      if (timeExisting < 0)
  //      {
  //          transform.position = new Vector3(transform.position.x, stopPos + 0.3f, 0.0f);
  //          timeExisting = -1;
		//	rb.linearVelocityY = 2.0f;
  //          Invoke("Die", 5.0f);
  //      }
  //      else
  //      {
		//    timeExisting -= Time.deltaTime;
		//}

  //      if (transform.position.y < stopPos)
  //      {
  //          rb.linearVelocityY = 0;
  //      }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
