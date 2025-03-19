//using Microsoft.Geospatial;
//using Microsoft.Maps.Unity;
//using Oculus.Interaction.Samples;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UserController : MonoBehaviour
{
    public User User { get; private set; }
    //private MapPin _mapPin;
    private PinDrone _pinDrone;
    [SerializeField] GameObject _details;
    private GameObject _player;
    private GameObject _labelGO;
    [SerializeField] TextMeshPro _nameTMP;
    [SerializeField] int _id;
    [SerializeField] string _name;
    [SerializeField] private UsersScriptableObject _usersScriptableObject;

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out _pinDrone))
        {
            Debug.Log($"No {typeof(PinDrone)} component available");
        }
        /*
        if (!TryGetComponent(out _mapPin))
        {
            Debug.LogError($"Error getting {typeof(ClusterMapPin).Name} component");
        }
        */
        if (_nameTMP != null)
        {
            _labelGO = _nameTMP.transform.parent.gameObject;
        }
        _player = GameObject.Find("OVRPlayerController");
    }

    // Update is called once per frame
    void Update()
    {
        if (_labelGO != null)
        {
            if (_player != null)
            {
                var target = new Vector3(_player.transform.position.x, _labelGO.transform.position.y, _player.transform.position.z);
                var rotation = Quaternion.LookRotation(target - _labelGO.transform.position).eulerAngles;
                rotation.x -= 180;
                _labelGO.transform.rotation = Quaternion.Slerp(_labelGO.transform.rotation, Quaternion.Euler(rotation), Time.deltaTime);
                //_labelGO.transform.rotation = Quaternion.Euler(_labelGO.transform.rotation.x - 180, _labelGO.transform.rotation.y, _labelGO.transform.rotation.z - 180);
            }
        }
    }

    private void OnEnable()
    {
        _usersScriptableObject.usersUpdateEvent.AddListener(UpdateUser);
        _usersScriptableObject.usersGeoUpdateEvent.AddListener(UpdateMapPosition);
    }

    public void SetUser(User user)
    {
        User = user;
        if (_nameTMP != null)
        {
            if (_nameTMP.text != user.name) 
            {
                _nameTMP.text = user.name;
            }
        }
    }

    private void UpdateUser(User user)
    {
        if (user.id == User.id)
        {
            User = user;
            _id = user.id;
            _name = user.name;
        }
    }

    private void UpdateMapPosition(User user)
    {
        try
        {
            //Debug.Log($"User Map Position: {user.id}");

            if (user == null || user.sensors == null || this.User.id != user.id)
            {
                return;
            }
            if (!user.sensors.TryGetValue("Geo", out Dictionary<string, UserSensor> sensors))
            {
                return;
            }
            if (!sensors.TryGetValue("data", out UserSensor geoSensor))
            {
                return;
            }
            /*
            if (_mapPin != null)
            {
                if (geoSensor.location.Value.LatLon.LatitudeInDegrees != 0 && geoSensor.location.Value.LatLon.LongitudeInDegrees != 0)
                {
                    _mapPin.Location = geoSensor.location.Value.LatLon;
                }
                if (geoSensor.location.Value.AltitudeInMeters != 0)
                {
                    _mapPin.Altitude = geoSensor.location.Value.AltitudeInMeters;
                }
            }
            if (_pinDrone != null)
            {
                _pinDrone.UpdateDroneAltitude(geoSensor.location.Value.AltitudeInMeters);
            }
            */
		} catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
