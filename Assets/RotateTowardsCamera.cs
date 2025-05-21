using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject leftHand;
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        leftHand = GameObject.FindGameObjectWithTag("LeftHand");
        transform.SetPositionAndRotation(leftHand.transform.position + leftHand.transform.up * 0.1f, mainCamera.transform.rotation);
    }
}
