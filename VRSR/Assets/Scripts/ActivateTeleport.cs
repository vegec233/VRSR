using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivateTeleport : MonoBehaviour
{
    // Left Teleportation if needed.

    // public GameObject leftTeleport;
    // public InputActionProperty leftActivate;

    public GameObject rightTeleport; 
    public InputActionProperty rightActivate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // leftTeleport.SetActive(leftActivate.action.ReadValue<float>() > 0.1);
        rightTeleport.SetActive(rightActivate.action.ReadValue<float>() > 0.1);
        
    }
}
