using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShovelController : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] InputActionProperty resetShovelAction;

    [Header("Object References")]
    [SerializeField] private Transform rightController;
    [SerializeField] private Transform shovelTip;

    [Header("Configuration")]
    [SerializeField] private float shovelAdjustSpeed;

    private Vector3 startPosition;
    private bool isHitting = false;


    void Start()
    {
        startPosition = transform.localPosition;
    }

   
    void Update()
    {
        HandleResetShovelAction();
        AdjustShovelPositionOnCollision();
    }



    private void HandleResetShovelAction()
    {
        if (resetShovelAction.action.IsPressed())
        {
            if (!isHitting)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * shovelAdjustSpeed);
            }
        }
    }


    private void AdjustShovelPositionOnCollision()
    {
        // Direction from base to tip
        Vector3 directionToTip = (shovelTip.position - transform.position).normalized;

        // Approximate shovel length (distance from back to tip)
        float shovelLength = Vector3.Distance(shovelTip.position, rightController.position);

        // Safe offset used to keep tip slightly outside voxel
        float surfaceOffset = 0.1f;

        // SphereCast radius approximating shovel thickness
        float shovelRadius = 0.05f;

        // Origin of the cast is behind the shovel tip
        Vector3 rayOrigin = shovelTip.position - directionToTip * shovelLength;

        // Visual debug lines
        Debug.DrawLine(transform.position, shovelTip.position, Color.green); // direction vector
        Debug.DrawRay(rayOrigin, directionToTip * shovelLength, Color.red, 0.5f); // spherecast path

        if (Physics.SphereCast(rayOrigin, shovelRadius, directionToTip, out RaycastHit hit, shovelLength))
        {
            if (hit.collider.CompareTag("Voxel"))
            {
                isHitting = true;

                // Slightly offset the tip position back from hit point
                Vector3 targetTipPosition = hit.point - directionToTip * surfaceOffset;

                // Offset from base to tip (in world space)
                Vector3 tipOffset = shovelTip.position - transform.position;

                // Compute where base needs to be for tip to match targetTipPosition
                Vector3 desiredBasePosition = targetTipPosition - tipOffset;

                // Convert to local space for controlled movement
                Vector3 desiredLocalPosition = transform.parent.InverseTransformPoint(desiredBasePosition);

                // Clamp only distance away from player, not to a fixed Z
                float maxZOffset = 0.5f;
                float zDelta = desiredLocalPosition.z - startPosition.z;
                if (zDelta > maxZOffset)
                    desiredLocalPosition.z = startPosition.z + maxZOffset;

                // Lock other axes (optional depending on design)
                desiredLocalPosition.x = startPosition.x;
                desiredLocalPosition.y = startPosition.y;

                // Smooth movement
                transform.localPosition = Vector3.Lerp(transform.localPosition, desiredLocalPosition, Time.deltaTime * shovelAdjustSpeed);
            }
            else
            {
                isHitting = false;
            }
        }
        else
        {
            // No hit, return to original position
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * shovelAdjustSpeed);
            isHitting = false;
        }
    }



    //private void AdjustShovelPositionOnCollision()
    //{
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    RaycastHit hit;

    //    Vector3 tipOffsetWorld = shovelTip.position - transform.position;
    //    float raycastMaxDistance = Vector3.Magnitude(tipOffsetWorld);

    //    Debug.DrawRay(transform.position, transform.forward * raycastMaxDistance, Color.red, .5f);

    //    if (Physics.Raycast(ray, out hit, raycastMaxDistance))
    //    {
    //        if (hit.collider.CompareTag("Voxel"))
    //        {

    //            // Calculate where the shovel tip should be, slightly inside the voxel surface
    //            Vector3 targetTipPosition = hit.point - transform.forward;

    //            // Compute where the shovel base (transform) needs to be to place the tip at targetTipPosition
    //            Vector3 desiredBasePosition = targetTipPosition - tipOffsetWorld;

    //            // Convert to local space relative to parent and calculate movement offset
    //            Vector3 desiredLocalPosition = transform.parent.InverseTransformPoint(desiredBasePosition);



    //            // Clamp z movement toward the player only (no pushing beyond start)
    //            float maxPullback = 0.5f;
    //            Vector3 delta = desiredLocalPosition - startPosition;
    //            if (delta.z > maxPullback) desiredLocalPosition.z = startPosition.z + maxPullback;
    //            desiredLocalPosition.x = startPosition.x;
    //            desiredLocalPosition.y = startPosition.y;

    //            // Smoothly move the shovel toward the target position
    //            transform.localPosition = Vector3.Lerp(transform.localPosition, desiredLocalPosition, Time.deltaTime * shovelAdjustSpeed);
    //        }
    //    }
    //    else
    //    {
    //        transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime * shovelAdjustSpeed);
    //    }
    //}
}
