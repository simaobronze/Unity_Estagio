using System.Collections.Generic;
using UnityEngine;
using MQTTnet.Client;
using MQTTnet;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Networking;
using System;
using System.Collections;
using Newtonsoft.Json;
using uPLibrary.Networking.M2Mqtt;

public class MQTTClient : MonoBehaviour
{
    private IMqttClient _client;
    [SerializeField] private string mqttURL;
    [SerializeField] private string clientID;
    [SerializeField] private string username;
    [SerializeField] private string password;
    [SerializeField] private UsersScriptableObject _usersStore;
    [SerializeField] private ImagesScriptableObject _imagesStore;
    [SerializeField] private MissionsScriptableObject _missionsStore;
    [SerializeField] private SessionManager _sessionManager;



    public void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        //Gets a mqtt client from the factory
        _client = new MqttFactory().CreateMqttClient();
        StartCoroutine(GetCertificate((cert) =>
        {
            if (cert != null)
            {
                ConnectMQTTAsync(cert);
            }
        }));
        //Subscribes the methods to the events from the mqtt client
        _client.Connected += OnConnected;
        _client.ApplicationMessageReceived += OnMessageReceived;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDestroy()
    {
        Debug.Log("---------- DISCONNECT MQTT -----------");
        DisconnectMQTTAsync();
    }

    public IEnumerator GetCertificate(Action<X509Certificate2> callback)
    {
        var caRequest = UnityWebRequest.Get($"{Application.streamingAssetsPath}/Certificates/star.mobitrust.org.crt");
        yield return caRequest.SendWebRequest();
        if (!caRequest.isDone && caRequest.result != UnityWebRequest.Result.ConnectionError && caRequest.result != UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Error getting request {caRequest.result}");
            callback(null);
        }
        else
        {
            callback(new X509Certificate2(caRequest.downloadHandler.data));
        }
    }

