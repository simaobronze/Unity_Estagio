using System;
using TMPro;
using UnityEngine;

public class MissionDetails : MonoBehaviour
{
    private SessionManager _sessionManager;
    [SerializeField] private TextMeshPro _missionName;
    [SerializeField] private TextMeshPro _missionStatus;
    [SerializeField] private TextMeshPro _missionDescription;
    [SerializeField] private TextMeshPro _missionTeams;
    [SerializeField] private GameObject _missionMap;
    [SerializeField] private MissionsScriptableObject _missionsScriptableObject;

    // Start is called before the first frame update
    void Start()
    {
        _sessionManager = FindObjectOfType<SessionManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        _missionsScriptableObject.missionChangeEvent.AddListener(UpdateMissionDetails);
    }

    private void OnDisable()
    {
        _missionsScriptableObject.missionChangeEvent.RemoveListener(UpdateMissionDetails);
    }

    public void UpdateMissionDetails(Mission mission)
    {
        if (mission != null)
        {
            Debug.Log($"Mission id: {mission.id}");
            //StartCoroutine(_sessionManager.GetMissionsController().FetchMission(mission.id);
            _missionName.text = mission.name;
            _missionDescription.text = mission.description;
            _missionStatus.text = $"Status: {mission.data_configs.status}";
            SetMap(mission);
        }
    }

    private void SetMap(Mission mission)
    {
        if (mission == null)
        {
            return;
        }
        if (_missionMap == null)
        {
            return;
        }
        if (!_missionMap.TryGetComponent(out BingMapController map))
        {
            Debug.LogError("Error getting component bingmapcontroller");
            return;
        }
        map.UpdateCenter(mission);
    }
}
