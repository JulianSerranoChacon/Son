using UnityEngine;

public class E1 : MonoBehaviour
{
    private AudioSource As;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        As = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKey(KeyCode.Alpha0))
            As.volume += 0.5f * Time.deltaTime;
        else if ( Input.GetKey(KeyCode.Alpha1) ) 
            As.volume -= 0.5f * Time.deltaTime;
    }
}
