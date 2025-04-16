using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class UserListing : MonoBehaviour
{
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject _userItemPrefab;
    [SerializeField] private UsersScriptableObject _usersStore;

    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    private void OnEnable()
    {
        _usersStore.usersFetchedEvent.AddListener(HandleUsersFetched);
    }

    private void OnDisable()
    {
        _usersStore.usersFetchedEvent.RemoveListener(HandleUsersFetched);
    }

    public void HandleUsersFetched(Dictionary<int, User> users)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() =>
            {
                UpdatedUsers(users);
            });
        }
    }

    public void UpdatedUsers(Dictionary<int, User> u)
    {
        try
        {
            Dictionary<int, User> users = u;
            foreach (var user in users.Values)
            {
                GameObject newUserItem = Instantiate(_userItemPrefab, scrollViewContent);
                if (newUserItem.TryGetComponent(out UserListItem item))
                {
                    item.SetUser(user);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"{MethodBase.GetCurrentMethod().Name} - Error updating list of users, ex - {ex.Message}");
            return;
        }
    }

    internal void ShowUser(User user)
    {
        _usersStore.ShowUser(user);
    }
}
