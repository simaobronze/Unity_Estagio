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

    // Para streams vindos do back-end (já existentes)
    private Dictionary<ulong, User> _listedStreams = new();

    /*
    [System.Serializable]
    public class Drone
    {
        public ulong id;
        public string nome;
    }

    // Array manual de drones
    private Drone[] drones = new Drone[]
    {
        new Drone { nome = "drone1", id = 997 },
        new Drone { nome = "drone2", id = 998 },
        new Drone { nome = "drone3", id = 999 }
    };

    */

    private List<GameObject> _droneStreams = new List<GameObject>();

    void Start()
    {
        _mqttClient = FindAnyObjectByType<MQTTClient>();
        if (_mqttClient == null)
        {
            Debug.LogError("MQTTClient not found");
        }
        /*
        // Chama o método que instancia os botões a partir da array manual
        UpdateDronePanelManual();
        */
    }

    void Update() { }

    private void OnEnable()
    {
        _streamsStore.streamsFetchedEvent.AddListener(UpdateList);
    }

    private void OnDisable()
    {
        _streamsStore.streamsFetchedEvent.RemoveListener(UpdateList);
    }

    // Método existente para atualizar a lista de streams (back-end)
    private void UpdateList(Dictionary<int, User> users)
    {
        foreach (KeyValuePair<int, User> user in users)
        {
            if (_listedStreams.ContainsKey(user.Value.streams[0].stream_id))
                continue;
            if (user.Value.streams == null || user.Value.streams.Count <= 0)
                continue;
            /* if (user.Value.streams[0].device_id.ToLower().Contains("drone"))
             {
                 _listedStreams.Add(user.Value.streams[0].stream_id, user.Value);
                 _droneStreams.Add(InstantiateButton(user.Value));
             }*/
            _listedStreams.Add(user.Value.streams[0].stream_id, user.Value);
            _droneStreams.Add(InstantiateButton(user.Value));
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_list.transform as RectTransform);
    }

    // Método já existente para instanciar botões para usuários vindos do back-end
    private GameObject InstantiateButton(User user)
    {
        GameObject button = Instantiate(_buttonPrefab, _list.transform);
        Button buttonComponent = button.GetComponent<Button>();
        TMPro.TextMeshProUGUI tmp = buttonComponent.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        tmp.text = user.name;
        buttonComponent.onClick.AddListener(() => { OnClick(user.streams[0].stream_id); });

        return button;
    }

    // Método para tratar o clique do botão que vem do back-end
    public void OnClick(ulong streamId)
    {
        GameObject streamPlane = GameObject.FindGameObjectWithTag("StreamPlaneDrone");

        if (streamPlane != null)
        {
            Debug.Log("Stream plane found");
        }

        _listedStreams.TryGetValue(streamId, out User user);

        if (user.streams.Count != 0)
        {
            _mqttClient.PublishStartStreamMessage(user.streams[0].device_id, user.streams[0].driver_device_id);
            _streamsStore.SetActiveStream(user, streamId);
            streamPlane.GetComponent<StreamPlane>().StartStream();
        }
    }

    /*
    // Novo método para instanciar os botões usando a array manual de drones
    private void UpdateDronePanelManual()
    {
        foreach (var drone in drones)
        {
            GameObject button = Instantiate(_buttonPrefab, _list.transform);
            Button buttonComponent = button.GetComponent<Button>();
            TMPro.TextMeshProUGUI tmp = buttonComponent.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            tmp.text = drone.nome;
            // Associa o clique ao método que iniciará a transmissão do drone
            buttonComponent.onClick.AddListener(() => { OnDroneClick(drone.id); });
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_list.transform as RectTransform);
    }
    */

    
    /*

    // Novo método para tratar o clique do botão do drone manual
    public void OnDroneClick(ulong droneId)
    {
        Debug.Log("Clicou no drone com id: " + droneId);

        // Aqui você precisa definir como obter os parâmetros necessários para iniciar a stream,
        // como "device_id" e "driver_device_id". Se esses parâmetros podem ser derivados do droneId
        // ou se já estão definidos em algum outro local, faça essa implementação aqui.
        // Exemplo:
        string deviceId = "drone" + droneId; // ajuste conforme sua lógica
        string driverDeviceId = "driver" + droneId; // ajuste conforme sua lógica



        _mqttClient.PublishStartStreamMessage(deviceId, driverDeviceId);



        // Se necessário, você pode configurar o stream ativo no _streamsStore para o drone
        // _streamsStore.SetActiveStream(...);

        GameObject streamPlane = GameObject.FindGameObjectWithTag("StreamPlaneDrone");
        if (streamPlane != null)
        {
            Debug.Log("O problema é aqui?");
            streamPlane.GetComponent<StreamPlane>().StartStream();
            Debug.Log("Não");
        }
        else
        {
            Debug.LogError("StreamPlaneDrone not found");
        }
    }

    */
}
