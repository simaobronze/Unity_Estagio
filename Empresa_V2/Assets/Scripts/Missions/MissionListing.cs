using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissionListing : MonoBehaviour
{
    [SerializeField] private MissionsController _missionsController;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject _missionItemPrefab;
    [SerializeField] private MissionsScriptableObject _missionsScriptableObject;

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
        _missionsScriptableObject.missionsFetchEvent.AddListener(UpdateMissions);
    }

    private void OnDisable()
    {
        _missionsScriptableObject.missionsFetchEvent.RemoveListener(UpdateMissions);
    }

    public void UpdateMissions(Dictionary<int, Mission> missions)
    {
        foreach (var mission in missions.Values)
        {
            GameObject newMissionItem = Instantiate(_missionItemPrefab, scrollViewContent);
            if (newMissionItem.TryGetComponent(out MissionListItem item))
            {
                item.SetMission(mission);
            }
        }
    }

    internal void ShowMission(Mission mission)
    {
        StartCoroutine(_missionsController.FetchMission(mission.id));
    }
}
