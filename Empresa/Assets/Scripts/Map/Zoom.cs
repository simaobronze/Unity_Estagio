//using Microsoft.Maps.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
	//private MapRenderer mapRenderer;

	private void Start()
	{
		/*
		if (!TryGetComponent(out mapRenderer))
		{
			Debug.LogError("Map not found");
			return;
		}
		*/
	}

	public void ZoomIn()
	{
		/*
		if (mapRenderer == null)
			return;
		if (mapRenderer.ZoomLevel <= mapRenderer.MaximumZoomLevel)
			mapRenderer.ZoomLevel += 1;
		*/
	}

	public void ZoomOut()
	{
		/*
		if (mapRenderer == null)
			return;
		if (mapRenderer.ZoomLevel >= mapRenderer.MinimumZoomLevel)
			mapRenderer.ZoomLevel -= 1;
		*/
	}
}
