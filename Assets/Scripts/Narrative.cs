//IMDM Course material
// Author: Myungin Lee
// Date: Fall 2023
// This code shows how to manipulate audio over the time seamlessly referring the flow of the narrative

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrative : MonoBehaviour
{
    float flowtime = 0;
    public GameObject flow;
    // Start is called before the first frame update
    void Start()
    {
        flow = GameObject.Find("flow");
    }

    // Update is called once per frame
    void Update()
    {
        flowtime += Time.deltaTime;
        float tension = Envelope(flowtime);
        //Debug.Log(tension);
        flow.transform.localPosition = new Vector3(flowtime-2, tension, 15);
        if (flowtime > 10)
        {
            flowtime = 0;
        }
    }

    public float Envelope(float t)
    {   // should have something looks like..: __/\_
        // https://www.sciencedirect.com/topics/engineering/envelope-function
        float a = 0.13f;
        float b = 0.45f;
        float tempo = 2f;// timeIdx is an integer increasing rapidly so calm down
        float tension = 10*(Mathf.Abs(Mathf.Exp(-a * (10-t) * tempo) - Mathf.Exp(-b * (10-t) * tempo)));
        return tension;
    }
}
