using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;

public class MapGrab : MonoBehaviour
{
    private bool _isGrabbed = false;
    private Vector3 _defaultPosition;
    private Vector3 _lastPosition;
    private double _minMovement;
    [SerializeField]
    private AbstractMap _abstractMap;
    [SerializeField]
    private MapController _mapController;

    // Start is called before the first frame update
    void Start()
    {
        _defaultPosition = transform.position;
        _lastPosition = transform.position;
        _minMovement = 0.008;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGrabbed) {
            Vector3 movement = transform.position - _lastPosition;

            if (movement.z >= _minMovement)
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x - 0.0001, _abstractMap.CenterLatitudeLongitude.y);
            else if (movement.z <= -_minMovement)
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x + 0.0001, _abstractMap.CenterLatitudeLongitude.y);

            if (movement.x >= _minMovement)
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x, _abstractMap.CenterLatitudeLongitude.y - 0.0001);
            else if (movement.x <= -_minMovement)
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x, _abstractMap.CenterLatitudeLongitude.y + 0.0001);

        }
        else { 
            transform.position = _defaultPosition;
        }

        _lastPosition = transform.position;
    }

    public void toggleGrab()
    {
        _isGrabbed = !_isGrabbed;
    }
}
