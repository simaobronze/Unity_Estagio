[System.Serializable]
public class StartRequest : JanusPluginRequest
{
    public StartRequestBody body;
    public Jsep jsep;
    public StartRequest(Jsep jsep)
    {
        body = new StartRequestBody();
        this.jsep = jsep;
    }
}

[System.Serializable]
public class StartRequestBody
{
    public string request;

    public StartRequestBody()
    {
        request = "start";
    }
}


