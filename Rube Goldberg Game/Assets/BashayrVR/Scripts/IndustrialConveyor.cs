using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialConveyor : MonoBehaviour {
    public GameObject belt;
    public Transform endpoint;
    public float speed;

    void OnCollisionStay(Collision collision)
    {
        collision.transform.position = Vector3.MoveTowards(collision.transform.position, endpoint.position, speed * Time.deltaTime);

    }
    
}
