[System.Serializable]
public class ListAllRequest : JanusPluginRequest
{
    public ListAllRequestBody body;
    public ListAllRequest()
    {
        body = new ListAllRequestBody();
    }
}

[System.Serializable]
public class ListAllRequestBody
{
    public string request;

    public ListAllRequestBody()
    {
        request = "list";
    }
}

