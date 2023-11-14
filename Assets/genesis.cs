using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genesis : MonoBehaviour
{
    public Vector3[] pose = new Vector3[32];
    public Vector3[] righthandpos = new Vector3[20];
    public Vector3[] lefthandpos = new Vector3[20];
    private GameObject head, rhand, lhand, body;
    public static Genesis gen; // singleton
    public bool trigger;
    private void Awake()
    {
        if(Genesis.gen == null)
        {
            Genesis.gen = this;
        }        
    }
    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.Find("head");
        rhand = GameObject.Find("rhand");
        lhand = GameObject.Find("lhand");
        body = GameObject.Find("body");
    }

// Update is called once per frame
void Update()
    {
        head.transform.position = -pose[0] * 30;
        rhand.transform.position = -pose[15] * 30;
        lhand.transform.position = -pose[16] * 30;
        body.transform.position = -pose[12] * 30;
        AudioSynthFM.synth.frequency = pose[15].y * 3000 + 300;
        AudioSynthFM.synth.carrierMultiplier = Mathf.Abs(pose[15].z) * 100 + 10;
        float distance = (pose[15] - pose[16]).magnitude * 100;
        if (distance < 10)
        {
            trigger = true;
        }
    }
}
