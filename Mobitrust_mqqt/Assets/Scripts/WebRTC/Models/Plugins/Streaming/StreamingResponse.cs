public class StreamingResponse : JanusResponse
{
    public StreamingResponsePluginData plugindata;
}

public class StreamingResponsePluginData
{
    public string plugin;
    public StreamingResponseData data;
}

public class StreamingResponseData
{
    public string streaming;
}
