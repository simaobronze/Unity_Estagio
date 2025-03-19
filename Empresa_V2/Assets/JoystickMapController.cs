using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAGES;
using MAGES.Interaction.Interactables;
using MAGES.Interaction.Interactors;
//using MAGES.RemotePhysics;
//using MAGES.RemotePhysics.Runtime;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;

public class JoystickMapController : MonoBehaviour
{
    private const float k_rotationLimit = 40f;
    private const float k_deadZone = 5f;
    private const float k_grabbableMaxDistance = 0.7f;
    private const float k_axisDeadZone = 1f;
    private const string k_stopCommand = "stop_movement";

    [SerializeField]
    private GameObject _joyStick;
    [SerializeField]
    private Rigidbody _joyStickRigidBody;
    [SerializeField]
    private StreamsScriptableObject _streamScriptableObject;
    [SerializeField]
    private AbstractMap _abstractMap;
    [SerializeField]
    private MapController _mapController;
    //[SerializeField]
    //private DissectedGameObject _dissectedGameObject;

    private HandInteractor _lController;
    private HandInteractor _rController;

    private Grabbable _grabbable;
    private bool _isGrabbed = false;

    private MQTTClient _mqttClient;
    private string _lastCommandSent = k_stopCommand;

    void Start()
    {
        _lController = GameObject.Find("HandInteractorL").GetComponent<HandInteractor>();
        _rController = GameObject.Find("HandInteractorR").GetComponent<HandInteractor>();
        _joyStick = GameObject.Find("JoyStickMap");
        _grabbable = _joyStick.GetComponent<Grabbable>();
        _mqttClient = FindAnyObjectByType<MQTTClient>();
    }

    void Update()
    {
        string droneCommand;
        Vector3 currentJoystickRotation = NormalizeAngles(_joyStick.transform.rotation.eulerAngles);
        ConstraintJoystickRotation(currentJoystickRotation);

        //Debug.Log("Command -> " + currentJoystickRotation);
        //Debug.Log($"Command -> Set grabbed {_isGrabbed}");
        if (
            /*(Math.Abs(currentJoystickRotation.x) > k_deadZone
            || Math.Abs(currentJoystickRotation.z) > k_deadZone) &&*/
            _isGrabbed)
        {
            droneCommand = GetJoystickOrientation(currentJoystickRotation);
            DragMap(droneCommand);
        }
        ToggleGrabbable();
    }

    private void ToggleGrabbable()
    {
        if (
            Vector3.Distance(_lController.transform.position, _joyStick.transform.position) > k_grabbableMaxDistance
            || Vector3.Distance(_rController.transform.position, _joyStick.transform.position) > k_grabbableMaxDistance
            )
        {
            if (_lController.HasActivation)
            {
                _lController.SelectExit(_grabbable);
            }
            else if (_rController.HasActivation)
            {
                _rController.SelectExit(_grabbable);
            }
        }
    }

    private void ConstraintJoystickRotation(Vector3 currentJoystickRotation)
    {
        //Debug.Log("CurrentJoystickRotation -> " + _joyStickTransform.eulerAngles);
        if (currentJoystickRotation.x < -k_rotationLimit || currentJoystickRotation.x > k_rotationLimit)
        {
            float clampedX = ClampedAngle(currentJoystickRotation.x, -k_rotationLimit, k_rotationLimit);
            currentJoystickRotation.x = clampedX;
        }
        if (currentJoystickRotation.z < -k_rotationLimit || currentJoystickRotation.z > k_rotationLimit)
        {
            float clampedX = ClampedAngle(currentJoystickRotation.z, -k_rotationLimit, k_rotationLimit);
            currentJoystickRotation.z = clampedX;
        }
        _joyStick.transform.rotation = Quaternion.Euler(currentJoystickRotation);
    }

    private float ClampedAngle(float angle, float min, float max)
    {
        return Mathf.Clamp(angle, min, max);
    }

    private Vector3 NormalizeAngles(Vector3 rotation)
    {
        if (rotation.x > 180)
        {
            rotation.x -= 360;
        }
        if (rotation.z > 180)
        {
            rotation.z -= 360;
        }
        return rotation;
    }

    private string GetJoystickOrientation(Vector3 currentJoystickRotation)
    {
        Vector2 direction = new Vector2(currentJoystickRotation.z, currentJoystickRotation.x);
        //Debug.Log("command -> " + direction);

        if (Math.Abs(direction.x - direction.y) < k_axisDeadZone)
        {
            return "stop_movement";
        }

        if (direction.x > direction.y)
        {
            if (-direction.x > direction.y)
            {
                return "up";
            }
            else
            {
                return "right";
            }
        }
        else if (direction.x < direction.y)
        {
            if (-direction.x > direction.y)
            {
                return "left";
            }
            else
            {
                return "down";
            }
        }
        else
        {
            return "stop_movement";
        }
    }

    public void SetGrabbed()
    {
        _isGrabbed = !_isGrabbed;
    }

    private void DragMap(string direction)
    {
        Debug.Log(direction);
        switch (direction)
        {
            case "up":
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x + 0.001, _abstractMap.CenterLatitudeLongitude.y);
                break;
            case "down":
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x - 0.001, _abstractMap.CenterLatitudeLongitude.y);
                break;
            case "left":
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x, _abstractMap.CenterLatitudeLongitude.y - 0.001);
                break;
            case "right":
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x, _abstractMap.CenterLatitudeLongitude.y + 0.001);
                break;
        }
    }
}
