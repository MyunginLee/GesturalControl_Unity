using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genesis : MonoBehaviour
{
    public Vector3 faceposition;
    private GameObject head;
    public static Genesis gen;
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
        faceposition = new Vector3(0, 0, 0);
        head = GameObject.Find("head");
    }

// Update is called once per frame
void Update()
    {
        Debug.Log("face[1] position = " + faceposition);
        head.transform.position = faceposition;
    }
}
