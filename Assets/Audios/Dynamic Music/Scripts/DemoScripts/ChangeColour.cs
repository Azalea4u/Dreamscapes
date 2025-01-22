using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    public Camera cam;
    public float fadeSpeed = 0.01f;
    Vector3 currentColour;
    Color targetColor;

    private void Start()
    {
        currentColour = ColourToVector3(cam.backgroundColor);
        targetColor = RandomColour();
    }

    void Update()
    {
        if(cam.backgroundColor != targetColor)
        {
            currentColour = Vector3.MoveTowards(currentColour, ColourToVector3(targetColor), fadeSpeed * Time.deltaTime);
            cam.backgroundColor = Vector3ToColour(currentColour);
        }
        else {
            targetColor = RandomColour();
        }

    }

    Vector3 ColourToVector3(Color colour)
    {
        return new Vector3 (colour.r,colour.g,colour.b);
    }

    Color Vector3ToColour(Vector3 vector3)
    {
        return new Color(vector3.x,vector3.y,vector3.z);
    }

    Color RandomColour ()
    {
        float maxSat = 0.5f;
        return new Color(Random.Range(0, maxSat), Random.Range(0, maxSat), Random.Range(0, maxSat));
    }
}
