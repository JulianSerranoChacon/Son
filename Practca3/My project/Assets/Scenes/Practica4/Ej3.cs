using UnityEngine;
using System.Collections;
using UnityEngine.PlayerLoop;
using Unity.VisualScripting;

public class Ej3 : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] AS;

    [SerializeField]
    private float time = 2;

    int actual;

    private AudioSource ASActual;

    [SerializeField]
    AudioClip[] AC;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actual = 0;

        if(AS.Length > 0)
        {
            int clip = Random.Range(0,AC.Length);
            if(AC.Length > 0)
                AS[0].generator = AC[clip];

            ASActual = AS[0];
            
            ASActual.volume = 1;
            ASActual.Play();
        }

        for(int i = 1; i < AS.Length; i++)
        {
            AS[i].Stop();
            AS[i].volume = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!AudioSourceExtensions.isFading() && Input.GetKeyDown(KeyCode.N))
        {
            int aux = Random.Range(0,AS.Length);

            if(aux == actual)
                aux = (aux + 1) % AS.Length;

            actual = aux;

                        
            int clip = Random.Range(0,AC.Length);
            if(AC[clip] == ASActual.clip)
            {
                clip = (clip + 1) % AC.Length;
            }

            StartCoroutine(AudioSourceExtensions.FadeOut(ASActual, time));
            ASActual = AS[actual];
            ASActual.generator = AC[clip];
            StartCoroutine(AudioSourceExtensions.FadeIn(ASActual, time));

        }
    }
}

public static class AudioSourceExtensions
{
    private static bool onFadeIn = false;
    private static bool onFadeOut = false;

    public static bool isFading()
    {
        return onFadeIn || onFadeOut;
    }

    public static IEnumerator FadeOut(AudioSource a, float duration) {
        onFadeOut = true;
        float startVolume = a.volume;

        while (a.volume > 0)
        {
            a.volume -= startVolume * Time.deltaTime / duration;
            yield return new WaitForEndOfFrame();
        }

        a.Stop();
        onFadeOut = false;
    }

     public static IEnumerator FadeIn(AudioSource a, float duration) {
        onFadeIn = true;
        a.volume = 0;
        a.Play();

        while (a.volume < 1)
        {
            a.volume += Time.deltaTime / duration;
            yield return new WaitForEndOfFrame();
        }

        a.volume = 1;
        onFadeIn = false;
    }
}

