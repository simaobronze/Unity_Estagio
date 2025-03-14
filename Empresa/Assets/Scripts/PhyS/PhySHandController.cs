using MAGES.RemotePhysics.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhySHandController : MonoBehaviour
{
    private Transform _transformToFollow;
    private DissectedGameObject _dissectedGameObject;
    private DissectedGameObject _dissectedGameObjectChild;

    // Start is called before the first frame update
    void Start()
    {
        _transformToFollow = GetTransformToFollow();
        _dissectedGameObject = GetComponent<DissectedGameObject>();
        //_dissectedGameObjectChild = GetComponentInChildren<DissectedGameObject>();

        setColisionListenners();
        Debug.Log($"{name} : {_dissectedGameObject.Object_id}");
    }

    // Update is called once per frame
    void Update()
    {
        //_dissectedGameObject.transform.position = _transformToFollow.position;
        _dissectedGameObject.ExportPhysicsComponentContainer(PhysicsComponentContainerType.Create);
        //_dissectedGameObject.RequestChangeObjectOwnership(true);
        _dissectedGameObject.SetMoveTransform(_transformToFollow.position,_transformToFollow.rotation);
    }

    private Transform GetTransformToFollow()
    {
        if (name[0] == 'L')
        {
            Debug.Log("Left hand");
            return GameObject.Find("HandInteractorL").transform;
        }
        else
        {
            Debug.Log("Right hand");
            return GameObject.Find("HandInteractorR").transform;
        }
    }

    private void setColisionListenners()
    {
        _dissectedGameObject.GetRemoteCollider().OnCollisionEnter.AddListener(() =>
        {
            Debug.Log($"{name} colision ENTER");
        });
        _dissectedGameObject.GetRemoteCollider().OnCollisionStay.AddListener(() =>
        {
            Debug.Log($"{name} colision STAY");
        });
        _dissectedGameObject.GetRemoteCollider().OnCollisionExit.AddListener(() =>
        {
            Debug.Log($"{name} colision EXIT");
        });
    }
}