    async void ConnectMQTTAsync(X509Certificate2 certCA)
    {
        try
        {
            Debug.Log("------------ Connecting to MQTT ------------");
            IMqttClientOptions options = new MqttClientOptionsBuilder()
            .WithTcpServer(mqttURL)
            .WithClientId($"{clientID}-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}")
            .WithCredentials(username, password)
            .WithTls(new MqttClientOptionsBuilderTlsParameters()
            {
                UseTls = true,
                AllowUntrustedCertificates = true,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                Certificates = new List<byte[]>
                {
                    certCA.Export(X509ContentType.Cert),
                },
                CertificateValidationCallback = delegate { return true; }
            })
            .WithProtocolVersion(MQTTnet.Serializer.MqttProtocolVersion.V311)
            .WithCleanSession()
            .Build();

            await _client.ConnectAsync(options);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    async void DisconnectMQTTAsync()
    {
        await _client.DisconnectAsync();
        Debug.Log("---------- MQTT DISCONNECTED -----------");
    }

    //Method called when the client gets connected to the broker, used to subscribe to the various topics
    private async void OnConnected(object sender, MqttClientConnectedEventArgs e)
    {
        Debug.Log("-------------- MQTT CONNECTED --------------");
        //await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("notifications/+").Build());
        //await client.SubscribeAsync(new TopicFilterBuilder().WithTopic("orchestrator/registration").Build());

        await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("drone_images").Build());
        await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("devices/+/data").Build());
        await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("devices/+/control_all_device").Build());
        await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("devices/+/control_device_ccc").Build());
        await _client.SubscribeAsync(new TopicFilterBuilder().WithTopic("notifications/control_all_ccc").Build());




        //await client.SubscribeAsync(new TopicFilterBuilder().WithTopic("monitor").Build());
        //await client.SubscribeAsync(new TopicFilterBuilder().WithTopic("orchestrator/response").Build());
    }

    //Callback called when message is received via MQTT to process payload
    private void OnMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
    {
        MQTTPayloadGeneric data = null;
        try
        {
            data = JsonUtility.FromJson<MQTTPayloadGeneric>(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            return;
        }
        try
        {
            if (data != null && data.info != null)
            {
                if (data.type == "3d_object")
                {
                    _missionsStore.UpdateFlightPlans(data.mission_id);
                    return;
                }
                //Debug.Log($"USER: {data.device_id}, VALUE_TYPE: {data.info.value_type}");

                if (data.type == "device_connection" || data.type == "start_stream_response")
                {
                    Debug.Log("USERS debug -> new device connected updating users");
                    _sessionManager.UpdateUsers();
                    _sessionManager.UpdateStreams();
                }

                User user = null;
                UserSensor userSensor = null;
                foreach (var u in _usersStore.Users.Values)
                {
                    if (u.devices.Exists(d => d.uuid == data.device_id))
                    {
                        user = u;
                    }
                }
                if (user == null)
                {
                    return;
                }
                user.sensors ??= new Dictionary<string, Dictionary<string, UserSensor>>();

                if (data.type != "data" || data.driver_device_id == null)
                {

                }
                else
                {
                    foreach (var driver in _usersStore.Drivers)
                    {
                        if (!data.driver_device_id.Contains(driver))
                        {
                            continue;
                        }
                        if (driver == "Geo")
                        {
                            MQTTPayloadGeo dataGeo = JsonUtility.FromJson<MQTTPayloadGeo>(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));

                            if (dataGeo.info.value.lat == 0 && dataGeo.info.value.lon == 0)
                            {
                                return;
                            }
                            double[] location = { dataGeo.info.value.lat, dataGeo.info.value.lon, dataGeo.info.value.alt };
                            userSensor = new UserSensor(dataGeo.device_id, dataGeo.type, null, location, false);

                        }
                        else
                        {
                            userSensor = new UserSensor(data.device_id, data.info.value_type, data.info.value, null, data.info.warning);
                        }
                        user = UpdateUserSensors(user, driver, userSensor);
                        break;
                    }
                    if (!data.driver_device_id.Contains("Geo"))
                    {
                        _usersStore.UpdateUserGeoLocation(user);
                    }
                    _usersStore.UpdateUserSensor(user);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"MQTT ERROR - {data.driver_device_id} - {data.type} - {data.info.value_type} - {ex}");
        }
    }

    private User UpdateUserSensors(User user, string driver, UserSensor userSensor)
    {
        if (!user.sensors.ContainsKey(driver))
        {
            user.sensors.Add(driver, new Dictionary<string, UserSensor>());
        }

        if (!user.sensors[driver].ContainsKey(userSensor.name))
        {
            user.sensors[driver].TryAdd(userSensor.name.ToLower(), userSensor);
        }
        else
        {
            user.sensors[driver][userSensor.name.ToLower()] = userSensor;
        }
        return user;
    }

    public async void PublishDroneCommand(string command, string deviceId, string driverDeviceId)
    {
        MQTTDronePayload dp = new MQTTDronePayload
        {
            type = "ptz_action",
            token = clientID + "_" + DateTime.Now.Ticks,
            timestamp = DateTime.Now.Ticks.ToString(),
            device_id = deviceId,
            driver_device_id = driverDeviceId,
            action = command,
        };

        string dronePayloadJSON = JsonConvert.SerializeObject(dp);
        byte[] dronePayloadBytes = Encoding.UTF8.GetBytes(dronePayloadJSON);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic("drone/" + deviceId + "/commands")
            .WithPayload(dronePayloadBytes)
            .WithExactlyOnceQoS()
            .WithRetainFlag()
            .Build();

        await _client.PublishAsync(message);
        //Debug.Log($"MQTT -> Sent message to {deviceId} command -> {dp.action}");
    }

    public async void PublishStartStreamMessage(string deviceId, string driverDeviceId)
    {
        MQTTStartStream payload = new MQTTStartStream
        {
            type = "start_stream",
            token = clientID + "_" + DateTime.Now.Ticks.ToString(),
            timestamp = DateTime.Now.Ticks.ToString(),
            device_id = deviceId,
            driver_device_id = driverDeviceId,
            quality = "HD",
        };

        string startStreamPayload = JsonConvert.SerializeObject(payload);
        byte[] startStreamPayloadBytes = Encoding.UTF8.GetBytes(startStreamPayload);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic("devices/" + deviceId + "/control_device_ccc")
            .WithPayload(startStreamPayloadBytes)
            .WithExactlyOnceQoS()
            .WithRetainFlag()
            .Build();

        await _client.PublishAsync(message);
    }
}
