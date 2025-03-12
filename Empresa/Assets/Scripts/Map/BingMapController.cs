using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserTypePin
{
    public string type;
    public GameObject prefab;
}


public class BingMapController : MonoBehaviour
{
    // ! vars related to Bing Map SDK
    // private MapRenderer _map;
    // private MapPinLayer [] _pinLayer;
    // ! var only for debug
    //[SerializeField] List<Geo> locations;

    [Header("Prefabs for user types")]
    [SerializeField] private List<UserTypePin> _pinPrefabs;

    [Header("Stores")]
    [SerializeField] private MissionsScriptableObject _missionsStore;
    [SerializeField] private UsersScriptableObject _usersStore;

    void Start()
    {
        /*
        _pinLayer = GetComponents<MapPinLayer>();
        if (!TryGetComponent(out _map) || _pinLayer.Length == 0)
        {
            Debug.LogError("Map or pin layer not found");
            return;
        }
        */
    }

    private void OnEnable()
    {
        _missionsStore.missionChangeEvent.AddListener(UpdateCenter);
        _usersStore.usersFetchedEvent.AddListener(CreateUsersPins);
    }

    private void CreateUsersPins(Dictionary<int, User> u)
    {
        Dictionary<int, User> users = new(u);
        foreach (var user in users)
        {
            if (user.Value.sensors.Count <= 0 || !user.Value.sensors.ContainsKey("Geo"))
            {
                continue;
            }
            GameObject newPin = null;

            foreach (UserTypePin userTypePin in _pinPrefabs)
            {
                if (newPin != null)
                {
                    break;
                }
                if (newPin == null)
                {
                    foreach (var role in user.Value.roles)
                    {
                        if (newPin != null)
                        {
                            break;
                        }
                        if (userTypePin.type.ToLower() == role.label.ToLower())
                        {
                            newPin = userTypePin.prefab;
                            break;
                        }
                    }
                }
                if (newPin != null)
                {
                    break;
                }
                
            }
            if (newPin == null)
            {
                foreach (UserTypePin userTypePin in _pinPrefabs)
                {
                    if (user.Value.data_configs.portal.type != null)
                    {
                        if (userTypePin.type.ToLower() == user.Value.data_configs.portal.type.ToLower())
                        {
                            newPin = userTypePin.prefab;
                        }
                    }
                }
            }

            if (newPin == null)
            {
                newPin = _pinPrefabs.Find((pin) => pin.type == "default")?.prefab;
                if (newPin == null)
                {
                    continue;
                }
            }

            if (newPin != null) {
                GameObject userPin = Instantiate(newPin);
			    if (!userPin.TryGetComponent(out UserController userController))
                {
                    Destroy(userPin);
                    continue;
                }
                userController.SetUser(user.Value);
                /*
                if (!userPin.TryGetComponent(out MapPin pin))
                {
                    Destroy(userPin);
                    continue;
                }
                pin.Location = new LatLon(0, 0);
                foreach(var layer in _pinLayer)
                {
                    if(layer.LayerName.ToLower() == userPin.name.ToLower())
                    {
					    layer.MapPins.Add(pin);
				    }
                }
                userPin.transform.parent = _map.transform;
                userPin.SetActive(true);
                */
			}
		}
    }

    public void UpdateCenter(Mission mission)
    {
        if (mission == null || mission.data_configs == null || mission.data_configs.lat == 0 || mission.data_configs.lon == 0 || mission.data_configs.zoom == 0)
        {
            return;
        }
        //_map.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(mission.data_configs.lat, mission.data_configs.lon), mission.data_configs.zoom));
    }
}
