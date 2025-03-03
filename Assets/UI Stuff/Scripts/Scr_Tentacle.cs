using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Scr_Tentacle : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] Animator anims;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject water;
    [SerializeField] SpriteRenderer waterRenderer;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();

		// all motions that happen are entirely controlled by the animation player
        anims.Play("TentacleAttack");
    }


	// These functions are to align the greyscale and flashing with the real Poly
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

	public void DamageDark()
	{
		spriteRenderer.material.SetInt("_Dark", 1);
		waterRenderer.material.SetInt("_Dark", 1);
		StartCoroutine(Dark());
	}

	private IEnumerator Dark()
	{
		yield return new WaitForSeconds(Time.deltaTime * 6);
		spriteRenderer.material.SetInt("_Dark", 0);
		waterRenderer.material.SetInt("_Dark", 0);
	}

	void Die()
    {
        Destroy(gameObject);
    }
}
