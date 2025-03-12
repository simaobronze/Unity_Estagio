using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}

public class UsersController : MonoBehaviour
{
    private SessionManager _sessionManager;

    [SerializeField] private UsersScriptableObject _usersStore;

    [SerializeField] private string email;
    [SerializeField] private string password;


    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator Login(Action<bool> callback)
    {
        if (!TryGetComponent(out _sessionManager))
        {
            Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error getting SessionManager component");
            Application.Quit();
        }
        WWWForm form = new();
        UnityWebRequest request;
        try
        {
            form.AddField("email", email);
            form.AddField("password", password);
            Debug.Log($"API URL: {_sessionManager.GetApiUrl()}");
            request = UnityWebRequest.Post(_sessionManager.GetApiUrl() + "/login", form);
            request.certificateHandler = new BypassCertificate();
            request.SetRequestHeader("Accept", "application/json");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error on Login: {e}");
            request = null;
            callback(false);
        }
        if (request != null)
        {
            using (request)
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Login request error: {request.error}");
                    callback(false);
                }
                else
                {
                    Debug.Log($"STATUS OF LOGIN: {request.result}");
                    _usersStore.AuthenticatedUser = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text).user;
                    callback(true);
                }
                request.Dispose();
            }
        }
        
    }

    //public IEnumerator FetchUser(int id)
    //{
    //    User user = null;
    //    UnityWebRequest request = UnityWebRequest.Get($"{_sessionManager.GetApiUrl()}/users/{id}");
    //    request.SetRequestHeader("Authorization", $"Bearer {_usersStore._AuthenticatedUser.token}");
    //    request.SetRequestHeader("Accept", "application/json");
    //    using (request)
    //    {
    //        yield return request.SendWebRequest();
    //        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
    //        {
    //            Debug.Log(request.error);
    //        }
    //        else
    //        {
    //            Debug.Log(request.result);
    //            user = JsonUtility.FromJson<UserResponse>(request.downloadHandler.text).user;
               
    //            //user.sensors = new Dictionary<string, UserSensor>();
    //            //if (user.devices == null || user.devices.Count == 0)
    //            //{
    //            //    return;
    //            //}
    //            //user.devices.ForEach(device =>
    //            //{
    //            //    device.drivers.ForEach(driver =>
    //            //    {
    //            //        foreach (var d in _usersStore.Drivers)
    //            //        {
    //            //            if (driver.device_driver_id.)
    //            //            {

    //            //            }
    //            //        }
    //            //        driver.data_types.ForEach(data_type =>
    //            //        {
    //            //            //user.sensors[data_type.type] = new UserSensor();
    //            //        });
    //            //    });
    //            //});
    //        }
    //        request.Dispose();
    //    }
    //}

    public IEnumerator FetchUsers(Action<List<User>> callback)
    {
        List<User> users = new();
        UnityWebRequest request = null;
        try
        {
            request = UnityWebRequest.Get(_sessionManager.GetApiUrl() + "/users?dashboard=true");
            request.SetRequestHeader("Authorization", $"Bearer {_usersStore.AuthenticatedUser.token}");
            request.SetRequestHeader("Accept", "application/json");
            request.certificateHandler = new BypassCertificate();
        }
        catch (Exception e)
        {
            request = null;
            Debug.LogException(e);
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
                    users.ForEach(user =>
                    {
                        user.sensors = new Dictionary<string, Dictionary<string, UserSensor>>();
                        if (user.devices == null || user.devices.Count == 0)
                        {
                            return;
                        }
                        user.devices.ForEach(device =>
                        {
                            device.drivers.ForEach(driver =>
                            {
                                foreach (var d in _usersStore.Drivers)
                                {
                                    if (!driver.driver_device_id.Contains(d))
                                    {
                                        continue;
                                    }
                                    if (!user.sensors.ContainsKey(d))
                                    {
                                        user.sensors.Add(d, new Dictionary<string, UserSensor>());
                                    }
                                }
                            });
                        });
                    });
                }
                request.Dispose();
            }
        }
        callback(users);
    }

    public static IEnumerator FetchUserPFP(string url, Action<Sprite> callback)
    {
        Sprite sprite = null;
        if (url != null && url != "")
        {
            UnityWebRequest request;
            try
            {
                request = UnityWebRequestTexture.GetTexture(url);
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
                    try
                    {
                        if (request.result != UnityWebRequest.Result.Success)
                        {
                            Debug.LogError(request.error);
                        }
                        else
                        {
                            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                            sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
        }
        callback(sprite);
    }
}
