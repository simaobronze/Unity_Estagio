using Mapbox.Unity.Location;
//using Microsoft.Geospatial;
//using Microsoft.Maps.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MovementMapController : MonoBehaviour
{
	//private MapRenderer mapRenderer;
	private GameObject raycaster;
	private Vector3 lastHitPoint = Vector3.zero;
	//private LatLonAlt lastLatLon;
	private Vector3 lastHit = Vector3.zero;
	private bool stopMovement = false;
	[SerializeField] private double distance = 0.0003f;
	[SerializeField] private float speedMovement = 1.5f;
	[SerializeField] private float distanceLastPoint = 0.1f;


	private void Start()
	{
		/*
		if (!TryGetComponent(out mapRenderer))
		{
			Debug.LogError("Map or pin layer not found");
			return;
		}
		*/
		raycaster = GameObject.Find("ControllerPointerPose");
	}

	void Update()
	{
		Ray ray = new (raycaster.transform.position, raycaster.transform.forward);
		MoveMap(ray);
	}

	private void MoveMap(Ray ray)
	{
		if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0)
		{
			stopMovement = true;
			/*
			if (mapRenderer.Raycast(ray, out MapRendererRaycastHit hitInfo))
			{
				if (lastHitPoint == Vector3.zero)
				{
					lastHitPoint = hitInfo.Point;
					lastLatLon = hitInfo.Location;
				}
				else
				{
					if (lastHit != Vector3.zero)
					{
						Debug.Log("Distancia" + Vector3.Distance(lastHit, hitInfo.Point));
						if (Vector3.Distance(lastHit, hitInfo.Point) < distanceLastPoint)
						{
							mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(mapRenderer.Center.LatitudeInDegrees, mapRenderer.Center.LongitudeInDegrees), mapRenderer.ZoomLevel), MapSceneAnimationKind.Linear, 10);
							return;
						}
					}
					var center = mapRenderer.Center;
					double deltaLatitude = hitInfo.Location.LatitudeInDegrees - lastLatLon.LatitudeInDegrees;
					double deltaLongitude = hitInfo.Location.LongitudeInDegrees - lastLatLon.LongitudeInDegrees;
					if ((deltaLatitude<=distance && deltaLatitude >= -distance) && (deltaLongitude <= distance && deltaLongitude >= -distance))
					{
						lastHit = hitInfo.Point;
						return;
					}
					mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(center.LatitudeInDegrees - deltaLatitude*speedMovement, center.LongitudeInDegrees - deltaLongitude*speedMovement), mapRenderer.ZoomLevel), MapSceneAnimationKind.Linear, 10);
					lastHit = hitInfo.Point;
				}
			}
			else
			{
				lastHitPoint = Vector3.zero;
				lastHit = Vector3.zero;
			}
			*/
		}
		else if (stopMovement)
		{
			lastHitPoint = Vector3.zero;
			lastHit = Vector3.zero;
			stopMovement = false;
			//mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(mapRenderer.Center.LatitudeInDegrees, mapRenderer.Center.LongitudeInDegrees), mapRenderer.ZoomLevel), MapSceneAnimationKind.Linear, 10);
		}
	}

	//private void TapMap(Ray ray)
	//{
	//	if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0)
	//	{
	//		if (mapRenderer.Raycast(ray, out MapRendererRaycastHit hitInfo))
	//		{
	//			LatLonAlt latLonAlt = hitInfo.Location;
	//			Debug.Log("Latitude: " + latLonAlt.LatitudeInDegrees + ", Longitude: " + latLonAlt.LongitudeInDegrees);
	//			mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(latLonAlt.LatitudeInDegrees, latLonAlt.LongitudeInDegrees), 19));
	//		}
	//	}
	//}
}
