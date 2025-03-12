using MAGES.Interaction;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MapController : MonoBehaviour
{
    private const float HEIGHT_OFFSET = -1f;

    [SerializeField]
    private const float STEP_SIZE = 1f;
    [SerializeField]
    private MissionsScriptableObject _missionsScriptableObject;
    [SerializeField]
    private GameObject _mapGameObject;
    [SerializeField]
    private AbstractMap _map;
    [SerializeField]
    private Vector2 _mapTargetPosition;

    private bool _mapInPlace = false;

    void Start()
    {
        _missionsScriptableObject.missionChangeEvent.AddListener(_ChangeMapCoords);

        /*
        _mapGameObject = GameObject.Find("CitySimulatorMap");
        if(!_mapGameObject.TryGetComponent<AbstractMap>(out _map))
        {
            Debug.Log("Map not found");
        }
        */

        // _mapGameObject.transform.position = new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Forcing map to update position in the first frame of the app
        /*
        if (!_mapInPlace)
        {
            _mapGameObject.transform.position = new Vector3(0, 1f, 0);
            _mapInPlace = true;
        }
        */
    }

    private void _AddHeightToMapTransform()
    {
        //_mapGameObject.transform.position += new Vector3(0, 0.1f, 0);
    }

    private void _ResetHeight()
    {
        _mapGameObject.transform.position += new Vector3(0, -3.0f, 0);
    }

    private void _ChangeMapCoords(Mission mission)
    {
        if(mission == null)
        {
            return;
        }

        //Debug.Log($"Lat: {mission.data_configs.lat} Lon: {mission.data_configs.lon}");

        Vector2d latLon = new Vector2d(mission.data_configs.lat,mission.data_configs.lon);

        _map.UpdateMap(latLon/*,18.0f*/);
        _ResetHeight();

        //Commented until a way to find the flight plans orientation
        //_ChangeMapTilesTranform();

        /*
        if(mission.data_configs.lat != 0 && mission.data_configs.lon != 0)
        {
            _AddHeightToMapTransform();
        }
        else
        {
            _ResetHeight();
        }*/
    }

    public void ChangeMapCoords(float lat, float lon)
    {
        Vector2d latLon = new Vector2d(lat,lon);
        _map.UpdateMap(latLon);
    }

    public void ChangeMapCoords(double lat, double lon)
    {
        Vector2d latLon = new Vector2d(lat, lon);
        _map.UpdateMap(latLon);
    }

    private void _ChangeMapTilesTranform()
    {
        List<Transform> childrenList = _GetMapTiles();
        float tileScale = 1;
        for(int tileNumber = 0; tileNumber < 9; tileNumber++)
        {
            Transform tile = childrenList[tileNumber];
            Vector3 tilePosition = tile.position;
            switch (tileNumber)
            {
                //middle tile
                case 0:
                    tile.position = new Vector3(_mapTargetPosition.x, tilePosition.y, _mapTargetPosition.y);
                    break;
                //top right
                case 1:
                    tile.position = new Vector3(_mapTargetPosition.x - tileScale, tilePosition.y, _mapTargetPosition.y + tileScale);
                    Debug.Log($"Set tr to {tile.position.x}");
                    break;
                //top
                case 2:
                    tile.position = new Vector3(_mapTargetPosition.x - tileScale, tilePosition.y, _mapTargetPosition.y);
                    break;
                //top right
                case 3:
                    tile.position = new Vector3(_mapTargetPosition.x - tileScale, tilePosition.y, _mapTargetPosition.y - tileScale);
                    break;
                //right
                case 4:
                    tile.position = new Vector3(_mapTargetPosition.x, tilePosition.y, _mapTargetPosition.y + tileScale);
                    break;
                //left
                case 5:
                    tile.position = new Vector3(_mapTargetPosition.x, tilePosition.y, _mapTargetPosition.y - tileScale);
                    break;
                //down right
                case 6:
                    tile.position = new Vector3(_mapTargetPosition.x + tileScale, tilePosition.y, _mapTargetPosition.y + tileScale);
                    break;
                //down
                case 7:
                    tile.position = new Vector3(_mapTargetPosition.x + tileScale, tilePosition.y, _mapTargetPosition.y);
                    break;
                //down left
                case 8:
                    tile.position = new Vector3(_mapTargetPosition.x + tileScale, tilePosition.y, _mapTargetPosition.y - tileScale);
                    break;
            }
        }

    }

    private List<Transform> _GetMapTiles()
    {
        List<Transform> childrenList = new List<Transform>();

        for (int childIndex = 1; childIndex < _mapGameObject.transform.childCount; childIndex++)
        {
            childrenList.Add(_mapGameObject.transform.GetChild(childIndex));
        }

        if (childrenList.Count < 9)
        {
            return null;
        }

        return childrenList;
    }
}
