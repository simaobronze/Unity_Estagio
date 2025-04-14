using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreamSelection : MonoBehaviour
{
    [SerializeField]
    private StreamsScriptableObject _streamsStore;
    [SerializeField]
    private GameObject _list;  // Painel da lista de streams
    [SerializeField]
    private GameObject _stream; // Aqui suponho que seja o painel da stream, se aplicável
    [SerializeField]
    private GameObject _buttonPrefab;
    private MQTTClient _mqttClient;

    // Streams vindos do back-end
    private Dictionary<ulong, User> _listedStreams = new();

    private List<GameObject> _droneStreams = new List<GameObject>();

    void Start()
    {
        _mqttClient = FindAnyObjectByType<MQTTClient>();
        if (_mqttClient == null)
        {
            Debug.LogError("MQTTClient not found");
        }
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

    private void UpdateList(Dictionary<int, User> users)
    {
        // Para cada usuário recebido
        foreach (KeyValuePair<int, User> kvp in users)
        {
            User user = kvp.Value;
            // Verifica se o usuário tem pelo menos uma stream
            if (user.streams == null || user.streams.Count <= 0)
                continue;

            // Usa a primeira stream como chave
            ulong streamId = user.streams[0].stream_id;
            if (_listedStreams.ContainsKey(streamId))
                continue;

            // Adiciona o usuário e instancia o botão
            _listedStreams.Add(streamId, user);
            _droneStreams.Add(InstantiateButton(user));
        }
        // Atualiza o layout para garantir que os botões apareçam corretamente
        LayoutRebuilder.ForceRebuildLayoutImmediate(_list.transform as RectTransform);
    }

    // Método para instanciar botões para os usuários recebidos
    private GameObject InstantiateButton(User user)
    {
        GameObject button = Instantiate(_buttonPrefab, _list.transform);
        Button buttonComponent = button.GetComponent<Button>();
        TMPro.TextMeshProUGUI tmp = buttonComponent.GetComponentInChildren<TMPro.TextMeshProUGUI>();

        tmp.text = user.name;
        buttonComponent.onClick.AddListener(() => { OnClick(user.streams[0].stream_id); });

        return button;
    }

    // Método que trata o clique do botão
    public void OnClick(ulong streamId)
    {
        // Localiza o GameObject onde a stream será exibida
        GameObject streamPlane = GameObject.FindGameObjectWithTag("StreamPlaneDrone");
        if (streamPlane == null)
        {
            Debug.LogError("StreamPlaneDrone not found");
            return;
        }

        // Recupera o usuário correspondente ao streamId
        if (!_listedStreams.TryGetValue(streamId, out User user))
        {
            Debug.LogError("User not found for streamId " + streamId);
            return;
        }

        if (user.streams.Count > 0)
        {
            // Publica a mensagem para iniciar a stream com os parâmetros do usuário selecionado
            _mqttClient.PublishStartStreamMessage(user.streams[0].device_id, user.streams[0].driver_device_id);
            _streamsStore.SetActiveStream(user, streamId);

            // Opcional: desativa a lista de botões para exibir apenas a stream selecionada
            if (_list != null)
            {
                _list.SetActive(false);
            }

            // Inicia a stream no componente responsável (ex. um painel de exibição)
            StreamPlane plane = streamPlane.GetComponent<StreamPlane>();
            if (plane != null)
            {
                plane.StartStream();
            }
            else
            {
                Debug.LogError("StreamPlane component not found on " + streamPlane.name);
            }
        }
    }
}
