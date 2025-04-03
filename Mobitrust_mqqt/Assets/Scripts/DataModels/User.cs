using System;
using System.Collections.Generic;
using System.Reflection;

[System.Serializable]
public class User : ICloneable
{
    public int id;
    public string name;
    public string email;
    public DataConfigs data_configs;
    public List<Role> roles;
    public List<Mission> missions;
    public List<UserTeam> teams;
    public List<Device> devices;
    public List<Stream> streams;
    public Dictionary<string, Dictionary<string, UserSensor>> sensors;
    public string token;

    public object Clone()
    {
        return MemberwiseClone();
    }
}

[System.Serializable]
public class DataConfigs
{
    public PFP pfp;
    public Portal portal;
    public VRConfigs vr_configs;
}

[System.Serializable]
public class PFP
{
    public string name;
    public string exp_date;
    public string url;
}

[System.Serializable]
public class Portal
{
    public string type;
    public List<Waypoint> wayPoints;
    public Geo geo;
}

[System.Serializable]
public class Waypoint
{
    public double lat;
    public double lng;
}

[System.Serializable]
public class VRConfigs
{
    public string map_type;
}

[System.Serializable]
public class Geo
{
    public string map_type;
    public double lat;
    public double lon;
    public double alt;
    public int zoom;
}

[System.Serializable]
public class Role
{
    public int id;
    public string label;
}

[System.Serializable]
public class UserTeam
{
    public int id;
    public string name;
}

[System.Serializable]
public class Device
{
    public int id;
    public string uuid;
    public string connection;
    public List<Drivers> drivers;
}

[System.Serializable]
public class Drivers
{
    public int id;
    public string driver_id;
    public string status;
    public string driver_device_id;
    public string description;
    public List<DataType> data_types;
    public List<DriverDataConfig> data_configs;
    public Configs configs;
}

[System.Serializable]
public class DataType
{
    public string type;
    public string visual;
    public string x;
    public string y;
    public int min;
    public int max;
}

[System.Serializable]
public class DriverDataConfig
{
    public string type;
    public DriverConfig configs;
}

[System.Serializable]
public class DriverConfig
{
    public bool active;
    public int channel;
    public string min;
    public string max;
    public string label;
}

[System.Serializable]
public class Configs
{
    public int rate_normal;
    public int rate_receive;
    public int rate_warning;
    public int battery_threshold;
    public int sample_rate;
    public int n_samples;
    public int ecg_send;
    public ulong stream_id;
}

[System.Serializable]
public class UserSensor
{
    public string driver;
    public string name;
    public string value;
    public double[] location = new double[2];
    public bool warning;

    public UserSensor()
    {
        this.driver = null;
        this.name = null;
        this.value = null;
        this.location = new double[2];
        this.warning = false;
    }

#nullable enable
    public UserSensor(string? driver, string? name, string? value, double[]? location, bool warning = false)
    {
        this.driver = driver;
        this.name = name;
        this.value = value;
        this.location = location;
        this.warning = warning;
    }
#nullable disable
}

[System.Serializable]
public class Stream
{
    public int id;
    public string driver_id;
    public string status;
    public string driver_device_id;
    public string description;
    public string device_id;
    public ulong stream_id;
}