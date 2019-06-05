using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputManage : MonoBehaviour {
    public SteamVR_TrackedObject trackedObject;
    public SteamVR_Controller.Device device;
    public float throwForce = 1.5f;
    public bool isRight;
    public static bool isThrow;



    // Teleporter
    private LineRenderer laser;
    public GameObject teleportAimerObject;
    public Vector3 teleportLocation;
    public GameObject player;
    public LayerMask laserMask;
    public static float yNudgeAmount = 1f; // specific to teleportAimerObject height
    private static readonly Vector3 yNudgeVector = new Vector3(0f, yNudgeAmount, 0f);

    //Walking
    public Transform playerCam;
    public float moveSpeed = 4f;
    private Vector3 movementDirection;

    //Dash
    public float dashSpeed = 0.3f;
    private bool isDashing;
    private float lerpTime;
    private Vector3 dashStartPosition;

    //Swipe
    private float swipeSum;
    private float touchLast;
    private float touchCurrent;
    private float distance;
    private bool hasSwipedLeft;
    private bool hasSwipedRight;
    public ObjectMenuManager objectMenuManager;

    // Use this for initialization
    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        laser = GetComponentInChildren<LineRenderer>();
    }

    void setLaserStart(Vector3 startPos)
    {
        laser.SetPosition(0, startPos);
       
    }

    void setLaserEnd(Vector3 endPos)
    {
        laser.SetPosition(1, endPos);
    }

    // Update is called once per frame
    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (isRight)
        {
            if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                
                touchLast = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;

            }
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                touchCurrent = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
                distance = touchCurrent - touchLast;
                touchLast = touchCurrent;
                swipeSum += distance;
                if (!hasSwipedRight)
                {
                    if (swipeSum > 0.5f)
                    {
                        swipeSum = 0;
                        SwipedRight();
                        hasSwipedRight = true;
                        hasSwipedLeft = false;
                    }
                }
                if (!hasSwipedLeft)
                {
                    if (swipeSum < -0.5f)
                    {
                        swipeSum = 0;
                        SwipedLeft();
                        
                        hasSwipedLeft = true;
                        hasSwipedRight = false;
                    }
                }
                
            }
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
            {
                swipeSum = 0;
                touchCurrent = 0;
                touchLast = 0;
                hasSwipedLeft = false;
                hasSwipedRight = false;
            }


            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            {
                //Spwan
                SpawnObject();
            }

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                objectMenuManager.EnableDisableMenu();
            }
        }else
        {
            //not used!
            if (isDashing)
            {
                lerpTime += Time.deltaTime * dashSpeed;
                player.transform.position = Vector3.Lerp(dashStartPosition, teleportLocation, lerpTime);
                if (lerpTime >= 1)
                {
                    isDashing = false;
                    lerpTime = 0;
                }
            }
            else
            {
                if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger) && device.GetPress(SteamVR_Controller.ButtonMask.Axis0))
                {
                    laser.gameObject.SetActive(true);
                    teleportAimerObject.SetActive(true);
                    

                    setLaserStart(gameObject.transform.position);
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, 25f, laserMask) )
                    {
                        teleportLocation = hit.point;

                    }
                    else
                    {
                        teleportLocation = transform.position + 15 * transform.forward;
                        RaycastHit groundRay;
                        if (Physics.Raycast(teleportLocation, -Vector3.up, out groundRay, 17, laserMask))
                        {
                            teleportLocation.y = groundRay.point.y;
                        }
                    }
                    setLaserEnd(teleportLocation);
                    // aimer
                    teleportAimerObject.transform.position = teleportLocation + yNudgeVector;
                }

                if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                {
                    laser.gameObject.SetActive(false);
                    teleportAimerObject.SetActive(false);
                    player.transform.position = teleportLocation;
                    //dashStartPosition = player.transform.position;
                    //isDashing = true;
                }


            }
            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                movementDirection = playerCam.transform.forward;
                movementDirection = new Vector3(movementDirection.x, 0, movementDirection.z);
                movementDirection = movementDirection * moveSpeed * Time.deltaTime;
                player.transform.position += movementDirection;
            }
        }
        

    }
    
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Throwable"))
        {
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                ThrowObject(col);
            }
            else if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                GrabObject(col);

            }
        }

        if (col.gameObject.CompareTag("Structure"))
        {
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                ThrowObjectforRubeGoldberg(col);
            }
            else if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                GrabObject(col);

            }
        }
    }

    void GrabObject(Collider col)
    {
        col.transform.SetParent(gameObject.transform);
        col.GetComponent<Rigidbody>().isKinematic = true;
        device.TriggerHapticPulse(2000);
        Debug.Log("You are touching down the trigger on an object");

    }
    public void ThrowObject(Collider col)
    {
        col.transform.SetParent(null);
        Rigidbody rigidbody = col.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.velocity = device.velocity * throwForce;
        rigidbody.angularVelocity = device.angularVelocity;
        Debug.Log("You have released the trigger");
        isThrow = true;

    }
    void SpawnObject()
    {
        objectMenuManager.SpawnCurrentObject();
    }
    void SwipedLeft()
    {
        objectMenuManager.MenueLeft();
        Debug.Log("SwipeLeft!");
    }
    void SwipedRight()
    {
        objectMenuManager.MenueRight();
        Debug.Log("SwipeRight!");
    }

    //Fix Object Grabbing for Rube Goldberg objects
    void ThrowObjectforRubeGoldberg(Collider col)
    {
        col.transform.SetParent(null);
        Rigidbody rigidbody = col.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.angularVelocity = device.angularVelocity;
        Debug.Log("You have released the trigger");
    }
}