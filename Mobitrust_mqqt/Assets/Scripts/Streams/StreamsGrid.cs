using Oculus.Platform.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StreamsGrid : MonoBehaviour
{
    [SerializeField] int gridSize;
    [SerializeField] GameObject _streamPlanePrefab;
    public List<ulong> StreamIds { get; private set; }
    public List<Janus> Streams { get; private set; }
    public List<StreamPlane> StreamPlanes { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out RectTransform rectTransform))
        {
            Debug.Log("stream grid has no rect transform");
            Application.Quit();
        }

        StreamIds = new(gridSize);
        Streams = new(gridSize);
        StreamPlanes = new(gridSize);

        for (int i = 0; i < gridSize; i++)
        {
            GameObject streamPlane = Instantiate(_streamPlanePrefab, transform);
            if (!streamPlane.TryGetComponent(out Janus janus))
            {
                Debug.Log("stream plane prefab has no Janus component");
                Application.Quit();
            }
            if (!streamPlane.TryGetComponent(out StreamPlane plane))
            {
                Debug.LogError($"Error getting StreamPlane component");
                Application.Quit();
            }
            StreamPlanes.Add(plane);
            Streams.Add(janus);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void AddStream(User user, ulong streamId)
    {
        try
        {
            if (user == null || streamId == 0)
            {
                return;
            }
            StreamIds.Add(user.streams[0].stream_id);
            Streams[StreamIds.Count - 1].StreamId = streamId;
            StreamPlanes[StreamIds.Count - 1].User = user;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
