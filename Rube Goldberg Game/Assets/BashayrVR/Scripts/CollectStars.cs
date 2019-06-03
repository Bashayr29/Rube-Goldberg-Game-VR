using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStars : MonoBehaviour {
    public GameObject scoreText;
    
    public AudioSource collectSound;
    public static bool isDestroyed;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Throwable"))
        {

            collectSound.Play();
            ScoringSystem.theScore += 1;
            gameObject.SetActive(false);
            isDestroyed = true;
            

        }
    }
}
