﻿using System.Collections;
using System.Collections.Generic;
using DeadMosquito.GoogleMapsView;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {

	//Rect object
	public GameObject uiObjectMap;

	//Google map
	private GoogleMapsView gMap = null;
	private Rect gMapLastRect = new Rect(0,0,0,0);
	private bool gMapCanChangeSize = true;
	// Use this for initialization
	void Start () 
	{
		//Create google map
		CreateGMap ();
	}

	GoogleMapsOptions CreateMapViewOptions()
	{
		var options = new GoogleMapsOptions();

		options.MapType(GoogleMapType.Hybrid);

		// Camera position
		options.Camera(new CameraPosition(
			new LatLng(52.0779648f, 4.334087f), 
			2f, 
			1f, 
			0f));

		// Specifies a LatLngBounds to constrain the camera target
		// so that when users scroll and pan the map, the camera target does not move outside these bounds.
		//var southWest = new LatLng(_boundsSouthWestPosLat, _boundsSouthWestPosLng);
		//var northEast = new LatLng(_boundsNorthEastPosLat, _boundsNorthEastPosLng);
		//options.LatLngBoundsForCameraTarget(new LatLngBounds(southWest, northEast));

		// Other settings
		options.AmbientEnabled(false);
		options.CompassEnabled(true);
		options.LiteMode(true);
		options.MapToolbarEnabled(false);
		options.RotateGesturesEnabled(true);
		options.ScrollGesturesEnabled(true);
		options.TiltGesturesEnabled(true);
		options.ZoomGesturesEnabled(true);
		options.ZoomControlsEnabled(true);

		return options;
	}

	bool ObjectChangedItsSize()
	{
		var currRect = GetScreenCoordinates (uiObjectMap.GetComponent<RectTransform> ());
		return 
		   gMapLastRect.x != currRect.x
		|| gMapLastRect.y != currRect.y
		|| gMapLastRect.width != currRect.width
		|| gMapLastRect.height != currRect.height;
	}

	//Create map
	void CreateGMap()
	{
		if (gMapCanChangeSize) 
		{
			//lock
			gMapCanChangeSize = false;
			gMapLastRect = GetScreenCoordinates(uiObjectMap.GetComponent<RectTransform>());
			//re-create map
			gMap = new GoogleMapsView(new GoogleMapsOptions());
			//show
			gMap.Show(gMapLastRect,OnMapReady);
		}
	}

	void OnMapReady()
	{
		gMap.IsMyLocationEnabled = true;
		gMap.UiSettings.IsMyLocationButtonEnabled = true;
		//Now we can call "DeleteGMap"
		//Unlock
		gMapCanChangeSize = true;
	}

	//Delete map
	void DeleteGMap()
	{
		if (gMap != null && gMapCanChangeSize) 
		{
			gMap.Dismiss ();
			gMap = null;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (ObjectChangedItsSize()) 
		{
			DeleteGMap ();
			CreateGMap ();
		}
	}

	//From scene to screen
	public Rect GetScreenCoordinates(RectTransform transform)
	{
		Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
		Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
		rect.x -= transform.pivot.x * size.x;
		rect.y -= (1.0f - transform.pivot.y) * size.y;
		rect.x = Mathf.CeilToInt(rect.x);
		rect.y = Mathf.CeilToInt(rect.y);
		return rect;
	}
}