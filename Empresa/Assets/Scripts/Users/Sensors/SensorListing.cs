using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SensorListing : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _driverName;
    [SerializeField] private Transform _list;
    [SerializeField] private GameObject _sensorItemPrefab;
    [SerializeField] private UsersScriptableObject _usersStore;

    public string Driver { get; private set; } = null;
    public User User { get; private set; } = null;

    private Dictionary<string, GameObject> _sensorsListed = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ListSensors();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        _usersStore.usersUpdateEvent.RemoveListener(UpdateSensorsList);
    }

    internal void SetDriver(User user, string driver)
    {
        User = user;
        Driver = driver;
        _driverName.text = driver;
        _usersStore.usersUpdateEvent.AddListener(UpdateSensorsList);
    }

    private void UpdateSensorsList(User user)
    {
        if (user == null || User == null) { return; }
        if (User.id != user.id) { return; }
        User = user;
    }

    private void ListSensors()
    {
        if (User == null || Driver == null) { return; }
        foreach (var sensor in User.sensors[Driver])
        {
            try
            {
                if (_sensorsListed.ContainsKey(sensor.Key)) { continue; }
                CreateSensor(sensor);
            }
            catch (Exception e)
            {
                Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error creating sensor object, e - {e.Message}");
            }
        }
    }

    private void CreateSensor(KeyValuePair<string, UserSensor> sensor)
    {
        GameObject sensorGO;
        try
        {
            sensorGO = Instantiate(_sensorItemPrefab, _list);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error instantiating sensor prefab, e - {e.Message}");
            return;
        }
        if (!sensorGO.TryGetComponent(out UserDeviceSensor deviceSensor))
        {
            Debug.LogError("SensorListing: UserDeviceSensor component not found in driver prefab");
            Destroy(sensorGO);
            return;
        }
        deviceSensor.SetSensor(User.id, Driver, sensor.Key);
        _sensorsListed.Add(sensor.Key, sensorGO);
    }
}
