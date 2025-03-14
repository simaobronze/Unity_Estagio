using MAGES.RemotePhysics.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magesTeste : MonoBehaviour
{
    [SerializeField]
    private DissectedGameObject cube;
    [SerializeField]
    private DissectedGameObject plane;
    private bool printedDebug = false;
    private bool setupListenners = false;

    // Update is called once per frame
    void Update()
    {
        if (cube.IsReady && !printedDebug)
        {
            if (!setupListenners)
            {
                cube.GetRemoteCollider().OnCollisionEnter.AddListener(() =>
                {
                    Debug.Log("Colision enter -> " + cube.OutgoingRemoteGameObject.RemoteTransform.LocalPosition.ToString());
                    cube.RequestChangeObjectOwnership(true);
                    cube.SetVelocity(new Vector3(0f,5f,0f));
                    cube.OutgoingRemoteGameObject.RemoteTransform.LocalPosition = new Vector3(3.72f,10f,-4f);
                    Debug.Log("Cube transform set to -> " + cube.OutgoingRemoteGameObject.RemoteTransform.LocalPosition.ToString());
                });

                cube.GetRemoteCollider().OnCollisionExit.AddListener(() => 
                {
                    Debug.Log("Colision Exit");
                });
            }
            //Debug.Log($"ID -> {cube.Object_id}");
            //Debug.Log($"Owner -> {cube.OwnerID}");
            //Debug.Log($"Outgoing remote object transform -> {cube.OutgoingRemoteGameObject.RemoteTransform}");
            //Debug.Log($"Outgoing remote object type -> {cube.OutgoingRemoteGameObject.Type}");
            //Debug.Log($"Object layer -> {cube.Layer}");
            //Debug.Log($"Ownership -> {cube.OwnershipState}");

            if(cube.OutgoingRemoteGameObject != null)
            {
                Debug.Log("OutGoing remote object is not null");
                if (cube.OutgoingRemoteGameObject.HasRigidbody)
                {
                    Debug.Log("RemoteRigidBody found");
                    cube.SetGravity(true);
                    Debug.Log("Gravity changed to -> " + cube.OutgoingRemoteGameObject.RemoteRigidbody.UseGravity);
                }
                else
                {
                    Debug.Log("RemoteRigidBody is null");
                }
                Debug.Log($"Remote colider: {cube.OutgoingRemoteGameObject.ColliderType}");
            }
            else
            {
                Debug.Log("RemoteGameObject is null");
            }

            printedDebug = true;
        }
    }
}
