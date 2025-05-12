using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR;

[RequireComponent(typeof(RawImage))]
public class Mapbox : MonoBehaviour
{
    [Header("Mapbox Token")]
    public string token;

    [Header("Map Settings")]
    public float zoom = 12f;
    public int bearing = 0;
    public int pitch = 0;

    public enum Style { Light, Dark, Streets, Outdoors, Satellite, SatelliteStreets }
    public Style mapStyle = Style.Streets;

    public enum CenterOn { Drone, VR }
    [Header("Map Center")]
    public CenterOn centerOn = CenterOn.Drone;

    [Header("Map Resolution")]
    public int scale = 1; // 1 or 2 for @2x

    // Internal positions
    private float droneLat, droneLon;
    private float vrLat, vrLon;

    // Cached values for changes
    private string tokenLast;
    private float zoomLast;
    private int bearingLast;
    private int pitchLast;
    private Style mapStyleLast;
    private int scaleLast;
    private float droneLatLast, droneLonLast;
    private float vrLatLast, vrLonLast;
    private CenterOn centerOnLast;

    private RawImage rawImage;
    private int mapWidth;
    private int mapHeight;

    private readonly string[] styleStrings = { "light-v10", "dark-v10", "streets-v11", "outdoors-v11", "satellite-v9", "satellite-streets-v11" };
    private bool needsUpdate = true;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Start()
    {
        UpdateDimensions();
        // Inicializa VR coordinates
        InputDevice head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        if (head.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos))
            SetVRPosition(pos.x, pos.z);
        StartCoroutine(UpdateMapTexture());
    }

    void Update()
    {
        if (needsUpdate || tokenLast != token ||
            !Mathf.Approximately(zoomLast, zoom) || bearingLast != bearing || pitchLast != pitch ||
            mapStyleLast != mapStyle || scaleLast != scale ||
            centerOnLast != centerOn ||
            !Mathf.Approximately(droneLatLast, droneLat) || !Mathf.Approximately(droneLonLast, droneLon) ||
            !Mathf.Approximately(vrLatLast, vrLat) || !Mathf.Approximately(vrLonLast, vrLon))
        {
            needsUpdate = false;
            UpdateDimensions();
            StartCoroutine(UpdateMapTexture());
        }
    }

    void UpdateDimensions()
    {
        var rect = rawImage.rectTransform.rect;
        mapWidth = Mathf.RoundToInt(rect.width);
        mapHeight = Mathf.RoundToInt(rect.height);
    }

    public void SetDronePosition(float lat, float lon)
    {
        droneLat = lat;
        droneLon = lon;
        needsUpdate = true;
    }

    public void SetVRPosition(float lat, float lon)
    {
        vrLat = lat;
        vrLon = lon;
        needsUpdate = true;
    }

    public void ForceUpdate() => needsUpdate = true;

    private IEnumerator UpdateMapTexture()
    {
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("[Mapbox] Access token is empty.");
            yield break;
        }

        string styleStr = styleStrings[(int)mapStyle];

        // Selects the center based on the selected option
        float centerLat = (centerOn == CenterOn.Drone) ? droneLat : vrLat;
        float centerLon = (centerOn == CenterOn.Drone) ? droneLon : vrLon;

        string lonD = droneLon.ToString(CultureInfo.InvariantCulture);
        string latD = droneLat.ToString(CultureInfo.InvariantCulture);
        string lonV = vrLon.ToString(CultureInfo.InvariantCulture);
        string latV = vrLat.ToString(CultureInfo.InvariantCulture);
        string lonC = centerLon.ToString(CultureInfo.InvariantCulture);
        string latC = centerLat.ToString(CultureInfo.InvariantCulture);
        string zoomStr = zoom.ToString(CultureInfo.InvariantCulture);
        string bearingStr = bearing.ToString(CultureInfo.InvariantCulture);
        string pitchStr = pitch.ToString(CultureInfo.InvariantCulture);
        string scaleSuffix = scale > 1 ? $"@{scale}x" : string.Empty;

        // Markers
        string droneMarker = $"pin-l+ff0000({lonD},{latD})";
        string vrMarker = $"pin-s+0000ff({lonV},{latV})";
        string overlay = $"{droneMarker},{vrMarker}";

        // URL with dynamic center
        string url = $"https://api.mapbox.com/styles/v1/mapbox/{styleStr}/static/"
                   + $"{overlay}/"
                   + $"{lonC},{latC},{zoomStr},{bearingStr},{pitchStr}/"
                   + $"{mapWidth}x{mapHeight}{scaleSuffix}?access_token={token}";

        Debug.Log("[Mapbox] Fetching map: " + url);

        using var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.LogError($"[Mapbox] Error fetching map: {request.error}");
        else
        {
            var tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            rawImage.texture = tex;
            Debug.Log("[Mapbox] Map updated successfully.");

            // Cache
            tokenLast = token;
            zoomLast = zoom;
            bearingLast = bearing;
            pitchLast = pitch;
            mapStyleLast = mapStyle;
            scaleLast = scale;
            droneLatLast = droneLat;
            droneLonLast = droneLon;
            vrLatLast = vrLat;
            vrLonLast = vrLon;
            centerOnLast = centerOn;
        }
    }
}