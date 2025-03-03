using UnityEngine;

public class Toggle : MonoBehaviour {
    [SerializeField] GameObject toggleObject;

    public void toggle() {
        toggleObject.SetActive(!toggleObject.activeSelf);   
    }
}