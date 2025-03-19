//using Microsoft.Maps.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserMapPin : MonoBehaviour
{
    //private MapRenderer _mapRenderer;
    private LineRenderer _lineRenderer;
    private GameObject _player;
    [SerializeField] TextMeshProUGUI _label;
    [SerializeField] GameObject _model;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (!transform.parent.TryGetComponent(out _mapRenderer))
        {
            Debug.LogError($"Error getting {typeof(MapRenderer).Name} component");
            Destroy(this);
        }
        if (!TryGetComponent(out _lineRenderer))
        {
            Debug.LogError($"Error getting {typeof(LineRenderer).Name} component");
            Destroy(this);
        }
        if (_model == null)
        {
            Debug.LogError($"3D model reference not set");
            Destroy(this);
        }
        */
        _player = GameObject.Find("CenterEyeAnchor");

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (_mapRenderer == null || _lineRenderer == null || _model == null)
        {
            Destroy(this);
            return;
        }
        Ray ray = new(_model.transform.position, -gameObject.transform.up);
        if (_mapRenderer.Raycast(ray, out MapRendererRaycastHit hit))
        {
            _lineRenderer.enabled = true;
            Vector3 goPos = _model.transform.position;
            Vector3 mapPos = hit.Point;
            _lineRenderer.SetPosition(0, goPos);
            _lineRenderer.SetPosition(1, mapPos);
        }
        */
        if (_label != null)
        {
            _label.transform.LookAt(_player.transform);
        }
    }
}
