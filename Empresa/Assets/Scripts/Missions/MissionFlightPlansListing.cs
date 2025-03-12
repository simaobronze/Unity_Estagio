using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFlightPlansListing : MonoBehaviour
{
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject _missionFlightPlanItemPrefab;
    [SerializeField] private GameObject _emptyListPrefab;
    [SerializeField] private MissionsScriptableObject _missionStore;
    [SerializeField] private ImagesScriptableObject _imageStore;

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
        _missionStore.missionChangeEvent.AddListener(UpdateList);
    }

    private void OnDisable()
    {
        _missionStore.missionChangeEvent.RemoveListener(UpdateList);
    }

    private void CleanList()
    {
        for (int i = 0; i < scrollViewContent.childCount; i++)
        {
            Destroy(scrollViewContent.GetChild(i).gameObject);
        }
    }

    private void UpdateList(Mission mission)
    {
        CleanList();
        if (mission == null) 
        {
            return;
        }
        if (mission.flightPlans.Count == 0)
        {
            Instantiate(_emptyListPrefab, scrollViewContent);
            return;
        }
        foreach (var flightPlan in mission.flightPlans)
        {
            GameObject go = Instantiate(_missionFlightPlanItemPrefab, scrollViewContent);
            if (!go.TryGetComponent(out MissionFlightPlanItem item))
            {
                Debug.LogError($"Error getting component {typeof(MissionFlightPlanItem).Name}");
                Destroy(go);
                continue;
            }
            item.SetFlightPlan(flightPlan.Value);
        }
    }
}
