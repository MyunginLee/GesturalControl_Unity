using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour
{
    public static Finger f;
    public Vector3 thumb_pos;
    private void Awake()
    {
        if (Finger.f == null)
        {
            Finger.f = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        thumb_pos = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(thumb_pos);
        
    }
}
