// IMDM Course material
// Author: Myungin Lee
// Date: Fall 2023
// This code demonstrates the general applications of landmark information
// Pose + Left, Right hand landmarks data avaiable. Facial landmark need custom work
// Landmarks label reference: 
// https://developers.google.com/mediapipe/solutions/vision/pose_landmarker
// https://developers.google.com/mediapipe/solutions/vision/hand_landmarker

using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genesis : MonoBehaviour
{
    static int poseLandmark_number = 32;
    static int handLandmark_number = 20;
    // Declare landmark vectors 
    public Vector3[] pose = new Vector3[poseLandmark_number];
    public Vector3[] righthandpos = new Vector3[handLandmark_number];
    public Vector3[] lefthandpos = new Vector3[handLandmark_number];
    public GameObject[] PoseLandmarks, LeftHandLandmarks, RightHandLandmarks;
    private GameObject head, rhand, lhand, body;
    public GameObject tkLeftArm, tkRightArm, tkLeftLeg, tkRightLeg;

    public static Genesis gen; // singleton
    public bool trigger = false;
    private float distance;
    int totalNumberofLandmark;
    private void Awake()
    {
        if (Genesis.gen == null)
        {
            Genesis.gen = this;
        }
        totalNumberofLandmark = poseLandmark_number + handLandmark_number + handLandmark_number;
        PoseLandmarks = new GameObject[poseLandmark_number];
        LeftHandLandmarks = new GameObject[handLandmark_number];
        RightHandLandmarks = new GameObject[handLandmark_number];
    }
    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.Find("head");
        rhand = GameObject.Find("rhand");
        lhand = GameObject.Find("lhand");
        body = GameObject.Find("body");
        // Initiate pose landmarks as spheres
        for (int i = 0; i < poseLandmark_number; i++)
        {
            PoseLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        // Initiate R+L hands landmarks as spheres
        for (int i = 0; i < handLandmark_number; i++)
        {
            LeftHandLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            RightHandLandmarks[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Case 0. Draw holistic shape
        // Assign Pose landmarks position
        int idx = 0;
        foreach (GameObject pl in PoseLandmarks)
        {
            pl.transform.transform.position = -pose[idx];
            Color customColor = new Color(idx*10 / 255, idx * 7 / 255, idx * 3 / 255, 1); // Color of pose landmarks
            pl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            pl.transform.localScale = new Vector3(0.05f,0.05f,0.05f);
            idx++;
        }
        // Assign Left hand landmarks position
        idx = 0;
        foreach (GameObject lhl in LeftHandLandmarks)
        {
            lhl.transform.transform.position = -lefthandpos[idx];
            Color customColor = new Color(idx * 4 / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of left hand landmarks
            lhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            lhl.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            idx++;
        }
        // Assign Right hand landmarks position
        idx = 0;
        foreach (GameObject rhl in RightHandLandmarks)
        {
            rhl.transform.transform.position = -righthandpos[idx];
            Color customColor = new Color(idx * 4f / 255, idx * 15f / 255, idx * 30f / 255, 1); // Color of right hand landmarks
            rhl.GetComponent<Renderer>().material.SetColor("_Color", customColor);
            rhl.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            idx++;
        }

        // Case 1. Sound synth model ( head, R+L hands + body)
        // This part use existing Gameobjects and assign position. Not relavent to audio at all.
        head.transform.position = -pose[0] * 30;
        rhand.transform.position = -pose[15] * 30;
        lhand.transform.position = -pose[16] * 30;
        body.transform.position = -(pose[11] + pose[12] + pose[23] + pose[24]) / 4 * 30;

        // Map positional parameters to audio parameters
        AudioSynthFM.synth.frequency = Mathf.Abs(-pose[15].y * 100 + 100); // left hand pitch. but it can be anything you know
        AudioSynthFM.synth.carrierMultiplier = Mathf.Abs(pose[15].x + pose[16].x); 
        AudioSynthFM.synth.modularMultiplier = Mathf.Abs(pose[15].y + pose[16].y);
        // Debug.Log(AudioSynthFM.synth.frequency + "   " + AudioSynthFM.synth.carrierMultiplier + "   "  +  AudioSynthFM.synth.modularMultiplier);
        distance = (pose[15] - pose[16]).magnitude;
        AudioSynthFM.synth.distance = distance;
        // Clapping detection
        if (distance < 0.15)
        {
            trigger = true;
        }
        // Refer "audio.cs" to change the audio
    }
}
