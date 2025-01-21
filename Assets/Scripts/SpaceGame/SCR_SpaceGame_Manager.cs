using Unity.VisualScripting;
using UnityEngine;

public class SCR_SpaceGame_Manager : MonoBehaviour
{
    [SerializeField] private Transform spaceship;
    [SerializeField] private float shipPosition;
    [SerializeField] private float shipSpeed;
    [SerializeField] private Transform MeanGuyTest;
    [SerializeField] private float catchup;

	void Update()
    {
        catchup = Mathf.Lerp(catchup, 0.0f, Time.deltaTime);
        float ypos = -catchup;
        spaceship.position = new Vector3(shipPosition, Mathf.Lerp(spaceship.position.y, ypos, Time.deltaTime), 0.0f);
        MeanGuyTest.position -= new Vector3(0.0f, Time.deltaTime * (shipSpeed - catchup) , 0.0f);
        if (MeanGuyTest.position.y <= -7)
        {
            MeanGuyTest.position = new Vector3(0.0f, 7.0f, 0.0f);
        }
    }

    public void MoveLeft()
    {
        shipPosition -= shipSpeed * Time.deltaTime;
    }

	public void MoveRight()
	{
		shipPosition += shipSpeed * Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        Debug.Log("Boombaby");
        catchup += 10.0f;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        Debug.Log("Goodbye");
	}
}
