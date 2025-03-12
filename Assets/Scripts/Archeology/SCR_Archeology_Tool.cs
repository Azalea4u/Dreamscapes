using Unity.Collections;
using UnityEngine;

public class SCR_Archeology_Tool : MonoBehaviour
{
    [SerializeField] Animator animator;
    
    /// <summary>
    /// Starts the mining process
    /// </summary>
    /// <param name="position">Where the tool should be placed</param>
    public void StartMining(Vector3 position)
    {
        // Idle is played because it forces the mining animation to finish before trying to mine again
        // It also makes it feel better than if I dont
        animator.Play("Idle");
        animator.SetTrigger("Mine");
        transform.position = position;
    }
    
    /// <summary>
    /// Called during mining animation
    /// </summary>
    public void HitTile()
    {
        SCR_ArcheologyGrid.Instance.HitTile();
    }

    /// <summary>
    /// Called during mining animation<br/>
    /// The function resets the tool to a default state
    /// </summary>
    public void DoneMining()
    {
        animator.ResetTrigger("Mine");
        transform.position = Vector3.one * 10;
    }
}
