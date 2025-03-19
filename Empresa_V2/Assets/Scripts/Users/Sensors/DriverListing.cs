using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class DriverListing : MonoBehaviour
{
    [SerializeField] private Transform _driversList;
    [SerializeField] private GameObject _driverPrefab;
    [SerializeField] private UsersScriptableObject _usersStore;

    private Dictionary<string, GameObject> _driversListed = new();

    [SerializeField] private List<string> _skippableDrivers = new()
    {
        "Geo",
        "Video",
        "Audio",
    };

    private User _User = null;
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
        //if (IsUserNull()) { return; }
        //ListDrivers();
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
        _User = null;
        IsUserNull();
        _User = user;
    }

    private bool IsUserNull()
    {
        try
        {
            if (_User == null)
            {
                if (_driversListed.Count > 0)
                {
                    try
                    {
                        CleanDrivers();
                    } catch (Exception e)
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
            if (_User.sensors == null) { return; }
            foreach (var driver in _User.sensors)
            {
                if (_skippableDrivers.Contains(driver.Key)) { continue; }
                if (_driversListed.ContainsKey(driver.Key))
                {
                    continue;
                }
                try
                {
                    CreateDriver(driver.Key);
                } catch (Exception e)
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
        sensorsListing.SetDriver(_User, driverName);
        _driversListed.Add(driverName, driverGO);
    }

    private void UpdateSensorsList(User user)
    {
        if (_User == null) { return; }
        if (_User.id != user.id) { return; }
        _User = user;
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
