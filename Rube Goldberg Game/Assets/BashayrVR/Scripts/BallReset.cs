using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {
    private Vector3 startPosition;
    private Color errorColor = Color.red;
    public CollectStars []collectStars;
    bool isStarActive;


    private Renderer rend;
    // Use this for initialization
    void Start () {
        startPosition = gameObject.transform.position;
        rend = GetComponent<Renderer>();
        



    }
     void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Target") && !collision.gameObject.CompareTag("StartPlatform"))
        {
            
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.velocity = Vector3.zero;
            rigidBody.isKinematic = false;
            gameObject.transform.position = startPosition;
            rend.material.color = errorColor;

            if (CollectStars.isDestroyed)
            {
                Debug.Log("The ball fall on the ground, re-appear the star and the score removed!");

                for(int i =0; i< collectStars.Length; i++)
                {
                    
                    if (!collectStars[i].gameObject.activeSelf) {
                        collectStars[i].gameObject.SetActive(true);
                        ScoringSystem.theScore -= 1;
                    }
                    
                }
                
                
                CollectStars.isDestroyed = false;
            }
            

        }
        
    }
   
}
