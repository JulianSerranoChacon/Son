using UnityEngine;

public class Ej3 : MonoBehaviour
{
    //TODO: Serializae field de array para los audios generators
    private AudioSource As0;
    private AudioSource As1;
    private bool fading;
    [SerializeField]
    private float time = 2;
    private float speed;
    int actual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        As0 = transform.GetChild(0).GetComponent<AudioSource>();
        As1 = transform.GetChild(1).GetComponent<AudioSource>();
        speed = 1 / time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.N) & !fading)
        {
            fading = true;
            actual = -actual;
        }
            

        if (fading)
        {
            As1.volume -= speed * Time.deltaTime * actual;
            As0.volume -= speed * Time.deltaTime * -actual;
        }
            

        if (fading && (As1.volume <= 0 || As0.volume <= 0))
        {
            fading = false;
            if (As1.volume <= 0)
            {
                As1.volume = 0;
                As0.volume = 1;
            }
            else if (As0.volume <= 0)
            {
                As0.volume = 0;
                As1.volume = 1;
            }
            
        }
    }
}
