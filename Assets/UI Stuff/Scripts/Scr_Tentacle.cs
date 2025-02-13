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

    public void UpdateGreyscale( float value)
    {
        spriteRenderer.material.SetFloat("_Strength", value);
		waterRenderer.material.SetFloat("_Strength", value);
	}

    public void DamageFlicker()
    {
		spriteRenderer.material.SetInt("_Flash", 1);
		waterRenderer.material.SetInt("_Flash", 1);
		StartCoroutine(Flicker());
	}

	private IEnumerator Flicker()
    {
		yield return new WaitForSeconds(Time.deltaTime * 6);
		spriteRenderer.material.SetInt("_Flash", 0);
		waterRenderer.material.SetInt("_Flash", 0);
	}

    void Die()
    {
        Destroy(gameObject);
    }
}
