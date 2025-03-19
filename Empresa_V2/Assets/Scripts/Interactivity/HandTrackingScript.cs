using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingScript : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] OVRHand leftHand;
    [SerializeField] OVRHand rightHand;
    [SerializeField] OVRSkeleton skeleton;
    [SerializeField] float step = 5.0f;
    [SerializeField] float cubeDistanceFromFace = 5.0f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isIndexFingerPinching;

    private LineRenderer line;
    private Transform p0;
    private Transform p1;
    private Transform p2;

    private Transform handIndexTipTransform;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = cam.transform.position + cam.transform.forward * cubeDistanceFromFace;
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        step = 5.0f + Time.deltaTime;

        if (leftHand.IsTracked)
        {
            isIndexFingerPinching = leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            if (isIndexFingerPinching)
            {
                line.enabled = true;
                pinchCube();

                foreach (var bone in skeleton.Bones)
                {
                    if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                    {
                        handIndexTipTransform = bone.Transform;
                        break;
                    }
                }
                DrawCurve(handIndexTipTransform.position, cam.transform.position + cam.transform.forward * 0.8f, transform.position);
            }
            else
            {
                line.enabled = false;
            }
        }
    }

    void pinchCube()
    {
        targetPosition = leftHand.transform.position - leftHand.transform.forward * 0.4f;
        targetRotation = Quaternion.LookRotation(transform.position - leftHand.transform.position);

        transform.position = Vector3.Lerp(transform.position, targetPosition, step);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, step);
    }

    void DrawCurve(Vector3 point_0, Vector3 point_1, Vector3 point_2)
    {
        line.positionCount = 200;
        Vector3 B = new Vector3(0, 0, 0);
        float t = 0f;

        for (int i = 0; i < line.positionCount; i++)
        {
            t += 0.005f;
            B = (1 - t) * (1 - t) * point_0 + 2 * (1 - t) * t * point_1 + t * t * point_2;
            line.SetPosition(i, B);
        }
    }
}
