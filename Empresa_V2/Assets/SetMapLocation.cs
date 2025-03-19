using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;
using System.Threading;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class SetMapLocation : MonoBehaviour
{
    public TMP_InputField latitude;
    public TMP_InputField longitude;
    //public GameObject screen;
    //public GameObject ground;
    //public TMP_Text label;
    //public TMP_Text debug;
    //public Material stencilMat;
    //public Material loadingMat;
    //public GameObject player;
    [SerializeField]
    private AbstractMap _map;
    [SerializeField]
    private GameObject _mapGameObject;
    [SerializeField]
    private MapController _mapController;

    public void SetLocation()
    {
        float parsedLat, parsedLng;
        bool success = true;

        success = float.TryParse(latitude.text, out parsedLat);
        if (!success)
            return;

        success = float.TryParse(longitude.text, out parsedLng);
        if (!success)
            return;

        foreach (Transform child in _mapController.transform)
        {
            GetComponent<Renderer>().enabled = false;
        }

        _mapController.ChangeMapCoords(parsedLat, parsedLng);
        _map.SetCenterLatitudeLongitude(new Mapbox.Utils.Vector2d(parsedLat, parsedLng));
        _map.UpdateMap();
    }

    private void Start()
    {

    }

    void Update()
    {

    } 
}