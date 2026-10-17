
// '1' empieza la reproducción del sonido intermitente
// '2' lo para

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class IntermitentSound : MonoBehaviour {
    [SerializeField]
    int polifonia;
    [SerializeField]
    private AudioSource[] _Speaker;  // audio source asosicada a la entidad
    [SerializeField]
    private float[] timeStamps;  // audio source asosicada a la entidad
    [Range(0f, 1f )]
    public float minVol, maxVol, SourceVol;  // volumenes máximo y mínimo establecidos y volumen origintal del source
    [Range(0f, 30f )]
    public float minTime, maxTime;  // intervalo temporal de lanzamiento
    [Range(0, 50)]
    public int distRand, maxDist;   // 
    [Range(0f, 1.1f )]
    public float spatialBlend;
    public AudioClip[] pcmData;
    public bool enablePlayMode = false;

    [Serialize]
    bool oldest = true;

    [Range(-1f, 1f )]
    public float minPan, maxPan, SourcePan;  // volumenes máximo y mínimo establecidos y volumen origintal del source

    [Range(0f, 2f )]
    public float PitchVar;  // volumenes máximo y mínimo establecidos y volumen origintal del source

    void Awake()
    {
        _Speaker =  new AudioSource[polifonia];
        timeStamps =  new float[polifonia];

        for(int i = 0; i < polifonia; i++)
        { 
            GameObject obj = new GameObject("Speaker " + i);
            obj.transform.SetParent(transform);
            AudioSource speaker = obj.AddComponent<AudioSource>();
            speaker.playOnAwake = false;
            speaker.loop = false;
            speaker.volume = 0;

            _Speaker[i] = speaker;
            timeStamps[i] = 0;
        }
    }

    void Start() {
        foreach( AudioSource speaker in _Speaker)
        {
            speaker.playOnAwake = false;
            speaker.loop = false;
            speaker.volume = 0.1f;
        }
    }

    // Update is called once per frame
    void Update() {
        if (!enablePlayMode){
            Debug.Log("NotPlaying");
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                enablePlayMode = true;
                StartCoroutine("Waitforit");
            }
        } else if (enablePlayMode) 
            if (Input.GetKeyDown(KeyCode.Alpha2)) StopSound();            
    }

    IEnumerator Waitforit() {
        // tiempo de espera aleatorio en el intervalo [minTime,maxTime]
        float waitTime = UnityEngine.Random.Range(minTime, maxTime);
        Debug.Log(waitTime);

        // miramos si hay un clip asignado al source (sirve para la primera vez q se ejecuta)
        yield return new WaitForSeconds(waitTime);    
        
        // si esta activado reproducimos sonido
        if (enablePlayMode) PlaySound();        
    }

void PlaySound() {
    
        
        int x = -1;
        int i = 0; 
        float min = float.MaxValue;
        bool fin = false;

        while(!fin && i < timeStamps.Length)
        {
            if (!_Speaker[i].isPlaying)
            {
                fin = true;
                x = i;
            }
            else if(oldest && timeStamps[i] < min)
            {
                x = i;
                min = timeStamps[i];
            }

            i++;
        }

        if(x >= 0)
        {        
            AudioSource current = _Speaker[x];
            timeStamps[x] = Time.time;
            
            SetSourceProperties(current, pcmData[UnityEngine.Random.Range(0, pcmData.Length)], minVol, maxVol, distRand, maxDist, spatialBlend);
            current.Play();
            Debug.Log("back in it");
        }

        StartCoroutine("Waitforit");
    }

public void SetSourceProperties(AudioSource target, AudioClip audioData, float minVol, float maxVol,
                                    int minDist, int maxDist, float SpatialBlend) {
        target.loop = false;
        target.maxDistance = maxDist - UnityEngine.Random.Range(0f, distRand);
        target.spatialBlend = spatialBlend;
        target.clip = audioData;
        target.volume = SourceVol + UnityEngine.Random.Range(minVol, maxVol);
        target.panStereo = SourcePan + UnityEngine.Random.Range(minPan, maxPan);
        target.pitch =  UnityEngine.Random.Range(1 - PitchVar, 1+ PitchVar);
    }


    void StopSound() {
        enablePlayMode = false;
        Debug.Log("stop");
    }
}    
    
