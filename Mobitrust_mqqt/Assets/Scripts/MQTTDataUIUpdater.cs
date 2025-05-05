using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Serializable]
public class DataItem
{
    public string value_type;
    public JToken value;
}

[Serializable]
public class InfoPayload
{
    public List<DataItem> flightControl;
    public List<DataItem> battery;
    public List<DataItem> geo;
    public List<DataItem> camera;
    public List<DataItem> gimbal;
}

[Serializable]
public class DroneMessage
{
    public string token;
    public string type;
    public string device_id;
    public string ip;
    public long timestamp;
    public string driver_device_id;
    public InfoPayload info;
}

public class MQTTDataUIUpdater : MonoBehaviour
{
    [Header("Minimap")]
    public RawImage minimapImage;

    [Header("Feed de Vídeo Principal")]
    public RawImage videoFeedImage;

    [Header("Barra Inferior")]
    public TMP_Text altitudeText;            
    public TMP_Text homeDistanceText;  
    public TMP_Text droneBatteryText;        
    public TMP_Text remoteBatteryText;       
    public TMP_Text horizontalSpeedText;     
    public TMP_Text verticalSpeedText;       

    [Header("Barra Lateral Direita Superior")]
    public Button helpButton;           
    public Button detailsButton;        
    public Button droneCenterButton;    
    public Toggle mapSizeToggle;        

    [Header("Barra Lateral Direita Inferior")]
    public TMP_Text esquerdaText;
    public TMP_Text direitaText;
    public TMP_Text cimaText;
    public TMP_Text baixoText;

    private float accumulatedDistance = 0f; // Distância acumulada
    private float currentVerticalSpeed = 0f; // Velocidade vertical atual

    void Update()
    {
        // Calcula distância acumulada: velocidade vertical * deltaTime
        accumulatedDistance += currentVerticalSpeed * Time.deltaTime;
        if (homeDistanceText != null)
            homeDistanceText.text = accumulatedDistance.ToString("F2") + " m";
    }
    public void UpdateUIFromJson(string jsonPayload)
    {
        DroneMessage message;
        try
        {
            message = JsonConvert.DeserializeObject<DroneMessage>(jsonPayload);
        }
        catch (Exception ex)
        {
            Debug.LogError("Falha ao desserializar MQTT payload: " + ex);
            return;
        }

        // Exemplo de atualização de dados da barra inferior, caso existam em info.geo:
        var altItem = message.info.geo?.Find(x => x.value_type.Equals("altitude", StringComparison.OrdinalIgnoreCase));
        if (altItem != null)
            altitudeText.text = altItem.value + " m";

        // Outros campos podem ser atualizados a partir de flightControl e battery
        SetTextValue(message.info.flightControl, "speed_horizontal", horizontalSpeedText, " m/s");
        SetTextValue(message.info.flightControl, "speed_vertical", verticalSpeedText, " m/s");
        SetTextValue(message.info.battery, "battery", droneBatteryText, "%");
        SetTextValue(message.info.battery, "controller_battery", remoteBatteryText, "%");

        float speedX = 0f, speedY = 0f;
        var xItem = message.info.flightControl?.Find(x =>
            x.value_type.Equals("speed_x", StringComparison.OrdinalIgnoreCase));
        if (xItem != null && float.TryParse(xItem.value.ToString(), out var parsedX))
            speedX = parsedX;

        var yItem = message.info.flightControl?.Find(x =>
            x.value_type.Equals("speed_y", StringComparison.OrdinalIgnoreCase));
        if (yItem != null && float.TryParse(yItem.value.ToString(), out var parsedY))
            speedY = parsedY;

        // 2. Atualizar esquerda/direita
        if (speedX > 0f)
        {
            direitaText.text = speedX.ToString("F2") + " m/s";
            esquerdaText.text = "N/A";
        }
        else if (speedX < 0f)
        {
            esquerdaText.text = Mathf.Abs(speedX).ToString("F2") + " m/s";
            direitaText.text = "N/A";
        }
        else
        {
            // sem movimento horizontal
            esquerdaText.text = direitaText.text = "0 m/s";
        }

        // 3. Atualizar cima/baixo
        if (speedY > 0f)
        {
            cimaText.text = speedY.ToString("F2") + " m/s";
            baixoText.text = "N/A";
        }
        else if (speedY < 0f)
        {
            baixoText.text = Mathf.Abs(speedY).ToString("F2") + " m/s";
            cimaText.text = "N/A";
        }
        else
        {
            // sem movimento vertical
            cimaText.text = baixoText.text = "0 m/s";
        }

        // Ações de botões podem ser configuradas no Inspector ou via código:
        mapSizeToggle.onValueChanged.AddListener(isSmall =>
        {
            // Lógica para alternar tamanho do minimapa
            minimapImage.rectTransform.sizeDelta = isSmall ? new Vector2(150, 150) : new Vector2(300, 300);
        });
    }

    private void SetTextValue(List<DataItem> list, string key, TMP_Text target, string suffix = "")
    {
        if (target == null || list == null) return;
        var item = list.Find(x => x.value_type.Equals(key, StringComparison.OrdinalIgnoreCase));
        target.text = item != null ? item.value + suffix : "N/A";
    }
}
