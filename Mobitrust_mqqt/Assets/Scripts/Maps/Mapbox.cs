using System.Collections;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Mapbox : MonoBehaviour
{
    public string token;
    public float centerLongitude;
    public float centerLatitude;
    public float zoom = 12f;
    public int bearing = 0;
    public int pitch = 0;
    public enum style { Light, Dark, Streets, Outdoors, Satallite, SatelliteStreets};
    public style mapStyle = style.Streets;
    public enum resolution { low = 1, high = 2 };
    public resolution mapResolution = resolution.high;

    private int mapWidth = 800;
    private int mapHeight = 600;
    private string[] styleStr = new string[] { "light-v10", "dark-v10", "streets-v11", "outdoors-v11", "satellite-v9", "satellite-streets-v11" };
    private string url = "";
    private bool mapIsLoading = false; 
    private Rect mapRect;
    private bool updateMap = true;

    private string tokenLast;
    private float centerLatitudeLast;
    private float centerLongitudeLast;
    private float zoomLast = 12f;
    private int bearingLast = 0;
    private int pitchLast = 0;
    private style mapStyleLast = style.Streets;
    private resolution mapResolutionLast = resolution.high;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GetMapbox());
        mapRect = gameObject.GetComponent<RawImage>().rectTransform.rect;
        mapWidth = (int)Mathf.Round(mapRect.width);
        mapHeight = (int)Mathf.Round(mapRect.height);

    }

    // Update is called once per frame
    void Update()
    {
        if (updateMap && (tokenLast != token || !Mathf.Approximately(centerLongitudeLast, centerLatitude) || Mathf.Approximately(centerLongitudeLast, centerLongitude) || zoomLast != zoom ||
            bearingLast != bearing || pitchLast != pitch || mapStyleLast != mapStyle || mapResolutionLast != mapResolution)) {
            
            mapRect = gameObject.GetComponent<RawImage>().rectTransform.rect;
            mapWidth = (int)Mathf.Round(mapRect.width);
            mapHeight = (int)Mathf.Round(mapRect.height);
            StartCoroutine(GetMapbox());
            updateMap = false;
        }        
    }

    IEnumerator GetMapbox()
    {
        string lon = centerLongitude.ToString(CultureInfo.InvariantCulture);
        string lat = centerLatitude.ToString(CultureInfo.InvariantCulture);
        string zm = zoom.ToString(CultureInfo.InvariantCulture);
        string brg = bearing.ToString(CultureInfo.InvariantCulture);
        string ptc = pitch.ToString(CultureInfo.InvariantCulture);

        url = $"https://api.mapbox.com/styles/v1/mapbox/{styleStr[(int)mapStyle]}/static/"
            + $"{lon},{lat},{zm},{brg},{ptc}/"
            + $"{mapWidth}x{mapHeight}"
            + $"?access_token={token}";

        //Debug.Log ("O url é: " + url);
        mapIsLoading = true;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if(www.result != UnityWebRequest.Result.Success)
            {
            Debug.Log("Error: " + www.error);
        }
            else
        {
            mapIsLoading = false;
            gameObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            tokenLast = token;
            centerLatitudeLast = centerLatitude;
            centerLongitudeLast = centerLongitude;
            zoomLast = zoom;
            bearingLast = bearing;
            pitchLast = pitch;
            mapStyleLast = mapStyle;
            mapResolutionLast = mapResolution;
            updateMap = true;
        }
    }
}
