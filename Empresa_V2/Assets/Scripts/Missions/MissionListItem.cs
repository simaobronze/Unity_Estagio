using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionListItem : MonoBehaviour, IPointerClickHandler
{
    private Mission _mission;
    [SerializeField] private TextMeshProUGUI _missionName;
    [SerializeField] private MissionsScriptableObject _missionsStore;
    
    private Transform _content;
    private Toggle _toggle;

    // Start is called before the first frame update
    void Start()
    {
        if(!TryGetComponent(out _toggle))
        {
            Debug.LogError($"Error getting {typeof(Toggle).Name}");
        }
        _content = gameObject.transform.parent;
    }

    private void OnEnable()
    {
        _missionsStore.missionChangeEvent.AddListener(HandleMissionUpdate);
    }

    private void OnDisable()
    {
        _missionsStore.missionChangeEvent.RemoveListener(HandleMissionUpdate);
    }

    private void HandleMissionUpdate(Mission mission)
    {
        if (_toggle == null) { return; }
        if (mission == null) 
        { 
            _toggle.isOn = false; 
            return; 
        }
        _toggle.isOn = mission.id == _mission.id;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Mission mission = _mission;
        if (_missionsStore.Mission != null && _missionsStore.Mission.id == _mission.id)
        {
            mission = null;
        }
        _missionsStore.ShowMission(mission);
    }

    public void SetMission(Mission mission)
    {
        _mission = mission;
        _missionName.text = _mission.name;
    }
}
