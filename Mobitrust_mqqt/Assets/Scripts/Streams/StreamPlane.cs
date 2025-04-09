using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StreamPlane : MonoBehaviour
{
    [SerializeField] private GameObject streamPlane;
    [SerializeField] private Janus janus;

    [SerializeField] private GameObject streamInfo;
    [SerializeField] private Image _pfp;
    [SerializeField] private TextMeshProUGUI userName;

    [SerializeField] StreamsScriptableObject _streamStore;

    private User user;

    public User User
    {
        get
        {
            return user;
        }
        set
        {
            user = value;
            if (_pfp != null)
            {

            }
            if (userName != null)
            {
                userName.text = user.name;
            }
            string url = User?.data_configs?.pfp?.url;
            if (url != null && url != "")
            {
                StartCoroutine(UsersController.FetchUserPFP(url, (sprite) =>
                {
                    _pfp.sprite = sprite;
                }));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartStream()
    {
        if (_streamStore == null)
        {
            Debug.LogError("_streamStore is not assigned");
            return;
        }

        if (_streamStore.ActiveStreamId != 0)
        {
            if (janus == null)
            {
                Debug.LogError("janus is not assigned");
            }
            else
            {
                janus.StreamId = _streamStore.ActiveStreamId;
            }
        }

        if (_streamStore.ActiveUser != null)
        {
            if (userName == null)
            {
                Debug.LogError("userName is not assigned");
            }
            else
            {
                userName.text = _streamStore.ActiveUser.name;
            }

            if (streamInfo == null)
            {
                Debug.LogError("streamInfo is not assigned");
            }
            else
            {
                streamInfo.SetActive(true);
            }
        }
    }


    public void ShowInfo()
    {
        if (user == null) { return; }
        streamInfo.SetActive(true);
    }

    public void HideInfo()
    {
        if (user == null) { return; }
        streamInfo.SetActive(false);
    }

    public void Detach()
    {
        Debug.Log("---------- DETACH STREAM ----------");
        if (streamPlane == null || janus == null)
        {
            Debug.LogError($"Error getting required elements, streamPlane: {streamPlane}, janus: {janus}");
            return;
        }

        ulong streamId = janus.StreamId;
        GameObject streamGO = Instantiate(streamPlane);
        streamGO.transform.SetPositionAndRotation(new Vector3(0f, 1.5f, 8f), Quaternion.Euler(-90f, 0f, 0f));
        streamGO.transform.localScale = new Vector3(10f, 10f, 10f);
        if (!streamGO.TryGetComponent(out DetachableStream detachableStream))
        {
            Debug.LogError($"Error getting component Janus");
            Destroy(streamGO);
            return;
        }
        detachableStream.SetStream(User, streamId);
    }
}
