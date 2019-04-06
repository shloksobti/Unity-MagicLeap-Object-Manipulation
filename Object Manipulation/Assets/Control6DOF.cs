using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class Control6DOF : MonoBehaviour
{
    #region Private Variables
    private MLInputController _controller;
    private static int BUTTON_ACTIVE = 1;
    //private static int BUTTON_INACTIVE = 0;
    private GameObject copyController;
    #endregion

    #region Public Variables
    public float offset = 1;
    #endregion

    #region Unity Methods
    void Start()
    {
        //Start receiving input by the Control
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
        copyController = new GameObject();
    }


    void OnDestroy()
    {
        //Stop receiving input by the Control
        MLInput.Stop();
        Destroy(copyController);
    }


    void Update()
    {

        var button = (int)MLInputControllerButton.Bumper;
        var buttonState = _controller.State.ButtonState[button];

        //if (buttonState == BUTTON_ACTIVE){
        copyController.transform.position = _controller.Position;
        copyController.transform.rotation = _controller.Orientation;
        Vector3 v3 = copyController.transform.localToWorldMatrix * (new Vector3(0, 0, 1));
        transform.position = _controller.Position + v3;
        transform.rotation = _controller.Orientation;
       // }



    }
    #endregion
}