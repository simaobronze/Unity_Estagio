using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStreamListItem : MonoBehaviour
{
    [SerializeField] private GameObject _prefabStreamItem;
    [SerializeField] private GameObject _list;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUser(User user)
    {
        if (user == null )
        {
            return;
        }
        if (user.streams == null || user.streams.Count == 0 )
        {
            return;
        }
        foreach (var stream in user.streams)
        {
            GameObject streamGO = Instantiate(_prefabStreamItem, _list.transform);
            if (!streamGO.TryGetComponent(out StreamListItem streamItem))
            {
                Debug.LogError("Error getting StreamListItem component");
                Destroy(streamGO);
                return;
            }
            streamItem.SetStream(stream);
        }
    }
}
