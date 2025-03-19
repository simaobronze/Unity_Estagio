using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class MissionsResponse
{
    public List<Mission> missions;
}

[System.Serializable]
public class MissionResponse
{
    public Mission mission;
}

[System.Serializable]
public class FlightPlansResponse
{
    [JsonProperty("flight_plans")] public List<FlightPlans> flight_plans;
}

[System.Serializable]
public class FlightPlans
{
    [JsonProperty("id")] public int id;
    [JsonProperty("mission_id")] public int mission_id;
    [JsonProperty("name")] public string name;
    [JsonProperty("timestamp")] public string timestamp;
    [JsonProperty("bounding_box")] public string bounding_box;
    public BoundingBox boundingBox;
    [JsonProperty("flight_plan")] public string flight_plan;
    public FlightPlan flightPlan;
    [JsonProperty("wms_layers")] public string wms_layers;
    public List<WMS_Layer?> wmsLayers;
}

[System.Serializable]
public class BoundingBox
{
    [JsonProperty("type")] public string type;
}

[System.Serializable]
public class FlightPlan
{
    [JsonProperty("mission_type")] public string mission_type;
    [JsonProperty("name")] public string name;
}


[System.Serializable]
public class WMS_Layers
{
    public List<WMS_Layer> wms_layer;
}

[System.Serializable]
public class WMS_Layer
{
    [JsonProperty("layer")]
    public string layer;

    [JsonProperty("link")]
    public string link;

    [JsonProperty("link_3d")]
    public string link_3d;

    [JsonProperty("timestamp")]
    public string timestamp;
}


//[System.Serializable]
//public class FlightPlansResponse
//{
//    public List<FlightPlans> flight_plans;
//}

//[System.Serializable]
//public class FlightPlans
//{
//    public int id;
//    public int mission_id;
//    public string name;
//    public string timestamp;
//    public string bounding_box;
//    public BoundingBox boundingBox;
//    public string flight_plan;
//    public FlightPlan flightPlan;
//    public string wms_layers;
//    public List<WMS_Layer?> wmsLayers;
//}

//[System.Serializable]
//public class BoundingBox
//{
//    public string type;
//    public List<string> properties;
//}

//[System.Serializable]
//public class FlightPlan
//{
//    public string mission_type;
//    public string name;
//}


//[System.Serializable]
//public class WMS_Layers
//{
//    public List<WMS_Layer> wms_layer;
//}

//[System.Serializable]
//public class WMS_Layer
//{
//    [JsonProperty("link")]
//    public string link;

//    [JsonProperty("3d_link")]
//    public string link_3d;
//}