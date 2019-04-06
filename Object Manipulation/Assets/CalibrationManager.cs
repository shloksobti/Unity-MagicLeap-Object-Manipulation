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
   
    bool pulled;
    Quaternion lastControllerOrientation;
    Quaternion rotationDifference;
    Vector3 angles;
    private GameObject copyController;
    private ControllerConnectionHandler _controllerConnectionHandler;
    [HideInInspector] public float triggerValue;
    private const float TriggerThresh = 0.2f;

    // Use this for initialization
    void Start()
    {
        //This boolean describes whether the trigger is pulled.
        pulled = false;

        //_controller = MLInput.GetController(MLInput.Hand.Left);

        copyController = new GameObject();

        _controllerConnectionHandler = GameObject.Find("Controller").GetComponent<ControllerConnectionHandler>();
        _controller = _controllerConnectionHandler.ConnectedController;

    }


    // Update is called once per frame
    void Update()
    {
        triggerValue = _controller.TriggerValue;
        if (CollisionDetector._moveable && triggerValue > TriggerThresh)
        {

            if (!pulled) // Captures the instance trigger is pulled
                lastControllerOrientation = controllerObject.transform.rotation; //Save controller orientation.

            copyController.transform.position = _controller.Position;
            copyController.transform.rotation = _controller.Orientation;
            Vector3 v3 = copyController.transform.localToWorldMatrix * ((virtualFrameObject.transform.position - controllerObject.transform.position)); 
            virtualFrameObject.transform.position = _controller.Position + v3;


            // Apply the rotation from lastcontrolorientation to current orientation
            rotationDifference = Quaternion.Inverse(lastControllerOrientation) * controllerObject.transform.rotation;
            angles = rotationDifference.eulerAngles;
            virtualFrameObject.transform.Rotate(angles, Space.Self);
            pulled = true;
        }
        else
        {
            //Trigger is released.
            pulled = false;
        }

    }

}
