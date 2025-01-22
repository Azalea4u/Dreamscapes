using Unity.VisualScripting;
using UnityEngine;

public class SCR_SpaceGame_Manager : MonoBehaviour
{
    [SerializeField] private Transform spaceship;
    [SerializeField] private float shipPosition;
    [SerializeField] private float shipBounds;
    [SerializeField] private float shipMovementSpeed;
    [SerializeField] private float shipAscentSpeed;
    [SerializeField] private Transform MeanGuyTest;
    [SerializeField] private int health = 3;

	void Update()
    {
        transform.position = new Vector3(shipPosition, transform.position.y + (Time.deltaTime * shipAscentSpeed), 0.0f);

        MeanGuyTest.position -= new Vector3(0.0f, Time.deltaTime * shipAscentSpeed, 0.0f);
        if (MeanGuyTest.position.y <= transform.position.y - 7)
        {
            MeanGuyTest.position = new Vector3(Random.Range(-shipBounds, shipBounds), transform.position.y + 7.0f, 0.0f);
        }
    }

    public void MoveLeft()
    {
        shipPosition -= shipMovementSpeed * Time.deltaTime;
        shipPosition = Mathf.Clamp(shipPosition, -shipBounds, shipBounds);
    }

	public void MoveRight()
	{
		shipPosition += shipMovementSpeed * Time.deltaTime;
        shipPosition = Mathf.Clamp(shipPosition, -shipBounds, shipBounds);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
       // Debug.Log("Boombaby");
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        //Debug.Log("Goodbye");
	}
}
