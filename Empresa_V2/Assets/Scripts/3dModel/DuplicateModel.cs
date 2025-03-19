using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DuplicateModel : MonoBehaviour
{
    private bool canSpawn = true;
    [SerializeField] Vector3 _spawnLocation;
    [SerializeField] Vector3 _spawnRotation;

    //! GameObjects
    [SerializeField] GameObject _toDuplicate;
    
    [SerializeField] Material _notHover;
    [SerializeField] Material _hover;

    // Start is called before the first frame update
    void Start()
    {
        if ( _toDuplicate == null )
        {
            Debug.LogError($"Error {nameof(_toDuplicate)} is null");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void DuplicateModelOnClick()
    {
        if (!canSpawn)
        {
            return;
        }
        canSpawn = false;
        GameObject go = Instantiate(_toDuplicate, _spawnLocation, Quaternion.Euler(_spawnRotation));
        if (go.TryGetComponent(out LoadModel model))
        {
            model._handle.SetActive(true);
            model._grabGrab.gameObject.SetActive(true);
            model._rayGrab.gameObject.SetActive(true);
            model._collider.gameObject.SetActive(false);
        }
    }
}
