using UnityEngine;

public class Scr_Oscillator : MonoBehaviour
{
    [SerializeField] float rate = 1;
    [SerializeField] float amplitude = 1;
    [SerializeField] bool leftRight = false;
    float time = 0;
    Vector3 origin = Vector3.zero;
    private void Start()
    {
        origin = transform.position;
    }
    void Update()
    {
        time += Time.deltaTime * rate;
        float wave = Mathf.Sin(time) * amplitude;
        if (!leftRight) { transform.position = origin + (Vector3.up * wave); }
        else
        {
            transform.position = origin + (Vector3.forward * wave);
        }
    }
}
