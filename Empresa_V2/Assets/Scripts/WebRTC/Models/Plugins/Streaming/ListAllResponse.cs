using System.Collections.Generic;

[System.Serializable]
public class ListAllResponse : JanusResponse
{
    public ListAllPluginData plugindata;
}

[System.Serializable]
public class ListAllPluginData
{
    public string plugin;
    public ListAllData data;
}

[System.Serializable]
public class ListAllData
{
    public string streaming;
    public List<ListItem> list;
}

[System.Serializable]
public class ListItem
{
    public long id;
    public string type;
    public string description;
#nullable enable
    public string? metadata;
#nullable disable
    public bool enabled;
    public List<MediaItem> media;
}

[System.Serializable]
public class MediaItem
{
    public string mid;
    public string label;
    public string msid;
    public string type;
    public long? age_ms;
}
