using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;


/*
 * This script is the core component of the calibration functionality between the Magic Leap and VICON frames
 *
 */

public class CalibrationManager : MonoBehaviour
{
    public GameObject virtualFrameObject; //This is the object we are placing to overlay the real object
    public GameObject controllerObject;

    //These comprise VICONMapFrame's transform inside Unity
    private Vector3 viconToUnity_translation;
    private Quaternion viconToUnity_rotation;

    //Controler
    private MLInputController _controller;

    //Other things
   
    bool triggerPulled;
    Quaternion lastControllerOrientation;
    Vector3 offsetToController;
    Quaternion rotationDifference;
    Vector3 angles;
    private GameObject copyController;
    private ControllerConnectionHandler _controllerConnectionHandler;
    [HideInInspector] public float triggerValue;
    private const float triggerThresh = 0.2f;

    // Use this for initialization
    void Start()
    {
        //This boolean describes whether the trigger is pulled.
        triggerPulled = false;

        //_controller = MLInput.GetController(MLInput.Hand.Left);

        copyController = new GameObject();

        _controllerConnectionHandler = GameObject.Find("Controller").GetComponent<ControllerConnectionHandler>();
        _controller = _controllerConnectionHandler.ConnectedController;

        offsetToController = new Vector3();
    }


    // Update is called once per frame
    void Update()
    {
        
        //Trigger logic to manipulate the virtualFrameObject
        triggerValue = _controller.TriggerValue;
        if (triggerValue >= triggerThresh)
        {
            //Decide if we should initiate or continue movement, or do nothing

            //Initialize movement
            if (CollisionDetector._moveable && !triggerPulled)
            {
                triggerPulled = true;
                lastControllerOrientation = controllerObject.transform.rotation;
                offsetToController = controllerObject.transform.worldToLocalMatrix * (virtualFrameObject.transform.position - controllerObject.transform.position);
            }
            //Move virtualFrameObject with the controller
            else if (triggerPulled)
            {
                rotationDifference = controllerObject.transform.rotation * Quaternion.Inverse(lastControllerOrientation);
                lastControllerOrientation = controllerObject.transform.rotation;
                virtualFrameObject.transform.rotation = rotationDifference * virtualFrameObject.transform.rotation;

                Vector3 offsetToWorld = controllerObject.transform.localToWorldMatrix * offsetToController;
                virtualFrameObject.transform.position = offsetToWorld + controllerObject.transform.position;

            }
        }
        else
        {
            if (triggerPulled)
                triggerPulled = false;
        }


    }

}
