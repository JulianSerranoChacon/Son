using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice2: MonoBehaviour {
    private AudioSource head;
    private AudioSource tail;
    private AudioSource casing;
    private AudioSource loop;
    
    private AudioReverbZone reverb;

    [SerializeField]
    private AudioReverbPreset reverbPreset;

    public AudioClip[] pcmDataHeads, pcmDataTails, pcmDataCasing ,pcmDataLoop;
    private int nHeads, nTails, nCasing,nLoop;

    public float overlapTime=0.01f;
    private int overlapSamples;
    private int sRATE;

    [Range(0f, 1f )]
    public float minVol, maxVol, SourceVol;  // volumenes máximo y mínimo establecidos y volumen origintal del source

    [Range(0f, 2f )]
    public float PitchVar;  // volumenes máximo y mínimo establecidos y volumen origintal del source

    public float timeBetweenLoops;
    private float currentTimeBetweenLoops;

    bool loopB = false;

    void Awake(){
        Debug.Log("Awake");
        sRATE = AudioSettings.outputSampleRate;
        nHeads = pcmDataHeads.Length;
        nTails = pcmDataTails.Length;
        nCasing = pcmDataCasing.Length;
        nLoop = pcmDataLoop.Length;

        head = gameObject.AddComponent<AudioSource>();        
        tail = gameObject.AddComponent<AudioSource>();   
        casing = gameObject.AddComponent<AudioSource>();     
        loop = gameObject.AddComponent<AudioSource>();  

        reverb = gameObject.AddComponent<AudioReverbZone>();
        reverb.reverbPreset = reverbPreset;

        //  comprobar que son mono
        for (int i=0; i<nHeads; i++) {
          if (pcmDataHeads[i].channels!=1) throw new System.Exception("Tienen que ser muestras mono!!");
        }
        for (int i=0; i<nTails; i++) {
          if (pcmDataTails[i].channels!=1) throw new System.Exception("Tienen que ser muestras mono!!");
        }
        for(int i=0; i<nCasing; i++) {
          if (pcmDataCasing[i].channels!=1) throw new System.Exception("Tienen que ser muestras mono!!");
        }
        for(int i=0; i<nLoop; i++) {
          if (pcmDataLoop[i].channels!=1) throw new System.Exception("Tienen que ser muestras mono!!");
        }

        // convertimos tiempo de solapamiento a samples
        overlapSamples = (int) Mathf.Round(sRATE*overlapTime);

        currentTimeBetweenLoops = timeBetweenLoops;
    }

    void Start(){
        Debug.Log("Start");
        // los fades se aplican sobre el clip original, el pitch se aplica luego. 
        // No hay que tenerlo en cuenta para aplicar los fades
        for (int i=0; i<nHeads; i++) FadeOut(pcmDataHeads[i]);
        for (int i=0; i<nTails; i++) FadeOut(pcmDataTails[i]);
        for (int i=0; i<nTails; i++) FadeIn(pcmDataTails[i]);
        for (int i=0; i<nTails; i++) FadeIn(pcmDataCasing[i]);

        
        for (int i=0; i<nLoop; i++) FadeOut(pcmDataLoop[i]);
        for (int i=0; i<nLoop; i++) FadeIn(pcmDataLoop[i]);
    }


    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            int h = Random.Range(0, nHeads), t = Random.Range(0, nTails), c = Random.Range(0, nCasing);
            head.clip = pcmDataHeads[h];
            tail.clip = pcmDataTails[t];
            casing.clip = pcmDataCasing[c];

            //OPCIONAL: aleatorizamos el volumen y el ppitch
            SetSourceProperties(head);
            SetSourceProperties(tail);
            SetSourceProperties(casing);
            
            double clipLength = ((head.clip.samples-overlapSamples) / head.pitch); 
            double tailLength = ((head.clip.samples-overlapSamples)/ tail.pitch);       
            Debug.Log($"head {h} p tail {t}  sRATE: {sRATE}     lengthHead {clipLength}   overlapSamples {overlapSamples}");

            head.Play();
            // tail se reproduce solapado con head en el fragmento determinado por overlapSamples
            tail.PlayScheduled(AudioSettings.dspTime+(clipLength+overlapSamples)/sRATE);

            casing.PlayScheduled(AudioSettings.dspTime+(tailLength+overlapSamples)/sRATE);
        }
        else if (Input.GetKey(KeyCode.Alpha2)) {
            
            if(timeBetweenLoops <= currentTimeBetweenLoops)
            { 
                int l = Random.Range(0, nLoop), c = Random.Range(0, nCasing);
                loop.clip = pcmDataLoop[l];
                casing.clip = pcmDataCasing[c];

                
                SetSourceProperties(casing);

                //OPCIONAL: aleatorizamos el volumen y el ppitch
                SetSourceProperties(loop);
                double loopLength = ((loop.clip.samples-overlapSamples)/ loop.pitch);       
                loop.Play();
                casing.PlayScheduled(AudioSettings.dspTime+(loopLength+overlapSamples)/sRATE);
                currentTimeBetweenLoops = 0;
            }
            else
            {
                currentTimeBetweenLoops += Time.deltaTime;
            }
           
        }
         else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            currentTimeBetweenLoops = timeBetweenLoops;
        }
    }

    //  crossFade out en [0,overlapTime]   sqrt((overlapTime-t)/overlapTime)  
    void FadeOut(AudioClip clip){
        // pasamos clip a un array de samples
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        // ini: sample inicial donde aplicar fadeOut
        int ini=samples.Length-overlapSamples;
        for (int i=0; i<overlapSamples; i++){
            // mutiplicamos sample por valor de fadeOut. 
            // hay que convertir sample i a tiempo t = i/SRATE (regla de 3)
            samples[ini+i] *= Mathf.Sqrt((overlapTime-(i/sRATE))/overlapTime);
        }

        // volcamos array al clip
        clip.SetData(samples, 0);    
    }


    //  crossFadeIn en [0,overlapTime]   sqrt(t/overlapTime)  
    void FadeIn(AudioClip clip){
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);
        
        for (int i=0; i<overlapSamples; i++){
            // ojo con la conversión de sample a tiempo
            samples[i] *= Mathf.Sqrt((i/sRATE)/overlapTime);
        }
        clip.SetData(samples, 0);    
    }

    public void SetSourceProperties(AudioSource target) {
        target.volume = SourceVol + UnityEngine.Random.Range(minVol, maxVol);
        target.pitch =  UnityEngine.Random.Range(1 - PitchVar, 1+ PitchVar);
    }
}
