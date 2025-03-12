using System.Collections.Generic;

[System.Serializable]
public class Mission
{
    public int id;
    public string name;
    public string description;
    public MissionDataConfigs data_configs;
    public List<MissionTeam> teams;
    public Dictionary<int, FlightPlans> flightPlans;
    //public List<Mission> childs;
}

[System.Serializable]
public class MissionDataConfigs
{
    public string color;
    public string type;
    public double lat;
    public double lon;
    public float zoom;
    public string map_type;
    public string status;
    public MissionGeo geo;
    public List<Beacon> beacons_list;
}

[System.Serializable]
public class MissionGeo
{
    public string type;
    public List<Feature> features;
}

[System.Serializable]
public class Feature
{
    public string type;
    public FeatureGeometry geometry;
    public FeatureProperties properties;
}

[System.Serializable]
public class FeatureProperties
{
    public string type;
    public double radius;
}

[System.Serializable]
public class FeatureGeometry
{
    public string type;
    public List<double> coordinates;
}

[System.Serializable]
public class Beacon
{
    public double x;
    public double y;
    public string color;
    public string uuid;
    public string minor;
    public string major;
}

[System.Serializable]
public class MissionTeam
{
    public int team_id;
    public string name;
    public string description;
}
