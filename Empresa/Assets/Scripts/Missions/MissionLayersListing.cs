using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionLayersListing : MonoBehaviour
{
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject _missionLayerPrefab;
    [SerializeField] private GameObject _emptyListPrefab;
    [SerializeField] private ImagesScriptableObject _imagesStore;
    [SerializeField] private MissionsScriptableObject _missionsStore;

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
        _missionsStore.flightPlanUpdateEvent.AddListener(UpdateMissionLayers);
    }

    private void OnDisable()
    {
        _missionsStore.flightPlanUpdateEvent.AddListener(UpdateMissionLayers);
    }

    private void CleanList()
    {
        for (int i = 0; i < scrollViewContent.childCount; i++)
        {
            Destroy(scrollViewContent.GetChild(i).gameObject);
        }
    }

    private void UpdateMissionLayers(FlightPlans flightPlan)
    {
        CleanList();
        if (flightPlan == null)
        {
            Instantiate(_emptyListPrefab, scrollViewContent);
            return;
        }
        List<WMS_Layer> layers = flightPlan.wmsLayers;
        if (layers == null || layers.Count < 0)
        {
            Instantiate(_emptyListPrefab, scrollViewContent);
            return;
        }
        foreach (var layer in layers)
        {
            if (layer == null || layer.link_3d == null)
            {
                continue;
            }
            GameObject go = Instantiate(_missionLayerPrefab, scrollViewContent);
            if (!go.TryGetComponent(out MissionLayerItem missionLayer))
            {
                Debug.LogError($"Error getting component {typeof(MissionLayerItem).Name}");
                Destroy(go);
                continue;
            }
            missionLayer.SetLayer(layer);
        }
    }
}
