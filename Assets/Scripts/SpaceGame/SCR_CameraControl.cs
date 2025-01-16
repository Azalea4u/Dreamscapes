using UnityEngine;

public class SCR_CameraControl : MonoBehaviour
{
    public Transform target;
    public Transform bg1;
    public Transform bg2;

    private float size;
    private Vector3 cameraTargetPos = new Vector3();
    private Vector3 bg1_targetPos = new Vector3();
    private Vector3 bg2_targetPos = new Vector3();

    private BoxCollider2D bg1Collider;
    private BoxCollider2D bg2Collider;
    private float cameraZPosition;
    private bool isRepositioning = false;

    void Start()
    {
        bg1Collider = bg1.GetComponent<BoxCollider2D>();
        bg2Collider = bg2.GetComponent<BoxCollider2D>();
        size = bg1Collider.size.y;
        cameraZPosition = transform.position.z;

        // Position second background exactly one height above the first
        RepositionBackground(bg2, bg1.position.y + size);
    }

    void FixedUpdate()
    {
        // Camera movement
        Vector3 targetPos = SetPos(cameraTargetPos, transform.position.x, target.position.y, cameraZPosition);
        transform.position = Vector3.Lerp(transform.position, targetPos, 0.2f);

        if (!isRepositioning)
        {
            float halfSize = size / 2;

            if (transform.position.y > bg2.position.y + halfSize)
            {
                isRepositioning = true;
                float newY = bg2.position.y + size * 2;
                RepositionBackground(bg1, newY);
                SwitchBackground();
                isRepositioning = false;
            }
            else if (transform.position.y < bg1.position.y - halfSize)
            {
                isRepositioning = true;
                float newY = bg1.position.y - size * 2;
                RepositionBackground(bg2, newY);
                SwitchBackground();
                isRepositioning = false;
            }
        }
    }

    private void RepositionBackground(Transform background, float newY)
    {
        // Keep X and Z positions the same, only update Y
        Vector3 newPosition = background.position;
        newPosition.y = newY;
        background.position = newPosition;

        // Verify no overlap occurred
        if (IsOverlapping())
        {
            Debug.LogWarning("Overlap detected! Adjusting position...");
            // If overlap occurred, force correct positioning
            if (background == bg1)
            {
                newPosition.y = bg2.position.y + size;
            }
            else
            {
                newPosition.y = bg1.position.y - size;
            }
            background.position = newPosition;
        }
    }

    private bool IsOverlapping()
    {
        float bg1Top = bg1.position.y + (size / 2);
        float bg1Bottom = bg1.position.y - (size / 2);
        float bg2Top = bg2.position.y + (size / 2);
        float bg2Bottom = bg2.position.y - (size / 2);

        // Check if either background overlaps the other
        return (bg1Bottom < bg2Top && bg1Top > bg2Bottom) ||
               (bg2Bottom < bg1Top && bg2Top > bg1Bottom);
    }

    private void SwitchBackground()
    {
        Transform temp = bg1;
        bg1 = bg2;
        bg2 = temp;

        BoxCollider2D tempCollider = bg1Collider;
        bg1Collider = bg2Collider;
        bg2Collider = tempCollider;
    }

    private Vector3 SetPos(Vector3 pos, float x, float y, float z)
    {
        pos.x = x;
        pos.y = y;
        pos.z = z;
        return pos;
    }
}
