using Unity.Collections;
using UnityEngine;

public class SCR_Archeology_Tool : MonoBehaviour
{
    [SerializeField] Animator animator;
    
    public void StartMining(Vector3 position)
    {
        animator.Play("Idle");
        animator.SetTrigger("Mine");
        transform.position = position;
    }
    
    public void HitTile()
    {
        SCR_ArcheologyGrid.Instance.HitTile();
    }

    public void DoneMining()
    {
        animator.ResetTrigger("Mine");
        transform.position = Vector3.one * 10;
    }
}
