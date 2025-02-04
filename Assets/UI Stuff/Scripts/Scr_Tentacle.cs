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

    }

    void Die()
    {
        Destroy(gameObject);
    }
}
