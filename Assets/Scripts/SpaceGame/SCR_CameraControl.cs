using UnityEngine;

public class SCR_CameraControl : MonoBehaviour
{
    // Target the camera will follow
    public Transform target;

    // Background elements for infinite scrolling
    public Transform bg1;
    public Transform bg2;

    // Background height and camera's z-axis position
    private float size;
    private Vector3 cameraTargetPos = new Vector3();

    // Colliders for background detection
    private BoxCollider2D bg1Collider;
    private BoxCollider2D bg2Collider;
    private float cameraZPosition;
    private bool isRepositioning = false; // Prevents multiple reposition calls


	void Start()
    {
        // Initialize background colliders
        bg1Collider = bg1.GetComponent<BoxCollider2D>();
        bg2Collider = bg2.GetComponent<BoxCollider2D>();
        size = bg1Collider.size.y * bg1.localScale.y;
        cameraZPosition = transform.position.z;

        // Position second background exactly one height above the first
        RepositionBackground(bg2, bg1.position.y + size);
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

		// Smoothly move the camera to follow the target
		Vector3 velocity = Vector3.zero;
		Vector3 targetPos = SetPos(cameraTargetPos, transform.position.x, target.position.y, cameraZPosition);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 6.0f * Time.fixedDeltaTime / SCR_SpaceGame_Manager.instance.difficultyScale);

        // Handle infinite scrolling background
        if (!isRepositioning)
        {
            // Move bg1 above if camera moves above bg2
            if (transform.position.y > bg2.position.y)
            {
                isRepositioning = true;
                float newY = bg2.position.y + size;
                RepositionBackground(bg1, newY);
                SwitchBackground();
                isRepositioning = false;
            }
            // Move bg2 below if camera moves below bg1
            else if (transform.position.y < bg1.position.y)
            {
                isRepositioning = true;
                float newY = bg1.position.y - size;
                RepositionBackground(bg2, newY);
                SwitchBackground();
                isRepositioning = false;
            }
        }
    }

    // Moves the background to a new Y position
    private void RepositionBackground(Transform background, float newY)
    {
        Vector3 newPosition = background.position;
        newPosition.y = newY;
        background.position = newPosition;

        // Check and fix background overlap
        if (IsOverlapping())
        {
            Debug.LogWarning("Overlap detected! Adjusting position...");
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

    // Detects if the backgrounds overlap
    private bool IsOverlapping()
    {
        float bg1Top = bg1.position.y + (size / 2);
        float bg1Bottom = bg1.position.y - (size / 2);
        float bg2Top = bg2.position.y + (size / 2);
        float bg2Bottom = bg2.position.y - (size / 2);

        return (bg1Bottom < bg2Top && bg1Top > bg2Bottom) ||
               (bg2Bottom < bg1Top && bg2Top > bg1Bottom);
    }

    // Swaps bg1 and bg2 references for continuous scrolling
    private void SwitchBackground()
    {
        Transform temp = bg1;
        bg1 = bg2;
        bg2 = temp;

        BoxCollider2D tempCollider = bg1Collider;
        bg1Collider = bg2Collider;
        bg2Collider = tempCollider;
    }

    // Helper method to set a Vector3 position
    private Vector3 SetPos(Vector3 pos, float x, float y, float z)
    {
        pos.x = x;
        pos.y = y;
        pos.z = z;
        return pos;
    }
}
