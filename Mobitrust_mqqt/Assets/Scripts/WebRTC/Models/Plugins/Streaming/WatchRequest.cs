[System.Serializable]
public class WatchRequest : JanusPluginRequest
{
    public WatchRequestBody body;
    public WatchRequest(ulong mountpoint)
    {
        body = new WatchRequestBody(mountpoint);
    }
}

[System.Serializable]
public class WatchRequestBody
{
    public string request;
    public ulong id;
    public WatchRequestBody(ulong mountpoint)
    {
        id = mountpoint;
        request = "watch";
    }
}

