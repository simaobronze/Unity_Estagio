using Oculus.Platform.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamsScreening : MonoBehaviour
{
    [SerializeField] private GameObject _list;
    [SerializeField] private GameObject _streamGridPrefab;
    [SerializeField] private StreamsScriptableObject _streamsStore;
    public List<StreamsGrid> Pages { get; private set; } = new();

    // Start is called before the first frame update
    void Start()
    {
        if (_streamGridPrefab == null)
        {
            Debug.Log("stream grid prefab is null");
            Application.Quit();
        }
        GameObject firstPage = Instantiate(_streamGridPrefab, _list.transform);
        if (!firstPage.TryGetComponent(out StreamsGrid grid))
        {
            Debug.Log("stream grid prefab has no StreamPage component");
            Application.Quit();
        }
        Pages.Add(grid);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        //_streamsStore.watchStreamEvent.AddListener(AddStream);
    }

    private void OnDisable()
    {
        //_streamsStore.watchStreamEvent.RemoveListener(AddStream);
    }

    private void AddStream(User user, ulong streamId)
    {
        try
        {
            Debug.Log($"adding stream from user {user.id}");
            foreach (var page in Pages)
            {
                if (page.StreamIds.Count >= 9)
                {
                    continue;
                }
                else
                {
                    page.AddStream(user, streamId);
                    return;
                }
            }
            GameObject newPage = Instantiate(_streamGridPrefab, _list.transform);
            if (!newPage.TryGetComponent(out StreamsGrid grid))
            {
                Debug.LogError("stream grid prefab has no StreamPage component");
                Application.Quit();
            }
            grid.AddStream(user, streamId);
            Pages.Add(grid);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error adding stream - e: {e}");
            return;
        }
    }


}
