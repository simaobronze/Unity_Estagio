using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserStreamsListing : MonoBehaviour
{
    [SerializeField] private Transform _scrollViewContent;
    [SerializeField] private GameObject _userStreamItemPrefab;
    [SerializeField] private StreamsScriptableObject _streamsStore;

    private Dictionary<int, GameObject> _listedStreams = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _streamsStore.streamsFetchedEvent.AddListener(UpdateList);
    }

    private void OnDisable()
    {
        _streamsStore.streamsFetchedEvent.RemoveListener(UpdateList);
    }

    private void UpdateList(Dictionary<int, User> users)
    { 
        foreach (var user in users)
        {
            if (_listedStreams.ContainsKey(user.Key))
            {
                continue;
            }
            if (user.Value.streams == null || user.Value.streams.Count <= 0) 
            {
                Debug.Log("User with no streams");
                continue;
            }
            GameObject userStreamGO = Instantiate(_userStreamItemPrefab, _scrollViewContent);
            if (!userStreamGO.TryGetComponent(out UserListItem userItem))
            {
                Debug.Log("Error getting UserStreamListItem component");
                Destroy(userStreamGO);
                continue;
            }
            userItem.SetUser(user.Value);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollViewContent as RectTransform);
    }
}
