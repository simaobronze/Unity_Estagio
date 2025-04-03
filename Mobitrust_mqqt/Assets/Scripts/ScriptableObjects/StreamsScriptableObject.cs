using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StreamsScriptableObject", menuName = "ScriptableObjects/Streams")]
public class StreamsScriptableObject : ScriptableObject
{
    public Dictionary<int, User> Streams { get; private set; } = new();

    private ulong activeStreamId = 0;

    public ulong ActiveStreamId
    {
        get
        {
            return activeStreamId;
        }
        private set
        {
            activeStreamId = value;
        }
    }

    private User activeUser = null;

    public User ActiveUser
    {
        get
        {
            return activeUser;
        }
        private set
        {
            activeUser = value;
        }
    }

    [System.NonSerialized]
    public UnityEvent<Dictionary<int, User>> streamsFetchedEvent;
    [System.NonSerialized]
    public UnityEvent<User, ulong> watchStreamEvent;
    [System.NonSerialized]
    public UnityEvent<ulong> activeStreamUpdatedEvent;
    [System.NonSerialized]
    public UnityEvent<int> streamRemovedEvent;

    private void OnEnable()
    {
        streamsFetchedEvent ??= new();
        watchStreamEvent ??= new();
        activeStreamUpdatedEvent ??= new();
        streamRemovedEvent ??= new();
    }

    private void OnDisable()
    {
        streamsFetchedEvent = null;
        watchStreamEvent = null;
        activeStreamUpdatedEvent = null;
        streamRemovedEvent = null;
    }

    public void FetchedStreams(Dictionary<int, User> streams)
    {
        Streams = streams;
        streamsFetchedEvent.Invoke(streams);
    }

    public void AddStream(User user, ulong streamId)
    {
        if (streamId == 0 || user == null)
        {
            return;
        }
        watchStreamEvent.Invoke(user, streamId);
    }

    public void SetActiveStream(User user, ulong streamId)
    {
        ActiveUser = user;
        ActiveStreamId = streamId;
        activeStreamUpdatedEvent.Invoke(streamId);
    }

    //public void RemoveStream(int streamId)
    //{
    //    Streams.Remove(streamId);
    //    streamRemovedEvent.Invoke(streamId);
    //}
}
