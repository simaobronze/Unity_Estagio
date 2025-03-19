using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionFlightPlanItem : MonoBehaviour, IPointerClickHandler
{
    private FlightPlans _flightPlan;
    private Toggle _toggle;

    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] MissionsScriptableObject _missionStore;
    [SerializeField] ImagesScriptableObject _imagesStore;

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out _toggle))
        {
            Debug.LogError($"Error getting {typeof(Toggle).Name}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _missionStore.flightPlanUpdateEvent.AddListener(HandleFlightPlan);
    }

    private void OnDisable()
    {
        _missionStore.flightPlanUpdateEvent.RemoveListener(HandleFlightPlan);
    }

    private void HandleFlightPlan(FlightPlans flightPlan)
    {
        if (_toggle == null) 
        {
            return; 
        }
        if (flightPlan == null)
        {
            _toggle.isOn = false;
            return;
        }
        _toggle.isOn = flightPlan.id == _flightPlan.id;
    }

    internal void SetFlightPlan(FlightPlans flightPlan)
    {
        _flightPlan = flightPlan;
        _name.text = flightPlan.name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FlightPlans flightPlan = _flightPlan;
        if (_missionStore.Mission == null && _missionStore.FlightPlan.id == _flightPlan.id)
        {
            flightPlan = null;
        }
        _missionStore.ShowFlightPlan(flightPlan);
    }
}
