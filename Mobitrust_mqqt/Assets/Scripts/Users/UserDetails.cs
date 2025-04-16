//using Microsoft.Geospatial;
//using Microsoft.Maps.Unity;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserDetails : MonoBehaviour
{
    [SerializeField] UsersController _usersController;
    [SerializeField] private TextMeshPro _userName;
    [SerializeField] private TextMeshPro _userTeam;
    [SerializeField] private TextMeshPro _userMission;
    [SerializeField] private TextMeshPro _userDevice;
    [SerializeField] private Image _userPfp;
    [SerializeField] private GameObject _userMap;
    [SerializeField] private UsersScriptableObject _usersScriptableObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        _usersScriptableObject.showUserEvent.AddListener(UpdateUserDetails);
    }

    private void OnDisable()
    {
        _usersScriptableObject.showUserEvent.RemoveListener(UpdateUserDetails);
    }

    public void UpdateUserDetails(User user)
    {
        if (user != null)
        {
            Debug.Log($"User id: {user.id}");

            //StartCoroutine(_usersController.FetchUser(user.id));
            int aux = 0;
            _userName.text = user.name;
            _userTeam.text = null;
            if (user.teams.Count != 0)
            {
                foreach (var team in user.teams)
                {
                    _userTeam.text += team.name;
                    if (user.teams.Count - 1 != aux)
                        _userTeam.text += ", ";
                    aux++;
                }
            }
            else
            {
                _userTeam.text = "N/A";
            }
            _userMission.text = null;
            aux = 0;
            if (user.missions.Count != 0)
            {
                foreach (var mission in user.missions)
                {
                    _userMission.text += mission.name;
                    if (user.devices.Count - 1 != aux)
                        _userDevice.text += ", ";
                    aux++;
                }
            }
            else
            {
                _userMission.text = "N/A";
            }
            _userDevice.text = null;
            aux = 0;
            if (user.devices.Count != 0)
            {
                foreach (var device in user.devices)
                {
                    _userDevice.text += device.uuid;
                    if (user.devices.Count - 1 != aux)
                        _userDevice.text += ", ";
                    aux++;
                }
            }
            else
            {
                _userDevice.text = "N/A";
            }
            string url = user?.data_configs?.pfp?.url;
            if (url != null && url != "")
            {
                StartCoroutine(UsersController.FetchUserPFP(url, (sprite) =>
                {
                    _userPfp.sprite = sprite;
                }));
            }
            SetMap(user);
        }
    }

    private void SetMap(User user)
    {
        if (_userMap == null)
        {
            return;
        }
        /*
		if (!_userMap.TryGetComponent(out  MapRenderer map))
		{
			Debug.LogError("Error getting component bingmapcontroller");
			return;
		}
		*/
        if (user == null || user.data_configs == null || user.data_configs.portal == null || user.data_configs.portal.geo == null ||
            user.data_configs.portal.geo.lat == 0 || user.data_configs.portal.geo.lon == 0 || user.data_configs.portal.geo.zoom == 0)
        {
            return;
        }
        //map.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(user.data_configs.portal.geo.lat, user.data_configs.portal.geo.lon), user.data_configs.portal.geo.zoom));

    }
}
