using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiCheat : MonoBehaviour {
    public AudioSource cheatSound;

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.CompareTag("Throwable"))
        {
            if (!ControllerInputManage.isThrow)
            {
                cheatSound.Play();
                
                ControllerInputManage.isThrow = true;
            }
            
            

        }

    }
}
