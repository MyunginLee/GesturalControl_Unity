using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
using Random = System.Random;

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
    public float amplitude; // amplitude of audio
    AudioSource audioSource;
    int timeIdx = 0; // Time increment variable
    public float envelope;
    Renderer cubeRenderer;
    float phase = 0;
    public float distance;
    private Random rand = new Random();
    private bool Button = false;
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
        timeIdx = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // trigger of sound synth
        {
            Button = !Button;
        }
        // Check distance?
        // Debug.Log("distance: " + distance);
        // Low Pass Filter
        GetComponent<AudioDistortionFilter>().distortionLevel = 0.1f/distance;
        //// ReverbFilter
        GetComponent<AudioReverbFilter>().decayTime = 0.1f+ distance * 10f;
        // Random.Range vs rand.NextDouble()?
    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        if (Button)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                phase += 2 * Mathf.PI * frequency / sampleRate;
                // Pure FM
                data[i] = amplitude / (0.1f + distance) * FM(phase, carrierMultiplier, modularMultiplier);
                // Need something brutal? try this
                //data[i] = amplitude / (0.1f + distance) * FM(phase, carrierMultiplier, modularMultiplier) * (float)(rand.NextDouble());
                data[i + 1] = data[i];
                if (phase >= 2 * Mathf.PI)
                {
                    phase -= 2 * Mathf.PI;
                }
            }
        }
    }
    // Frequency Modulation computation
    public float FM(float phase, float carMul, float modMul)
    {
        //return Mathf.Sin(ComputeFreq(phase) * timeIdx); // Sine wave
        return Mathf.Sin(  phase * carMul + Mathf.Sin( phase * modMul) ); // fluctuating FM
    }
    // Envelope, Not used in this code but you may need it to shape something
    public float Envelope(int timeIdx)
    {   // should have something looks like..: /\__
        // https://www.sciencedirect.com/topics/engineering/envelope-function
        float a = 0.13f;
        float b = 0.45f;
        float tempo = 1000f;// timeIdx is an integer increasing rapidly so calm down
        return Mathf.Abs(Mathf.Exp(-a * (timeIdx) / tempo) - Mathf.Exp(-b * (timeIdx) / tempo));
    }
}

