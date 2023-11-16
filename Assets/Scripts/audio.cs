using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioDistortionFilter))]
[RequireComponent(typeof(AudioReverbFilter))]

// Frequency Modulation Synthesizer
public class AudioSynthFM : MonoBehaviour
{
    public static AudioSynthFM synth;
    [Range(20, 4000)]  //Creates a slider in the inspector
    public float frequency; // main note frequency
    [Range(0, 20)]
    public float carrierMultiplier; // carrier frequency = frequency * carrierMultiplier
    [Range(0, 20)]
    public float modularMultiplier; // modular frequency = frequency * modularMultiplier
    public float sampleRate = 44100;
    [Range(0.1f, 2)]  //Creates a slider in the inspector
    public float amplitude;
    AudioSource audioSource;
    int timeIdx = 0;
    public float envelope;
    Renderer cubeRenderer;
    float phase = 0;
    public float distance;
    void Awake()
    {
        if (AudioSynthFM.synth == null)
        {
            AudioSynthFM.synth = this;
        }
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        frequency = 100; // init
        carrierMultiplier = 1.4f;
        modularMultiplier = 0.5f;
        amplitude = 0.1f;
        distance = 0;
        audioSource.Play();
        timeIdx = 0;
    }
    void Update()
    {
        // Check distance?
        Debug.Log("distance: " + distance);
        // Low Pass Filter
        GetComponent<AudioDistortionFilter>().distortionLevel = 0.1f/distance;
        // ReverbFilter
        GetComponent<AudioReverbFilter>().decayTime = 0.1f+distance * 10f;
    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += 2 * Mathf.PI * frequency / sampleRate;
            //data[i] = amplitude * Envelope(timeIdx) * FM(timeIdx, phase, carrierMultiplier, modularMultiplier);
            data[i] = amplitude / (0.1f+distance) * FM(timeIdx, phase, carrierMultiplier, modularMultiplier);
            data[i + 1] = data[i];
            timeIdx++;
            if (phase >= 2 * Mathf.PI)
            {
                phase -= 2 * Mathf.PI;
            }
        }
    }
    // Compute frequency in angular frequency
    public float ComputeFreq(float phase)
    {   // why? http://hplgit.github.io/primer.html/doc/pub/diffeq/._diffeq-solarized002.html#:~:text=Mathematically%2C%20the%20oscillations%20are%20described,means%2044100%20samples%20per%20second.
        return 2 * Mathf.PI * phase / sampleRate; // e.g. 2*pi*440/44100
    }
    // Frequency Modulation computation
    public float FM(int timeIdx, float phase, float carMul, float modMul)
    {
        //return Mathf.Sin(ComputeFreq(phase) * timeIdx); // Sine wave
        return Mathf.Sin((ComputeFreq(phase) * carMul * timeIdx + Mathf.Sin(ComputeFreq(phase) * modMul * timeIdx))); // fluctuating FM
    }
    public float Envelope(int timeIdx)
    {   // should have something looks like..: /\__
        // https://www.sciencedirect.com/topics/engineering/envelope-function
        float a = 0.13f;
        float b = 0.45f;
        float tempo = 1000f;// timeIdx is an integer increasing rapidly so calm down
        return Mathf.Abs(Mathf.Exp(-a * (timeIdx) / tempo) - Mathf.Exp(-b * (timeIdx) / tempo));
    }
}

