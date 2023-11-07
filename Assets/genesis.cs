using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genesis : MonoBehaviour
{
    public Vector3 faceposition, righthandposition;
    private GameObject head, rhand;
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
        righthandposition = new Vector3(0, 0, 0);
        head = GameObject.Find("head");
        rhand = GameObject.Find("rhand");
    }

// Update is called once per frame
void Update()
    {
        Debug.Log("face[1] position = " + faceposition);
        Debug.Log("rhand[15] position = " + righthandposition);
        head.transform.position = faceposition * 30;
        rhand.transform.position = righthandposition * 30;
    }
}
