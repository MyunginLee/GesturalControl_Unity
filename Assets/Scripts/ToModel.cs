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
using UnityEngine;

public class ToModel : MonoBehaviour
{
    static int poseLandmark_number = 32;
    static int handLandmark_number = 20;
    public GameObject tkLeftArm, tkRightArm, tkLeftLeg, tkRightLeg, tkBody;
    public GameObject shuffler;
    public static Genesis gen; // singleton
    public bool trigger = false;
    private float distance;
    int totalNumberofLandmark;
    float timer=0;
    private void Awake()
    {
        totalNumberofLandmark = poseLandmark_number + handLandmark_number + handLandmark_number;
        tkLeftArm = GameObject.Find("TKworks@T-Pose/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm");
        tkRightArm = GameObject.Find("TKworks@T-Pose/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm");
        tkLeftLeg = GameObject.Find("TKworks@T-Pose/mixamorig:Hips/mixamorig:LeftUpLeg");
        tkRightLeg = GameObject.Find("TKworks@T-Pose/mixamorig:Hips/mixamorig:RightUpLeg");
        tkBody = GameObject.Find("TKworks@T-Pose/mixamorig:Hips/mixamorig:Spine");
        shuffler = GameObject.Find("Shuffling");
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        // move rabbit using right and left hands (right: thumb/index/middle & left: index/middle) 
        tkRightArm.transform.eulerAngles = (Genesis.gen.righthandpos[12] - Genesis.gen.righthandpos[9] ).normalized * 90;
        tkLeftArm.transform.eulerAngles = -(Genesis.gen.righthandpos[4] - Genesis.gen.righthandpos[1]).normalized *90;
        tkBody.transform.eulerAngles = -(Genesis.gen.righthandpos[8] - Genesis.gen.righthandpos[5]).normalized * 180;
        tkLeftLeg.transform.eulerAngles = (Genesis.gen.lefthandpos[12] - Genesis.gen.lefthandpos[9]).normalized * 180;
        tkRightLeg.transform.eulerAngles = (Genesis.gen.lefthandpos[8] - Genesis.gen.lefthandpos[5]).normalized * 180;
        shuffler.transform.transform.position = new Vector3(-0.97f + 0.3f*Mathf.Sin(timer),-0.76f,-1 + 0.3f * Mathf.Cos(timer));
        timer = timer + 0.01f;
    }
}
