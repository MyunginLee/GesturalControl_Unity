//IMDM Course material
// Author: Myungin Lee
// Date: Spring 2024
// This code demonstrates how to use landmarks to draw sets of skeletons on the finger
// Credit: Silas, Mateo, Sreya, Madeleine Team from IMDM290, 2024

using Mediapipe.Unity.Holistic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public GameObject obj;

    // List pairs of landmarks
    int[,] linePairsL = new int[,] { {4,3}, {3,2}, {2,1}, {1,0}, {8,7}, {7,6}, {6,5}, {5,0},
{12,11}, {11,10}, {10,9}, {16,15}, {15,14}, {14,13},
{20, 19}, {19,18}, {18,17}, {17,0}}; 

    int[,] linePairsR = new int[,] { {4,3}, {3,2}, {2,1}, {1,0}, {8,7}, {7,6}, {6,5}, {5,0},
{12,11}, {11,10}, {10,9}, {16,15}, {15,14}, {14,13},
{20, 19}, {19,18}, {18,17}, {17,0}}; 
    private GameObject[] capsuleContainerL; 
    private GameObject[] capsuleContainerR; 

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.SetActive(false);
        capsuleContainerL = new GameObject[linePairsL.GetLength(0)];
        for (int i = 0; i < capsuleContainerL.Length; i++)
        {
            capsuleContainerL[i] = Instantiate(obj);
            capsuleContainerL[i].SetActive(false);
        }

        capsuleContainerR = new GameObject[linePairsL.GetLength(0)];
        for (int i = 0; i < capsuleContainerR.Length; i++)
        {
            capsuleContainerR[i] = Instantiate(obj);
            capsuleContainerR[i].SetActive(false);
        }
    }
    // Update is called once per frame

    void placeCapsule(GameObject obj, Vector3 pos1, Vector3 pos2, float distance)
    {
        obj.SetActive(true);
        Vector3 v = pos2 - pos1;
        float fl = v.magnitude;
        obj.transform.position = pos1 + v / 2;
        obj.transform.up = v;
        obj.transform.localScale = new Vector3(distance, fl / 2, distance);
    }

    Vector3 scaleVector(Vector3 v)
    {
        Vector3 scaled;
        scaled = new Vector3(-20 * v.x + 10, -15 * v.y + 7, v.z+10);
        return scaled;
    }
    void Update()
    {
        // Lefthand skeleton draw
        for (int i = 0; i < linePairsL.GetLength(0); i++)
        {
            int first = linePairsL[i, 0];
            int second = linePairsL[i, 1];

            if (first < Gesture.gen.lefthandpos.Length && second < Gesture.gen.lefthandpos.Length)
            {
                Vector3 pos1 = Gesture.gen.lefthandpos[first];
                Vector3 pos2 = Gesture.gen.lefthandpos[second];
                pos1 = scaleVector(pos1);
                pos2 = scaleVector(pos2);
                placeCapsule(capsuleContainerL[i], pos1, pos2, .05f);
            }
        }
        // Righthand skeleton draw
        for (int i = 0; i < linePairsR.GetLength(0); i++)
        {
            int first = linePairsR[i, 0];
            int second = linePairsR[i, 1];

            if (first < Gesture.gen.righthandpos.Length && second < Gesture.gen.righthandpos.Length)
            {
                Vector3 pos1 = Gesture.gen.righthandpos[first];
                Vector3 pos2 = Gesture.gen.righthandpos[second];
                pos1 = scaleVector(pos1);
                pos2 = scaleVector(pos2);
                placeCapsule(capsuleContainerR[i], pos1, pos2, .05f);
            }
        }

    }
}