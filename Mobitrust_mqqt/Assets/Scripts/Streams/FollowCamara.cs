using Oculus.Platform;
using UnityEngine;

public class FollowCamara : MonoBehaviour
{
    public Transform vrCamera;
    public Vector3 offset = new(0, 0, 10);

    
    void LateUpdate()
    {
        if (vrCamera != null)
        {
            
            transform.SetPositionAndRotation(vrCamera.position + vrCamera.forward * offset.z
                                                 + vrCamera.up * offset.y
                                                 + vrCamera.right * offset.x, Quaternion.LookRotation(transform.position - vrCamera.position));
        }
        else
        {
            Debug.LogError("vr camera is not assigned!");
        }
    }

}
