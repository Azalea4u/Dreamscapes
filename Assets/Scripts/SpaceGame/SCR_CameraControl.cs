using UnityEngine;

public class SCR_CameraControl : MonoBehaviour
{
    public Transform target;

    public Transform bg1;
    public Transform bg2;

    private float size;

    void Start()
    {
        size = bg1.GetComponent<BoxCollider2D>().size.y;
    }

    void FixedUpdate()
    {
        // CAMERA
        Vector3 targetPos = new Vector3(transform.position.x, target.position.y, 0);
        target.position = Vector3.Lerp(transform.position, targetPos, 0.2f);

        // BACKGROUND
        if (transform.position.y >= bg2.position.y)
        {
            bg1.position = new Vector3(bg1.position.x, bg2.position.y + size , bg1.position.z);
            SwitchBackground();
        }
    }

    private void SwitchBackground()
    {
        Transform temp = bg1;
        bg1 = bg2;
        bg2 = temp;
    }
}
