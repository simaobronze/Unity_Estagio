using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMap : MonoBehaviour
{
    [SerializeField]
    private AbstractMap _abstractMap;
    [SerializeField]
    private MapController _mapController;
    public void Move(string direction)
    {
        Debug.Log(direction);
        switch (direction)
        {
            case "up":
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x + 0.005, _abstractMap.CenterLatitudeLongitude.y);
                break;
            case "down":
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x - 0.005, _abstractMap.CenterLatitudeLongitude.y);
                break;
            case "left":
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x, _abstractMap.CenterLatitudeLongitude.y - 0.005);
                break;
            case "right":
                _mapController.ChangeMapCoords(_abstractMap.CenterLatitudeLongitude.x, _abstractMap.CenterLatitudeLongitude.y + 0.005);
                break;
        }
    }
}
