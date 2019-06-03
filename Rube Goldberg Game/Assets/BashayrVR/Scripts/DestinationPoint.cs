using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestinationPoint : MonoBehaviour {
    public GameObject objectToLookFor;
    public string sceneToLoad= "";
    private Scene currentScene ;
    public AudioSource winSound;
    public CollectStars[] collectStars;
    bool isAllUnactive;
    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == objectToLookFor)
        {
            // Change scene.
            if(currentScene.name == "Scene01") {
                foreach(var c in collectStars)
                {
                    if (!c.gameObject.activeInHierarchy)
                    {
                        isAllUnactive = true;
                    }
                    
                }
                
                if (isAllUnactive)
                {
                    winSound.Play();
                    sceneToLoad = "Scene02";
                    SteamVR_LoadLevel.Begin(sceneToLoad);
                    Debug.Log("u r in level 2");
                }
                
            }else if (currentScene.name == "Scene02")
            {
                for (int i = 0; i < collectStars.Length; i++)
                {
                    if (!collectStars[i].gameObject.activeSelf)
                    {
                        isAllUnactive = true;
                    }
                    else
                    {
                        isAllUnactive = false;
                    }

                }
                if (isAllUnactive)
                {
                sceneToLoad = "Scene03";
                winSound.Play();
                SteamVR_LoadLevel.Begin(sceneToLoad);
                Debug.Log("u r in level 3");
                }
                
            }
            else if (currentScene.name == "Scene03")
            {
                for (int i = 0; i < collectStars.Length; i++)
                {
                    if (!collectStars[i].gameObject.activeSelf)
                    {
                        isAllUnactive = true;
                    }
                    else
                    {
                        isAllUnactive = false;
                    }

                }
                if (isAllUnactive)
                {
                    sceneToLoad = "Scene04";
                    winSound.Play();
                    SteamVR_LoadLevel.Begin(sceneToLoad);
                    Debug.Log("u r in level 4");
                }
                
            }
            else
            {
                for (int i = 0; i < collectStars.Length; i++)
                {
                    if (!collectStars[i].gameObject.activeSelf)
                    {
                        isAllUnactive = true;
                    }
                    else
                    {
                        isAllUnactive = false;
                    }

                }
                if (isAllUnactive)
                {
                    winSound.Play();
                    Debug.Log("Levels Complated!");
                }
                
            }
        }
    }
}
