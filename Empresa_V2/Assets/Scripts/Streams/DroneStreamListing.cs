using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneStreamListing : MonoBehaviour
{

    [SerializeField]
    private StreamsScriptableObject _streamsStore;
    [SerializeField]
    private GameObject _list;
    [SerializeField]
    private GameObject _stream;
    [SerializeField]
    private GameObject _buttonPrefab;
    private MQTTClient _mqttClient;

    private Dictionary<ulong, User> _listedStreams = new();


    private List<GameObject> _droneStreams = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _mqttClient = FindAnyObjectByType<MQTTClient>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _streamsStore.streamsFetchedEvent.AddListener(UpdateList);
    }

    private void OnDisable()
    {
        _streamsStore.streamsFetchedEvent.RemoveListener(UpdateList);
    }

    private void UpdateList(Dictionary<int, User> users)
    {
        foreach(KeyValuePair<int, User> user in users)
        {
            if (_listedStreams.ContainsKey(user.Value.streams[0].stream_id)){
                continue;
            }
            if (user.Value.streams == null || user.Value.streams.Count <= 0) 
            {
                continue;
            }
            if (user.Value.streams[0].device_id.ToLower().Contains("drone"))
            {
                _listedStreams.Add(user.Value.streams[0].stream_id, user.Value);
                _droneStreams.Add(InstantiateButton(user.Value));
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_list.transform as RectTransform);
    }

    private GameObject InstantiateButton(User user)
    {
        GameObject button = Instantiate(_buttonPrefab, _list.transform);
        Button buttonComponent = button.GetComponent<Button>();
        TMPro.TextMeshProUGUI tmp = buttonComponent.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        tmp.text = user.name;
        buttonComponent.onClick.AddListener(() => { OnClick(user.streams[0].stream_id); });
        
        return button;
    }

    public void OnClick(ulong streamId)
    {
        //Change to have different prefabs for drone streams and all streams 
        GameObject streamPlane = GameObject.FindGameObjectWithTag("StreamPlaneDrone");

        if (streamPlane != null)
        {
            Debug.Log("Stream plane found");
        }

        //Debug.Log($"Onclick -> streamId: {streamId}");

        _listedStreams.TryGetValue(streamId, out User user);

        if (user.streams.Count != 0)
        {
            _mqttClient.PublishStartStreamMessage(user.streams[0].device_id,user.streams[0].driver_device_id);
            _streamsStore.SetActiveStream(user, streamId);
            streamPlane.GetComponent<StreamPlane>().StartStream();
        }
    }
}
