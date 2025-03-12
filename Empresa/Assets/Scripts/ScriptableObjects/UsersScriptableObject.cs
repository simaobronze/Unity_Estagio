using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UsersScriptableObject", menuName = "ScriptableObjects/Users")]
public class UsersScriptableObject : ScriptableObject
{
    public List<string> Drivers { get; private set; } = new()
    {
        "Gas",
        "Bitalino",
        "Hexoskin",
        "Micro",
        "Geo"
    };

    public User AuthenticatedUser { get; set; } = null;
    public User User { get; private set; } = null;
    public Dictionary<int, User> Users { get; private set; } = new();

    [System.NonSerialized]
    public UnityEvent<User> showUserEvent;
    [System.NonSerialized]
    public UnityEvent<Dictionary<int, User>> usersFetchedEvent;
    [System.NonSerialized]
    public UnityEvent<User> usersUpdateEvent;
    [System.NonSerialized]
    public UnityEvent<User> usersGeoUpdateEvent;

    private void OnEnable()
    {
        showUserEvent ??= new UnityEvent<User>();
        usersFetchedEvent ??= new UnityEvent<Dictionary<int, User>>();
        usersUpdateEvent ??= new UnityEvent<User>();
        usersGeoUpdateEvent ??= new UnityEvent<User>();
    }
    private void OnDisable()
    {
        showUserEvent = null;
        usersFetchedEvent = null;
        usersUpdateEvent = null;
        usersGeoUpdateEvent = null;
    }

    public void ShowUser(User user)
    {
        if (user == User)
        {
            user = null;
        }
        User = user;
        showUserEvent.Invoke(user);
    }

    public void FetchedUsers(Dictionary<int, User> users)
    {
        Users = users;
        usersFetchedEvent.Invoke(users);
    }

    public void UpdateUserSensor(User user)
    {
        try
        {
            if (!Users.ContainsKey(user.id))
            {
                return;
            }
			Users[user.id] = user;
			usersUpdateEvent.Invoke(user);
        } catch (Exception e)
        {
            Debug.LogError($"Error invoking update user sensor - {e}");
        }
    }
    
    public void UpdateUserGeoLocation(User user)
    {
        if (!Users.ContainsKey(user.id))
        {
            return;
        }
        Users[user.id] = user;
        usersGeoUpdateEvent.Invoke(user);
    }
}
