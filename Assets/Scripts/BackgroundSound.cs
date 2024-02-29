using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioEchoFilter))]

public class BackgroundSound : MonoBehaviour
{
    GameObject flow;
    // Start is called before the first frame update
    void Start()
    {
        flow = GameObject.Find("flow");

    }

    // Update is called once per frame
    void Update()
    {
        // Low pass filter
        // left hand.y controls low pass filter's cutoff freq
        float lowcutoff = 12000 - Gesture.gen.pose[15].y * 11000;
        //Debug.Log(Gesture.gen.pose[15].y + " ~ " + cutofffreq);
        GetComponent<AudioLowPassFilter>().cutoffFrequency = lowcutoff;

        // High pass filter
        //float hightcutoff = Gesture.gen.pose[16].x * 11000;
        //GetComponent<AudioHighPassFilter>().cutoffFrequency = hightcutoff;
        //Debug.Log(Gesture.gen.pose[16].x + " ~ " + hightcutoff);

        // Echo filter
        // The Audio Echo Filter repeats a sound after a given Delay, attenuating the repetitions based on the Decay Ratio.
        if ((Gesture.gen.righthandpos[8] - Gesture.gen.righthandpos[4]).magnitude < 0.03f) // if right hand is pinched, do~
        {
            GetComponent<AudioEchoFilter>().delay = Gesture.gen.pose[16].y * 500;
            GetComponent<AudioEchoFilter>().wetMix = Gesture.gen.pose[16].x;
            GetComponent<AudioEchoFilter>().dryMix = Gesture.gen.pose[16].y;
        }
        else
        {
            GetComponent<AudioEchoFilter>().delay = 0;
        }

    }
}
