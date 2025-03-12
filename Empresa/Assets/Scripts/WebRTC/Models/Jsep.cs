[System.Serializable]
public class JsepResponse: JanusResponse
{
    public Jsep jsep;
}

[System.Serializable]
public class Jsep
{
    public string type; 
    public string sdp;

    public Jsep(string type, string sdp)
    {
        this.type = type;
        this.sdp = sdp;
    }
}