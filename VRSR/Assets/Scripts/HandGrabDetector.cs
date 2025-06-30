using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabDetector : MonoBehaviour
{

    public bool isNearGrabbable = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("grabable"))
        {
            isNearGrabbable = true;
            // Debug.Log(handName + " hand is near " + other.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("grabable"))
        {
            isNearGrabbable = false;
            // Debug.Log(handName + " hand left " + other.name);
        }
    }
}
