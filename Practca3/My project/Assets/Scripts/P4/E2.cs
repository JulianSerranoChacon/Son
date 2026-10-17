using UnityEngine;

public class E2 : MonoBehaviour
{
    private AudioSource As;
    private bool fading;
    [SerializeField]
    private float time = 2;
    private float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        As = GetComponent<AudioSource>();
        speed = 1 / time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
            fading = true;

        if(fading)
            As.volume -= speed * Time.deltaTime;

        if(fading && As.volume<= 0)
        {
            fading = false;
            As.volume = 0;
        }
    }
}
