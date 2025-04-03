using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MissionsScriptableObject", menuName = "ScriptableObjects/Missions")]
public class MissionsScriptableObject : ScriptableObject
{
    public FlightPlans FlightPlan { get; private set; } = null;
    public Mission Mission { get; private set; } = null;
    public Dictionary<int, Mission> Missions { get; private set; } = new();

    [System.NonSerialized]
    public UnityEvent<Mission> missionChangeEvent;
    [System.NonSerialized]
    public UnityEvent<FlightPlans> flightPlanUpdateEvent;
    [System.NonSerialized]
    public UnityEvent<Dictionary<int, Mission>> missionsFetchEvent;
    [System.NonSerialized]
    public UnityEvent<int> updateMissionFlightPlansEvent;

    private void OnEnable()
    {
        flightPlanUpdateEvent ??= new();
        missionChangeEvent ??= new UnityEvent<Mission>();
        missionsFetchEvent ??= new UnityEvent<Dictionary<int, Mission>>();
        updateMissionFlightPlansEvent ??= new();
    }

    private void OnDisable()
    {
        flightPlanUpdateEvent = null;
        missionChangeEvent = null;
        missionsFetchEvent = null;
        updateMissionFlightPlansEvent = null;
    }

    public void ShowFlightPlan(FlightPlans flightPlan)
    {
        FlightPlan = flightPlan;
        flightPlanUpdateEvent.Invoke(flightPlan);
    }

    public void ShowMission(Mission mission)
    {
        Mission = mission;
        flightPlanUpdateEvent.Invoke(null);
        missionChangeEvent.Invoke(mission);
    }

    public void FetchedMissions(Dictionary<int, Mission> missions)
    {
        Missions = missions;
        missionsFetchEvent.Invoke(missions);
    }

    internal void AddImageModel(int mission_id, string link_3d)
    {
        if (!Missions.TryGetValue(mission_id, out Mission mission))
        {

        }
    }

    internal void UpdateFlightPlans(int missionId)
    {
        updateMissionFlightPlansEvent.Invoke(missionId);
    }

    internal void UpdateMission(int id, Mission mission)
    {
        Missions[id] = mission;
        MissionsUpdated();
    }

    internal void MissionsUpdated()
    {
        missionsFetchEvent?.Invoke(Missions);
    }
}
