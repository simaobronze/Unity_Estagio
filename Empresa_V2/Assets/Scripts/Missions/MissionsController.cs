using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class MissionsController : MonoBehaviour
{

    [SerializeField] private UsersScriptableObject _usersStore;
    [SerializeField] private MissionsScriptableObject _missionsStore;

    private SessionManager _sessionManager;
    //private int updateMission = -1;
    //private int updateMissionFlightPlans = -1;

    private static readonly Queue<Action> _executionQueue = new Queue<Action>();


    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out _sessionManager))
        {
            Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error getting component SessionManager");
            Application.Quit();
        }
    }

    private void Update()
    {
        lock ( _executionQueue )
        {
            while ( _executionQueue.Count > 0 ) { _executionQueue.Dequeue().Invoke(); }
        }
        //    if (updateMission != -1)
        //    {
        //        // ! DO STUFF
        //        updateMission = -1;
        //    }
        //    if (updateMissionFlightPlans != -1)
        //    {
        //        try
        //        {

        //            updateMissionFlightPlans = -1;
        //        }

        //    }
    }

    private void OnEnable()
    {
        _missionsStore.updateMissionFlightPlansEvent.AddListener(HandleUpdateFlightPlans);
    }

    private void OnDisable()
    {
        _missionsStore.updateMissionFlightPlansEvent.RemoveListener(HandleUpdateFlightPlans);
    }

    private void HandleUpdateFlightPlans(int missionId)
    {
        lock ( _executionQueue )
        {
            _executionQueue.Enqueue(() =>
            {
                try
                {
                    StartCoroutine(FetchMissionFlightPlans(missionId, (flightPlans) =>
                    {
                        if (!_missionsStore.Missions.TryGetValue(missionId, out Mission mission))
                        {
                            Debug.LogError("Could not get Mission from store");
                            return;
                        }
                        mission.flightPlans = flightPlans;
                        _missionsStore.UpdateMission(missionId, mission);
                    }));
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            });
        }
    }

    public IEnumerator FetchMissions(Action<List<Mission>> callback)
    {
        List<Mission> missions = new();
        UnityWebRequest request = null;
        try
        {
            request = UnityWebRequest.Get(_sessionManager.GetApiUrl() + "/missions");
            request.SetRequestHeader("Authorization", $"Bearer {_usersStore.AuthenticatedUser.token}");
            request.SetRequestHeader("Accept", "application/json");
            request.certificateHandler = new BypassCertificate();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            request = null;
        }
        if (request != null)
        {
            using (request)
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    Debug.Log(request.result);
                    missions = JsonUtility.FromJson<MissionsResponse>(request.downloadHandler.text).missions;
                    foreach (var mission in missions)
                    {
                        StartCoroutine(FetchMissionFlightPlans(mission.id, (flightPlans) =>
                        {
                            mission.flightPlans = flightPlans;
                        }));
                    }
                }
                request.Dispose();
            }
        }
        callback(missions);
    }

    public IEnumerator FetchMissionFlightPlans(int missionId, Action<Dictionary<int, FlightPlans>?> callback)
    {
        Dictionary<int, FlightPlans> fPlans = new();
        UnityWebRequest request;
        try
        {
            Debug.Log($"FETCH FLIGHT PLANS: {_sessionManager.GetApiUrl()}/missions/{missionId}/flightplans");
            request = UnityWebRequest.Get(_sessionManager.GetApiUrl() + $"/missions/{missionId}/flightplans");
            request.SetRequestHeader("Authorization", $"Bearer {_usersStore.AuthenticatedUser.token}");
            request.SetRequestHeader("Accept", "application/json");
            request.certificateHandler = new BypassCertificate();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            request = null;
        }
        if (request != null)
        {
            using (request)
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    List<FlightPlans> flightPlans = new();
                    try
                    {
                        flightPlans = JsonConvert.DeserializeObject<FlightPlansResponse>(request.downloadHandler.text).flight_plans;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                        callback(fPlans);
                    }
                    foreach (var plan in flightPlans)
                    {
                        // ! DO SOMETHING WITH THIS IN THE FUTURE MAYBE
                        if (plan.flight_plan != null || plan.flight_plan == "")
                        {
                            plan.flightPlan = JsonConvert.DeserializeObject<FlightPlan>(plan.flight_plan);
                        }
                        if (plan.bounding_box != null || plan.bounding_box == "")
                        {
                            plan.boundingBox = JsonConvert.DeserializeObject<BoundingBox>(plan.bounding_box);
                        }
                        if (plan.wms_layers != null || plan.wms_layers == "")
                        {
                            //string j = @"[{""layer"": ""layer"",""link"":""something"", ""link_3d"":""https://minio.mobitrust.org:9000/mobitrust/pyodm_20240527172020_3d.glb?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=FJ6LP798571L5WSYN9EI%2F20240606%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20240606T112542Z&X-Amz-Expires=604800&X-Amz-Security-Token=eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJhY2Nlc3NLZXkiOiJGSjZMUDc5ODU3MUw1V1NZTjlFSSIsImV4cCI6MTcxNzcxNjE4NCwicGFyZW50IjoibW9iaXRydXN0In0.jO4EZ0hET2hPuv72BOtD8tcmbkbVcQajVhOZhVdUs2S9otouMmowluXjnOgNckvtTypKiEzRae5tOCpiM6QMzA&X-Amz-SignedHeaders=host&versionId=null&X-Amz-Signature=27f2f31b44ea291b54b21aa0703a5954f1e0853874299308aa30ccfb08b5e49a""}]";
                            //string j = @"[{""layer"": ""layer"",""link"":""something"", ""link_3d"":""https://modelviewer.dev/shared-assets/models/NeilArmstrong.glb""}]";
                            //plan.wmsLayers = JsonConvert.DeserializeObject<List<WMS_Layer?>>(j);
                            //plan.wmsLayers = JsonConvert.DeserializeObject<List<WMS_Layer?>>(plan.wms_layers);
                            
                        }
                        fPlans.Add(plan.id, plan);
                    }
                    callback(fPlans);
                }
            }
        }
    }

    public IEnumerator FetchMission(int id)
    {
        UnityWebRequest request = null;
        try
        {
            request = UnityWebRequest.Get($"{_sessionManager.GetApiUrl()}/mission/{id}");
            request.SetRequestHeader("Authorization", $"Bearer {_usersStore.AuthenticatedUser.token}");
            request.SetRequestHeader("Accept", "application/json");
            request.certificateHandler = new BypassCertificate();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            request = null;
        }
        if (request != null)
        {
            using (request)
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    Debug.Log(request.result);
                    _missionsStore.ShowMission(JsonUtility.FromJson<MissionResponse>(request.downloadHandler.text).mission);
                }
                request.Dispose();
            }
        }
    }
}
