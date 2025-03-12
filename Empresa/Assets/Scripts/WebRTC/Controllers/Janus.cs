using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Janus : MonoBehaviour
{
    [SerializeField]
    private ulong streamId = 0;
    private ulong activeStream;
    public ulong StreamId { 
        get 
        {
            return streamId;
        }
        set
        {
            streamId = value;
        } 
    }
    private bool isJanusInitialized = false;
    public static string JanusUrl = "https://webrtc.mobitrust.org/janus";
    public static long SessionId = -1;
    private JanusStream Stream = null;
    public delegate IEnumerator HandleAsyncMessage(string json);

    private Coroutine streamWatchCoroutine;
    private Coroutine streamAttachCoroutine;

    public static Dictionary<string, HandleAsyncMessage> AsyncMessageHolder = new Dictionary<string, HandleAsyncMessage>();

    [SerializeField] public Image image;
    public MeshRenderer meshRenderer;
   
    public static string GetNewRandomTransaction()
    {
        var chars = "abcdefghijklmnopqrstuvwxyz1234567890";
        var result = new string(Enumerable.Repeat(chars, 12).Select(s => s[Random.Range(0, s.Length)]).ToArray());
        return result;
    }
    public static bool HasJanusCommand(string json, string janus)
    {
        return JsonUtility.FromJson<JanusResponse>(json).janus == janus; 
    }

    public static string GetJanusCommand(string json)
    {
        return JsonUtility.FromJson<JanusResponse>(json).janus;
    }

    public static Jsep ExtractJsep(string json)
    {
        var jsepResponse = JsonUtility.FromJson<JsepResponse>(json);
        return jsepResponse.jsep;
    }
    
    public static IEnumerator SendMessage(string json, string url, Action<string, string> success, Action<string> error)
    {
        var requestJsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        var request = new UnityWebRequest(url , "POST");
        request.certificateHandler = new BypassCertificate();
        request.uploadHandler = new UploadHandlerRaw(requestJsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        using (request)
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                error(request.error);
            }
            else
            {
                var responseJson = request.downloadHandler.text;
                var command = GetJanusCommand(responseJson);

                success(command, responseJson);
            }
            request.Dispose();
        }
    }

    private IEnumerator AttachPlugin(string plugin, Action<JanusPlugin> success, Action<string> error)
    {
        var requestModel = new AttachPluginRequest(plugin);
        var requestJson = JsonUtility.ToJson(requestModel);
        Debug.Log($"ATTACH PLUGIN: {JanusUrl}/{SessionId}");
        yield return SendMessage(
            requestJson, $"{JanusUrl}/{SessionId}",
            (command, responseJson) =>
            {
                switch (command)
                {
                    case "success":
                        var id = JsonUtility.FromJson<AttachPluginResponse>(responseJson).data.id;
                        success(JanusPlugin.From(plugin, id, this));
                        break;
                    case "error":
                        error(responseJson);
                        break;
                }
            },
            error
        );
        
    }
    public IEnumerator EventHandler()
    {
        while (true)
        {
            Debug.Log($"Event Handler: {JanusUrl}/{SessionId}");
            using (UnityWebRequest request = UnityWebRequest.Get($"{JanusUrl}/{SessionId}"))
            {
                request.certificateHandler = new BypassCertificate();
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(request.error);
                }
                else
                {
                    var responseJson = request.downloadHandler.text;
                    Debug.Log(responseJson);
                    if (HasJanusCommand(responseJson, "event"))
                    {
                        var janusResponse = JsonUtility.FromJson<JanusResponse>(responseJson);

                        if (janusResponse.transaction != null && AsyncMessageHolder.TryGetValue(janusResponse.transaction, out HandleAsyncMessage callback))
                        {
                            yield return callback(responseJson);
                        }
                    }
                    else if(HasJanusCommand(responseJson, "keepalive"))
                    {
                        Debug.Log("keep alive...");
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private IEnumerator InitializeJanus()
    {
        var createSession = new CreateSessionRequest();
        var json = JsonUtility.ToJson(createSession);
        var jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        var request = new UnityWebRequest(JanusUrl, "POST");
        request.certificateHandler = new BypassCertificate();
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        using (request)
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
                Debug.Log("Error creating session!");
            }
            else
            {
                var responseJson = request.downloadHandler.text;
                if (HasJanusCommand(responseJson, "success"))
                {
                    SessionId = JsonUtility.FromJson<CreateSessionResponse>(request.downloadHandler.text).data.id;
                    isJanusInitialized = true;
                    StartCoroutine(EventHandler());

                    Debug.Log($"Created Janus session with id '{SessionId}'");
                }
                else if(HasJanusCommand(responseJson, "error"))
                {
                    Debug.Log("Error creating session!");
                }
            }
        }
    }

    private void resetStream()
    {
        Stream = null;
        if(streamWatchCoroutine != null)
        {
            StopCoroutine(streamWatchCoroutine);
            streamWatchCoroutine = null;
            Debug.Log("StreamCoroutine stopped");
        }
        if (streamAttachCoroutine != null)
        {
            StopCoroutine(streamAttachCoroutine);
            streamAttachCoroutine = null;
            Debug.Log("StreamAttached stopped");
        }
    }
    
    void Start()
    {
       StartCoroutine(InitializeJanus());
    }

    // Update is called once per frame
    void Update()
    {
        if (activeStream != streamId) 
        {
            //Debug.Log("DEBUG -> ActiveStream != streamId");
            resetStream();
        }
        if (StreamId > 0 && isJanusInitialized && Stream == null && activeStream != streamId)
        {
            activeStream = streamId;
            StartCoroutine(AttachPlugin(
                "janus.plugin.streaming",

                plugin =>
                {
                    var streamingPlugin = (StreamingPlugin)plugin;
                    Stream = new JanusStream(streamingPlugin);
                    Debug.Log($"Attached streaming plugin. Watching -> {streamId}");
                    streamWatchCoroutine = StartCoroutine(streamingPlugin.Watch(streamId));
                    //StartCoroutine(streamingPlugin.Watch(999));
                },
                error =>
                {
                    Debug.LogError($"Error attaching streaming plugin: {error}");
                }
            ));
        }
    }
}
