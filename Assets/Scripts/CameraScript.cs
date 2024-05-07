using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public FreeCameraMovement freeCameraMovementScript;
    public CameraController CurrentController;
    public TouchControlCamera touchControlCameraScript;

    void Start()
    {
        Debug.Log("EnteringChangeCameraScript");
        CurrentController = freeCameraMovementScript;
        CurrentController.Init(touchControlCameraScript);
    }

    void Update()
    {
        CurrentController.CameraRoutine(this);
    }
}
