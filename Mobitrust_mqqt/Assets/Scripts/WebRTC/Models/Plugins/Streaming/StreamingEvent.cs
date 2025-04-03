[System.Serializable]
public class StreamingEvent : JanusResponse
{
    public StreamingEventPluginData plugindata;
}

[System.Serializable]
public class StreamingEventPluginData
{
    public string plugin;
    public StreamingEventData data;
}

[System.Serializable]
public class StreamingEventData
{
    public string streaming;
    public StreamingEventResult result;
}

[System.Serializable]
public class StreamingEventResult
{
    public string status;
}
