using UnityEngine;
using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UnityEngine.UI;
using MQTTnet.Client;
using System.Threading.Tasks;
using MQTTnet;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

public class DroneInfo : MonoBehaviour
{
    public Text displayText;

    [SerializeField] private Transform _driversList;
    [SerializeField] private GameObject _driverPrefab;
    [SerializeField] private UsersScriptableObject _usersStore;

    private User _user = null;
    private Dictionary<string, GameObject> _driversListed = new();
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    private MQTTClient _mqttClient;
    private IApplicationMessageReceiver _messageReceiver;



    void Start()
    {
    }

    private void Update()
    {
        if (_user != null)
        {
            displayText.text = _user.name;
        }
    }

    private void OnEnable()
    {
        _usersStore.showUserEvent.AddListener(SetUser);
        _usersStore.usersUpdateEvent.AddListener(UpdateSensorsList);
    }

    private void OnDisable()
    {
        _usersStore.showUserEvent.AddListener(SetUser);
        _usersStore.usersUpdateEvent.RemoveListener(UpdateSensorsList);
    }

    private void SetUser(User user)
    {
        _user = null;
        IsUserNull();
        _user = user;
    }

    private bool IsUserNull()
    {
        try
        {
            if (_user == null)
            {
                if (_driversListed.Count > 0)
                {
                    try
                    {
                        CleanDrivers();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error cleaning drivers list, e - {e.Message}");
                    }
                }

                return true;

            }
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error checking user in drivers list, e - {e.Message}");
            return true;
        }
    }

    private void ListDrivers()
    {
        try
        {
            if (_user.sensors == null) { return; }
            foreach (var driver in _user.sensors)
            {
                //if (_skippableDrivers.Contains(driver.Key)) { continue; }
                if (_driversListed.ContainsKey(driver.Key))
                {
                    continue;
                }
                try
                {
                    CreateDriver(driver.Key);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error creating driver, e - {e.Message}");
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_driversList.GetComponent<RectTransform>());
        }
        catch (Exception e)
        {
            Debug.LogError($"Error updating sensors list, e - {e.Message}");
        }
    }

    private void CreateDriver(string driverName)
    {
        GameObject driverGO;
        try
        {
            driverGO = Instantiate(_driverPrefab, _driversList);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error instantiating driver prefab, e - {e.Message}");
            return;
        }
        if (!driverGO.TryGetComponent(out SensorListing sensorsListing))
        {
            Debug.LogError("DriverListing: SensorListing component not found in driver prefab");
            Destroy(driverGO);
            return;
        }
        sensorsListing.SetDriver(_user, driverName);
        _driversListed.Add(driverName, driverGO);
    }

    private void UpdateSensorsList(User user)
    {
        if (user == null) { return; }
        if (_user.id != user.id) { return; }
        _user = user;
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() =>
            {
                ListDrivers();
            });
        }
        //foreach (var driver in _User.sensors)
        //{
        //    if (!_driversListed.ContainsKey(driver.Key) && !_skippableDrivers.Contains(driver.Key))
        //    {

        //    }
        //}
    }

    private void CleanDrivers()
    {
        foreach (var driver in _driversListed)
        {
            try
            {
                Destroy(driver.Value);
            }
            catch (Exception e)
            {
                Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error removing/destroying driver object, e - {e.Message}");
            }
        }
        _driversListed.Clear();
    }
}

