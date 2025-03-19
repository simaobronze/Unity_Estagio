using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour
{
    [SerializeField]
    private GameObject _mapGameObject;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private GameObject _grabObject;

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10, layerMask))
        {
            if (hit.distance > 2.80)
            {
                _mapGameObject.transform.position += new Vector3(0, 0.04f, 0);
            }
            else if (hit.distance < 2.74)
            {
                _mapGameObject.transform.position -= new Vector3(0, 0.04f, 0);
            }
            else
            {
                _grabObject.transform.position = new Vector3(_grabObject.transform.position.x, _mapGameObject.transform.position.y + 0.1f, _grabObject.transform.position.z);
            }
        }
    }
}