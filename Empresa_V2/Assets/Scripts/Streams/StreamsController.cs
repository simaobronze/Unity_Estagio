using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class StreamsController : MonoBehaviour
{
    private SessionManager _sessionManager;

    [SerializeField] private UsersScriptableObject _usersStore;
    [SerializeField] private StreamsScriptableObject _streamsStore;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator FetchStreams(Action<List<User>> callback)
    {
        if (!TryGetComponent(out _sessionManager))
        {
            Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error getting SessionManager component");
            Application.Quit();
        }
        List<User> users = new();
        UnityWebRequest request = null;
        try
        {
            request = UnityWebRequest.Get(_sessionManager.GetApiUrl() + "/users/streams");
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
                    users = JsonUtility.FromJson<UsersResponse>(request.downloadHandler.text).users;
                    //Debug.Log("streams -> " + request.downloadHandler.text);
                }
                request.Dispose();
            }
        }
        callback(users);
    }
}
