[System.Serializable]
public class MQTTPayload
{
    public string token;
    public double timestamp;
    public string device_id;
    public string type;
    public string driver_device_id;
    public int mission_id;
    public string link_3d;
    public PayloadInfo info;
}

[System.Serializable]
public class MQTTSendDefaultPayload
{
    public string type;
    public string token;
    public string timestamp;
    public string device_id;
    public string driver_device_id;
}

[System.Serializable]
public class MQTTDronePayload: MQTTSendDefaultPayload
{
    public string action;
}

[System.Serializable]
public class MQTTStartStream: MQTTSendDefaultPayload
{
    public string quality;
}

[System.Serializable]
public class MQTTPayloadGeneric : MQTTPayload
{
    public new PayloadInfo info;
}

[System.Serializable]
public class PayloadInfo
{
    public string value_type;
    public string value;
    public string label;
    public bool warning;
}

[System.Serializable]
public class MQTTPayloadGeo : MQTTPayload
{
    public new MQTTGeoInfo info;
}

[System.Serializable]
public class MQTTGeoInfo
{
    public string value_type;
    public Geo value;
}