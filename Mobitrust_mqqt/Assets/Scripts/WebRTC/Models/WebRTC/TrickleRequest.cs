using System.Collections.Generic;
using System.Linq;
using Unity.WebRTC;
#nullable enable

[System.Serializable]
public class TrickleRequest : JanusRequest
{
    public List<TrickleRequestCandidate> candidates;

    public TrickleRequest(List<TrickleRequestCandidate> candidates)
    {
        janus = "trickle";
        this.candidates = candidates;
    }
}

[System.Serializable]
public class TrickleRequestCompleted : JanusRequest
{
    public TrickleRequestCandidateCompleted candidate;

    public TrickleRequestCompleted()
    {
        candidate = new TrickleRequestCandidateCompleted();
    }
}

[System.Serializable]
public class TrickleRequestCandidate
{
    public string sdpMid;
    public int? sdpMLineIndex;
    public string candidate;

    public TrickleRequestCandidate(string sdpMid, int? sdpMLineIndex, string candidate)
    {
        this.sdpMid = sdpMid;
        this.sdpMLineIndex = sdpMLineIndex;
        this.candidate = candidate;
    }
}

[System.Serializable]
public class TrickleRequestCandidateCompleted
{
    public bool completed;

    public TrickleRequestCandidateCompleted()
    {
        completed = true;
    }
}
