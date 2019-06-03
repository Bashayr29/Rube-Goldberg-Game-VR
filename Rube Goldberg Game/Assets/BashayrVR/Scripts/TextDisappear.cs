using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisappear : MonoBehaviour {

    public float time_seconds = 1.5f; //Seconds to read the text
    // Use this for initialization
    void Start()
    {
        
        Destroy(gameObject, time_seconds);
    }
}
