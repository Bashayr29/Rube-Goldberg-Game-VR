using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMenuManager : MonoBehaviour {
    public List<GameObject> objectList;
    public List<GameObject> objectPrefablist;
    public bool isActive = false;


    public int currentObject = 0;
	// Use this for initialization
	void Start () {
		foreach(Transform child in transform)
        {
            objectList.Add(child.gameObject);
        }
	}
	public void MenueLeft()
    {
        objectList[currentObject].SetActive(false);
        currentObject--;
        if(currentObject < 0)
        {
            currentObject = objectList.Count - 1;
        }
        objectList[currentObject].SetActive(true);
    }

    public void MenueRight()
    {
        objectList[currentObject].SetActive(false);
        currentObject++;
        if (currentObject > objectList.Count - 1)
        {
            currentObject = 0;
        }
        objectList[currentObject].SetActive(true);
    }

    public void SpawnCurrentObject()
    {
        Instantiate(objectPrefablist[currentObject],
            objectList[currentObject].transform.position,
            objectList[currentObject].transform.rotation);

    }



    public void EnableDisableMenu()
    {
        isActive ^= true;
        if (isActive)
        {
            // show current
            objectList[currentObject].SetActive(true);
        }
        else
        {
            // hide all objects 
            foreach (GameObject obj in objectList)
            {
                obj.SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
