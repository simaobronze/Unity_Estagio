using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneStream
{
    private GameObject _button;
    private User _streamUser;

    public DroneStream(GameObject button, User streamUser)
    {
        _button = button;
        _streamUser = streamUser;
    }

    public GameObject getButton()
    {
        return _button;
    }

    public User getStreamUser() 
    {
        return _streamUser;
    }
}
