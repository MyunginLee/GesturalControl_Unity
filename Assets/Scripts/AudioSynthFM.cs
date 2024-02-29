// IMDM Course material
// Author: Myungin Lee
// Date: Spring 2024
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(AudioReverbFilter))]
//[RequireComponent(typeof(AudioLowPassFilter))]

// Frequency Modulation Synthesizer with theremin like interface
// Press "space" to activate. 

public class AudioSynthFM : MonoBehaviour
{
    public float frequency; // main note frequency
    [Range(0, 20)]
    public float carrierMultiplier; // carrier frequency = frequency * carrierMultiplier
    [Range(0, 20)]
    public float modularMultiplier; // modular frequency = frequency * modularMultiplier
    public float sampleRate = 44100;
    [Range(0.1f, 2)]  //Creates a slider in the inspector
    public float amplitude; // amplitude of audio
    AudioSource audioSource;
    GameObject flow;
    float phase = 0;
    public float distance;
    private bool Button = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        flow = GameObject.Find("flow");
        audioSource.playOnAwake = false;
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        frequency = 100; // init
        carrierMultiplier = 1.4f;
        modularMultiplier = 1f;
        amplitude = 0.1f;
        distance = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // trigger of sound synth
        {
            Button = !Button;
        }
        // Check distance
        distance = (Gesture.gen.lefthandpos[8] - Gesture.gen.lefthandpos[4]).magnitude;
        //Debug.Log(distance);
        if (distance> 0 && distance < 0.03f)
        {
            Button = true;
        }
        else
        {
            Button = false;
        }
        //// ReverbFilter
        GetComponent<AudioReverbFilter>().decayTime = 10;
        // Lowpass filter
        //GetComponent<AudioLowPassFilter>().cutoffFrequency = 0.1f+ distance * 1000f;

        carrierMultiplier = distance * 5;
        // assign 
        frequency = (-Gesture.gen.pose[15].y+1) * 800 + 20;
        modularMultiplier = (Gesture.gen.pose[16].x + 1);
        // Random.Range vs rand.NextDouble()
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (Button)
        {
            for (int i = 0; i < data.Length; i += channels)
            {
                phase += 2 * Mathf.PI * frequency / sampleRate;
                // Pure FM
                //data[i] = amplitude / (0.1f + distance) * FM(phase, carrierMultiplier, modularMultiplier);
                data[i] = amplitude * FM(phase, carrierMultiplier, modularMultiplier) * (1 + Gesture.gen.pose[16].x);
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
        return Mathf.Sin(  phase * carMul + Mathf.Sin( phase * modMul) ); // fluctuating FM
    }
}

