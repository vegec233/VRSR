using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateTeleport : MonoBehaviour
{
    // Left Teleportation if needed.

    // public GameObject leftTeleport;
    // public InputActionProperty leftActivate;

    public GameObject rightTeleport;
    public InputActionProperty rightActivate;
    public InputActionProperty rightCancel;

    public GameObject leftTeleport;
    public InputActionProperty leftActivate;
    public InputActionProperty leftCancel;

    public XRBaseInteractor rightInteractor;
    public XRBaseInteractor leftInteractor;

    private bool heldCheck = false;
    public HandGrabDetector leftHandDetector;
    public HandGrabDetector rightHandDetector;

    public XRRayInteractor rightRay;
    public XRRayInteractor leftRay;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool rightHovering = rightRay.interactablesHovered.Count > 0;
        bool leftHovering = leftRay.interactablesHovered.Count > 0;

        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        bool isRightRayHovering = leftRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);


       
        rightTeleport.SetActive(
            !rightHandDetector.isNearGrabbable &&
            rightActivate.action.ReadValue<float>() > 0.1f && 
            !rightHovering &&
            !isRightRayHovering);

        leftTeleport.SetActive(
            !leftHandDetector.isNearGrabbable &&
            leftActivate.action.ReadValue<float>() > 0.1f && 
            !leftHovering &&
            !isLeftRayHovering);

    }
}
