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
    public Mapbox mapboxScript;

    [Header("UI Data")]
    public TMP_Text altitudeText;
    public TMP_Text homeDistanceText;
    public TMP_Text droneBatteryText;
    public TMP_Text remoteBatteryText;
    public TMP_Text horizontalSpeedText;
    public TMP_Text verticalSpeedText;
    public TMP_Text speedXLessText;
    public TMP_Text speedXPlusText;
    public TMP_Text speedYMinusText;
    public TMP_Text speedYPlusText;


    private float currentVerticalSpeed = 0f;
    private bool homeSet = false;
    private float homeLat, homeLon;

    public void UpdateUIFromJson(string jsonPayload)
    {
        Debug.Log("[MQTTDataUIUpdater] JSON recebido: " + jsonPayload);
        DroneMessage message;
        JObject jmsg;
        try
        {
            message = JsonConvert.DeserializeObject<DroneMessage>(jsonPayload);
            jmsg = JObject.Parse(jsonPayload);
        }
        catch (Exception ex)
        {
            Debug.LogError("Falha ao desserializar MQTT payload: " + ex);
            return;
        }

        // ALTITUDE
        var altItem = message.info.flightControl
            ?.Find(x => x.value_type.Equals("altitude", StringComparison.OrdinalIgnoreCase));
        if (altItem != null)
            altitudeText.text = altItem.value + " m";

        // BATTERY
        SetTextValue(message.info.battery, "battery", droneBatteryText, "%");
        SetTextValue(message.info.battery, "controller_battery", remoteBatteryText, "%");

        // SPEED
        SetTextValue(message.info.flightControl, "speed_horizontal", horizontalSpeedText, " m/s");
        SetTextValue(message.info.flightControl, "speed_vertical", verticalSpeedText, " m/s");

        // Directions
        float speedX = ParseValue(message.info.flightControl, "speed_x");
        float speedY = ParseValue(message.info.flightControl, "speed_y");
        UpdateDirectionUI(speedX, speedXPlusText, speedXLessText);
        UpdateDirectionUI(speedY, speedYPlusText, speedYMinusText);

        // GEO: drone_geo + home distance + map
        try
        {
            var geoArray = jmsg["info"]?["geo"] as JArray;
            if (geoArray != null)
            {
                foreach (var elem in geoArray)
                {
                    if (elem["value_type"]?.ToString().Equals("drone_geo", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        float lat = elem["lat"].ToObject<float>();
                        float lon = elem["lon"].ToObject<float>();
                        Debug.Log($"[MQTTDataUIUpdater] drone_geo: lat={lat}, lon={lon}");

                        if (!homeSet)
                        {
                            homeLat = lat;
                            homeLon = lon;
                            homeSet = true;
                        }

                        double distHome = HaversineDistance(homeLat, homeLon, lat, lon);
                        homeDistanceText.text = distHome.ToString("F2") + " m";

                        if (mapboxScript != null)
                            mapboxScript.SetDronePosition(lat, lon);

                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error processing drone_geo: " + ex);
        }
    }

    private void SetTextValue(List<DataItem> list, string key, TMP_Text target, string suffix = "")
    {
        if (target == null || list == null) return;
        var item = list.Find(x => x.value_type.Equals(key, StringComparison.OrdinalIgnoreCase));
        target.text = item != null ? item.value + suffix : "N/A";
    }

    private float ParseValue(List<DataItem> list, string key)
    {
        var item = list?.Find(x => x.value_type.Equals(key, StringComparison.OrdinalIgnoreCase));
        if (item != null && float.TryParse(item.value.ToString(), out var val))
        {
            currentVerticalSpeed = key.Equals("speed_vertical", StringComparison.OrdinalIgnoreCase) ? val : currentVerticalSpeed;
            return val;
        }
        return 0f;
    }

    private void UpdateDirectionUI(float value, TMP_Text positive, TMP_Text negative)
    {
        if (value > 0f)
        {
            positive.text = value.ToString("F2") + " m/s";
            negative.text = "N/A";
        }
        else if (value < 0f)
        {
            negative.text = Mathf.Abs(value).ToString("F2") + " m/s";
            positive.text = "N/A";
        }
        else
        {
            positive.text = negative.text = "0 m/s";
        }
    }

    private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371000;
        double dLat = (lat2 - lat1) * Math.PI / 180.0;
        double dLon = (lon2 - lon1) * Math.PI / 180.0;
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                 + Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0)
                 * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
}