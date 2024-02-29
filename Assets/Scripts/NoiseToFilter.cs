// IMDM Course material
// Author: Myungin Lee
// Date: Spring 2024
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
[RequireComponent(typeof(AudioLowPassFilter))]

// Noise to filter

public class NoiseToFilter : MonoBehaviour
{
    public float sampleRate = 44100;
    [Range(0.1f, 2)]  //Creates a slider in the inspector
    public float amplitude; // amplitude of audio
    public float frequency; // main note frequency
    AudioSource audioSource;
    GameObject flow;
    float phase = 0;
    public float distance;
    private Random rand = new Random();
    private bool Button = true;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        flow = GameObject.Find("flow");
        audioSource.playOnAwake = false;
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        amplitude = 0.05f;
        distance = 1;
        frequency = 100;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // trigger of sound synth
        {
            Button = !Button;
        }
        // Check distance
        distance = (Gesture.gen.pose[15] - Gesture.gen.pose[16]).magnitude;
        //// ReverbFilter
        GetComponent<AudioLowPassFilter>().cutoffFrequency = 0.1f + flow.transform.position.y * 1000f;

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
                // White Noise = completely random noise
                data[i] = amplitude * (float)(rand.NextDouble());
                data[i + 1] = data[i];
                if (phase >= 2 * Mathf.PI)
                {
                    phase -= 2 * Mathf.PI;
                }
            }
        }
    }
}

