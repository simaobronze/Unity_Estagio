using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Protocol;


public class MQTTReceiver : MonoBehaviour
{
    public string topic;
    private IMqttClient mqttClient;

    async void Start()
    {
        // Corrigido: Instanciar o cliente MQTT corretamente
        MQTTClient mqttClientInstance = FindAnyObjectByType<MQTTClient>();
        if (mqttClientInstance == null)
        {
            Debug.LogError("MQTTClient não encontrado na cena.");
            return;
        }

        mqttClient = mqttClientInstance.GetClient();

        if (mqttClient == null)
        {
            Debug.LogError("Cliente MQTT é null.");
            return;
        }

        if (mqttClient != null && mqttClient.IsConnected)
        {
            // Corrigido: Substituir ApplicationMessageReceivedAsync por ApplicationMessageReceived
            mqttClient.ApplicationMessageReceived += (sender, e) =>
            {
                string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Debug.Log($"Mensagem recebida:\n{message}");
            };

            await mqttClient.SubscribeAsync(topic, MqttQualityOfServiceLevel.AtMostOnce);

            Debug.Log($"Subscrito ao tópico: {topic}");
        }
        else
        {
            Debug.LogError("Cliente MQTT não está conectado.");
        }
    }
}
