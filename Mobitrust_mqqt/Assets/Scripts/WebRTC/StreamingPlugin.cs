using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.WebRTC;

public class StreamingPlugin : JanusPlugin
{

    private RTCPeerConnection _pc;
    private Janus _janus;
    public StreamingPlugin(long id, Janus janus)
    {
        this.id = id;
        _janus = janus;
        name = "janus.plugin.streaming";

        _pc = new RTCPeerConnection();
        _pc.OnIceCandidate += candidate =>
        {
            var candidateModel =
                new TrickleRequestCandidate(candidate.SdpMid, candidate.SdpMLineIndex, candidate.Candidate);

            _janus.StartCoroutine(Trickle(candidateModel));
        };


        _pc.OnNegotiationNeeded += () =>
        {
            Debug.Log("Negotiation Needed");
        };

        _pc.OnTrack += e =>
        {
            if (e.Track is VideoStreamTrack video)
            {
                video.OnVideoReceived += tex =>
                {
                    Debug.Log("got texture");
                    if (_janus.meshRenderer != null)
                    {
                        _janus.meshRenderer.material.mainTexture = tex;
                    }
                    if (_janus.image != null)
                    {
                        _janus.image.sprite = Sprite.Create((Texture2D)tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                    }
                    _janus.StartCoroutine(WebRTC.Update());
                };
            }
        };
    }

    public static string GetStreamingCommand(string json)
    {
        return JsonUtility.FromJson<StreamingResponse>(json).plugindata.data.streaming;
    }

    private IEnumerator HandleStartResponse(string json)
    {
        var streamingEvent = JsonUtility.FromJson<StreamingEvent>(json);
        var status = streamingEvent.plugindata.data.result.status;

        switch (status)
        {
            case "starting":
                {
                    Debug.Log("Starting...");
                    break;
                }
            default:
                Debug.Log($"other thing: {json}");
                break;
        }

        yield break;
    }
    private IEnumerator Start(Jsep answer)
    {
        var request = new StartRequest(answer);
        var requestJson = JsonUtility.ToJson(request);

        Debug.Log($"sending this to start {requestJson}");

        yield return Janus.SendMessage(requestJson, $"{Janus.JanusUrl}/{Janus.SessionId}/{id}",
            (command, responseJson) =>
            {
                Debug.Log($"start ack?: {responseJson}");
            },
            error =>
            {

            }
        );
    }

    public IEnumerator Trickle(TrickleRequestCandidate candidate)
    {
        var request = new TrickleRequest(new List<TrickleRequestCandidate> { candidate });
        var requestJson = JsonUtility.ToJson(request);

        Debug.Log($"sending trickle {requestJson}");

        yield return Janus.SendMessage(requestJson, $"{Janus.JanusUrl}/{Janus.SessionId}/{id}",
            (command, responseJson) =>
            {
                Debug.Log($"Trickle response: {responseJson}");
            },
            error =>
            {
                Debug.LogError($"Error sending trickle request: {error}");
            }
        );
    }

    private IEnumerator HandleOffer(Jsep offer)
    {
        var offerDesc = new RTCSessionDescription();
        offerDesc.type = RTCSdpType.Offer;
        offerDesc.sdp = offer.sdp;

        yield return _pc.SetRemoteDescription(ref offerDesc);
        var answer = _pc.CreateAnswer();

        yield return answer;
        var answerDesc = answer.Desc;
        yield return _pc.SetLocalDescription(ref answerDesc);

        var answerModel = new Jsep("answer", _pc.LocalDescription.sdp);
        yield return Start(answerModel);
    }

    private IEnumerator HandleWatchResponse(string json)
    {
        Debug.Log("calling callback");
        var streamingEvent = JsonUtility.FromJson<StreamingEvent>(json);
        var status = streamingEvent.plugindata.data.result.status;

        switch (status)
        {
            case "preparing":
                {
                    var jsep = Janus.ExtractJsep(json);
                    yield return HandleOffer(jsep);

                    break;
                }
            default:
                Debug.Log($"other thing: {json}");
                break;
        }
    }
    public IEnumerator Watch(ulong mountpoint)
    {
        var requestModel = new WatchRequest(mountpoint);
        var requestJson = JsonUtility.ToJson(requestModel);
        var requestJsonToSend = new System.Text.UTF8Encoding().GetBytes(requestJson);

        var request = new UnityWebRequest($"{Janus.JanusUrl}/{Janus.SessionId}/{id}", "POST");
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
                Debug.LogError(request.error);
                Debug.Log("Error listing all mountpoints!");
            }
            else
            {
                var responseJson = request.downloadHandler.text;
                if (Janus.HasJanusCommand(responseJson, "ack"))
                {
                    var ackResponse = JsonUtility.FromJson<JanusResponse>(responseJson);
                    Debug.Log("watch ack response");
                    Janus.AsyncMessageHolder.Add(ackResponse.transaction, HandleWatchResponse);
                }
            }
            request.Dispose();
        }
    }
    public IEnumerator ListAllMountPoints(Action<ListAllResponse> success, Action<string> error)
    {
        var requestModel = new ListAllRequest();
        var requestJson = JsonUtility.ToJson(requestModel);
        var requestJsonToSend = new System.Text.UTF8Encoding().GetBytes(requestJson);

        var request = new UnityWebRequest($"{Janus.JanusUrl}/{Janus.SessionId}/{id}", "POST");
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
                Debug.LogError(request.error);
                Debug.Log("Error listing all mountpoints!");
            }
            else
            {
                var responseJson = request.downloadHandler.text;
                if (Janus.HasJanusCommand(responseJson, "success"))
                {
                    Debug.Log(responseJson);
                    var response = JsonUtility.FromJson<ListAllResponse>(responseJson);
                    success(response);
                }
                else if (Janus.HasJanusCommand(responseJson, "error"))
                {
                    error(responseJson);
                }

            }
            request.Dispose();
        }
    }
}
