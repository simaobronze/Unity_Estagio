using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float camForwardOffset = 3.0f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float step;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = cam.transform.position + cam.transform.forward * camForwardOffset;
    }

    // Update is called once per frame
    void Update()
    {
        step = 5.0f * Time.deltaTime;
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.0f)
        {
            centerCube();
        }
        if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x < 0.0f)
        {
            transform.Rotate(0, 5.0f * step, 0);
        }
        if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x > 0.0f)
        {
            transform.Rotate(0, -5.0f * step, 0);
        }
        if (OVRInput.GetUp(OVRInput.Button.One))
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
        }
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.0f)
        {
            transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        }
    }

    void centerCube()
    {
        targetPosition = cam.transform.position + cam.transform.forward * camForwardOffset;
        targetRotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    }
}
