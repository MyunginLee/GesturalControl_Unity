// IMDM Course material
// Author: Myungin Lee
// Date: Spring 2024
// This code demonstrates mapping between gesture to a rigged model
// Landmarks label reference: 
// https://developers.google.com/mediapipe/solutions/vision/pose_landmarker
// https://developers.google.com/mediapipe/solutions/vision/hand_landmarker

using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToModel : MonoBehaviour
{
    static int poseLandmark_number = 32;
    static int handLandmark_number = 20;
    public GameObject tkLeftArm, tkRightArm, tkLeftLeg, tkRightLeg, tkBody, tkHip, tkWhole;
    public GameObject shuffler;
    public static Gesture gen; // singleton
    public bool trigger = false;
    private float distance;
    int totalNumberofLandmark;
    float timer=0;
    Vector3 tkposition;

    private void Awake()
    {
        totalNumberofLandmark = poseLandmark_number + handLandmark_number + handLandmark_number;
        tkLeftArm = GameObject.Find("TKworks@T-Pose/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm");
        tkRightArm = GameObject.Find("TKworks@T-Pose/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm");
        tkBody = GameObject.Find("TKworks@T-Pose/mixamorig:Hips/mixamorig:Spine");
        tkHip = GameObject.Find("TKworks@T-Pose/mixamorig:Hips");
        tkWhole = GameObject.Find("TKworks@T-Pose");
        shuffler = GameObject.Find("Shuffling");

    }
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        // 1. Mapping tk's motion from gestuer
        // landmark refer: https://developers.google.com/mediapipe/solutions/vision/hand_landmarker
        // Assign tk's position. averaged the 20 points from the righthands
        tkposition = new Vector3(0, 0, 0);
        for (int i = 0; i < 20; i++)
        { 
            tkposition = tkposition - Gesture.gen.righthandpos[i];
        }
        tkposition = tkposition / 20;
        tkWhole.transform.position = tkposition;

        // Assign tk's left and right arms and body position.
        // Averaged 2 vectors to get the stable estimation
        // move rabbit using right hands (right: thumb/index/middle) 
        tkRightArm.transform.position = -(Gesture.gen.righthandpos[4] + Gesture.gen.righthandpos[2] + Gesture.gen.righthandpos[3] + Gesture.gen.righthandpos[1]) / 4;
        tkLeftArm.transform.position = -(Gesture.gen.righthandpos[12] + Gesture.gen.righthandpos[10] + Gesture.gen.righthandpos[11] + Gesture.gen.righthandpos[9]) / 4;
        tkBody.transform.position = -(Gesture.gen.righthandpos[8] + Gesture.gen.righthandpos[6] + Gesture.gen.righthandpos[7] + Gesture.gen.righthandpos[5]) / 4;


        // 2. Update Shuffler's position
        shuffler.transform.transform.position = new Vector3(-0.5f + 0.5f*Mathf.Sin(timer), -0.76f, -0.3f + 0.3f * Mathf.Cos(timer));
        timer = timer + 0.01f;
    }
}
