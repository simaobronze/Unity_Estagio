[System.Serializable]
public abstract class JanusRequest
{
    public string janus;
    public string transaction;

    protected JanusRequest()
    {
        this.transaction = Janus.GetNewRandomTransaction();
    }
}



