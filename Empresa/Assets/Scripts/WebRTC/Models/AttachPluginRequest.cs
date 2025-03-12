[System.Serializable]
public class AttachPluginRequest : JanusRequest
{
    public string plugin;
    
    public AttachPluginRequest(string plugin)
    {
        this.janus = "attach";
        this.plugin = plugin;
    }
        
        
}