using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class SensorIcon
{
    public string name;
    public Sprite image;
}

public class UserDeviceSensor : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _value;
    [SerializeField] private UsersScriptableObject _usersStore;

    [SerializeField] private List<SensorIcon> _sensorIcons;

    public int UserId { get; set; } = -1;
    public string Driver { get; set; } = null;
    public string Sensor { get; set; } = null;

    private string Value { get; set; } = "N/A";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _value.text = Value;
    }

    private void OnEnable()
    {
        _usersStore.usersUpdateEvent.AddListener(SetValue);
    }

    private void OnDisable()
    {
        _usersStore.usersUpdateEvent.RemoveListener(SetValue);
    }

    private void SetValue(User user)
    {
        if (UserId < 0 || Driver == null || Sensor == null) { return; }
        if (UserId != user.id) { return; }
        if (user.sensors == null) { return; }
        if (!user.sensors.ContainsKey(Driver)) { return; }
        if (!user.sensors[Driver].ContainsKey(Sensor)) { return; }
        Value = user.sensors[Driver][Sensor].value;
        if (Value != null && Value != "") 
        { 
            Value = float.Parse(Value).ToString("F0");
        } 
        else
        {
            Value = "N/A";
        }
    }

    internal void SetSensor(int userId, string driver, string sensor)
    {
        try
        {
            UserId = userId;
            Driver = driver;
            Sensor = sensor;
            SetIcon(sensor);
        }
        catch (Exception e)
        {
            Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error setting sensor, e - {e.Message}");
        }
    }

    public void SetIcon(string type)
    {
        try
        {
            _sensorIcons.ForEach(icon =>
            {
                if (icon.name == type)
                {
                    _icon.sprite = icon.image;
                }
            });
        } catch (Exception e)
        {
            Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error setting icon, e - {e.Message}");
        }
    }
}
