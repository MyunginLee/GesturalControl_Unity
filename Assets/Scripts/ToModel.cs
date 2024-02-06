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

public class ToModel : MonoBehaviour
{
    static int poseLandmark_number = 32;
    static int handLandmark_number = 20;
    public GameObject tkLeftArm, tkRightArm, tkLeftLeg, tkRightLeg, tkBody;

    public static Genesis gen; // singleton
    public bool trigger = false;
    private float distance;
    int totalNumberofLandmark;
    float timer=0;
    private void Awake()
    {
        totalNumberofLandmark = poseLandmark_number + handLandmark_number + handLandmark_number;
        tkLeftArm = GameObject.Find("mixamorig:LeftArm");
        tkRightArm = GameObject.Find("mixamorig:RightArm");
        tkLeftLeg = GameObject.Find("mixamorig:LeftUpLeg");
        tkRightLeg = GameObject.Find("mixamorig:RightUpLeg");
        tkBody = GameObject.Find("mixamorig:Spine");
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        tkRightArm.transform.eulerAngles = (Genesis.gen.righthandpos[12] - Genesis.gen.righthandpos[9] ).normalized * 90;
        tkLeftArm.transform.eulerAngles = -(Genesis.gen.righthandpos[4] - Genesis.gen.righthandpos[1]).normalized *90;
        tkBody.transform.eulerAngles = -(Genesis.gen.righthandpos[8] - Genesis.gen.righthandpos[5]).normalized * 180;

        tkLeftLeg.transform.eulerAngles = (Genesis.gen.lefthandpos[12] - Genesis.gen.lefthandpos[9]).normalized * 180;
        tkRightLeg.transform.eulerAngles = (Genesis.gen.lefthandpos[8] - Genesis.gen.lefthandpos[5]).normalized * 180;


        // Map positional parameters to audio parameters
        AudioSynthFM.synth.frequency = Mathf.Abs(-Genesis.gen.pose[15].y * 100 + 100); // left hand pitch. but it can be anything you know
        AudioSynthFM.synth.carrierMultiplier = Mathf.Abs(Genesis.gen.pose[15].x + Genesis.gen.pose[16].x); 
        AudioSynthFM.synth.modularMultiplier = Mathf.Abs(Genesis.gen.pose[15].y + Genesis.gen.pose[16].y);
        // Debug.Log(AudioSynthFM.synth.frequency + "   " + AudioSynthFM.synth.carrierMultiplier + "   "  +  AudioSynthFM.synth.modularMultiplier);
        distance = (Genesis.gen.pose[15] - Genesis.gen.pose[16]).magnitude;
        AudioSynthFM.synth.distance = distance;
        // Clapping detection
        if (distance < 0.15)
        {
            trigger = true;
        }
        // Refer "audio.cs" to change the audio
        timer = timer +0.01f;
    }
}
