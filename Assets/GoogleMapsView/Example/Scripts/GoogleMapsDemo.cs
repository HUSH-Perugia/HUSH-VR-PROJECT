namespace DeadMosquito.GoogleMapsView.Demo
{
	using Internal;
	using UnityEngine;
	using UnityEngine.UI;

	public class GoogleMapsDemo : MonoBehaviour
	{
		#region map_options

		[Header("Bounds")]
		public Toggle boundsToggle;

		[Header("Options Toggles")]
		public Toggle ambientToggle;

		public Toggle compassToggle;
		public Toggle liteModeToggle;
		public Toggle mapToolbarToggle;
		public Toggle rotateGesturesToggle;
		public Toggle scrollGesturesToggle;
		public Toggle tiltGesturesToggle;
		public Toggle zoomGesturesToggle;
		public Toggle zoomControlsToggle;

		[Header("Map Type")]
		public Dropdown mapType;

		[Header("Min/Max Zoom")]
		public InputField minZoom;

		public InputField maxZoom;

		[Header("Camera Position")]
		public Slider camPosLat;

		public Slider camPosLng;
		public Slider camPosZoom;
		public Slider camPosTilt;
		public Slider camPosBearing;

		[Header("Camera Labels")]
		public Text camPosLatText;

		public Text camPosLngText;
		public Text camPosZoomText;
		public Text camPosTiltText;
		public Text camPosBearingText;

		[Header("Bound South-West")]
		[Range(-90, 90)]
		[SerializeField]
		float _boundsSouthWestPosLat;

		[Range(-180, 180)]
		[SerializeField]
		float _boundsSouthWestPosLng;

		[Header("Bound North-East")]
		[Range(-90, 90)]
		[SerializeField]
		float _boundsNorthEastPosLat;

		[Range(-180, 180)]
		[SerializeField]
		float _boundsNorthEastPosLng;

		#endregion

		#region circle_options

		[Header("Circle Center")]
		public Slider circleLat;

		public Slider circleLng;
		public Slider circleStokeWidth;
		public Slider circleRadius;
		public Toggle circleVisibilityToggle;

		#endregion

		#region marker_options

		[Header("Marker Center")]
		public Slider markerLat;

		public Slider markerLng;

		#endregion

		public RectTransform rect;

		GoogleMapsView _map;
		Circle _circle;
		Marker _marker;
		GroundOverlay _groundOverlay;
		Polyline _polyline;
		Polygon _coloradoPolygon;

		bool _startWasInvoked;
		
		// Tracking orientation changes
		int _screenWidth;
		int _screenHeight;

		void Awake()
		{
			SetupEvents();
			SetInitialOptionsValues();
		}

		void Start()
		{
			// Show the map when the demo starts
			OnShow();
			_startWasInvoked = true;
			_screenWidth = Screen.width;
			_screenHeight = Screen.height;
		}

		void Update()
		{
			ListenForOrientationChanges();
		}

		void ListenForOrientationChanges()
		{
			if (_screenWidth != Screen.width || _screenHeight != Screen.height)
			{
				OnShow();
				_screenWidth = Screen.width;
				_screenHeight = Screen.height;
			}
		}

		void OnApplicationPause(bool pauseStatus)
		{
			// Handle OnPause/OnResume
			if (pauseStatus)
			{
				Dismiss();
			}
			else
			{
				if (_startWasInvoked)
				{
					// Start handling onResume only after start was called
					OnShow();
				}
			}
		}

		void SetInitialOptionsValues()
		{
			mapType.value = (int) GoogleMapType.Normal;

			// Camera position
			camPosLat.value = 52.0779648f;
			camPosLng.value = 4.334087f;
			camPosZoom.value = 2f;
			camPosTilt.value = 1f;
			camPosBearing.value = 0f;

			// Zoom constraints
			minZoom.text = "1.0";
			maxZoom.text = "20.0";
		}

		void SetupEvents()
		{
			// Camera position
			camPosLat.onValueChanged.AddListener(newValue => { camPosLatText.text = string.Format("Lat:{0}", newValue); });
			camPosLng.onValueChanged.AddListener(newValue => { camPosLngText.text = string.Format("Lng:{0}", newValue); });
			camPosZoom.onValueChanged.AddListener(
				newValue => { camPosZoomText.text = string.Format("Zoom:{0}", newValue); });
			camPosTilt.onValueChanged.AddListener(
				newValue => { camPosTiltText.text = string.Format("Tilt:{0}", newValue); });
			camPosBearing.onValueChanged.AddListener(newValue => { camPosBearingText.text = string.Format("Bearing:{0}", newValue); });
		}

		/// <summary>
		/// Shows the <see cref="GoogleMapsView"/>
		/// </summary>
		public void OnShow()
		{
			Dismiss();

			_map = new GoogleMapsView(CreateMapViewOptions());
			_map.Show(RectTransformToScreenSpace(rect), OnMapReady);
		}

		void OnMapReady()
		{
			Debug.Log("Map is ready: " + _map);

			// UNCOMMENT if testing with showing users location. DON'T FORGET MANIFEST LOCATION PERMISSION!!!
			// _map.IsMyLocationEnabled = true;
			// _map.UiSettings.IsMyLocationButtonEnabled = true;

			_map.SetOnCircleClickListener(circle => Debug.Log("Circle clicked: " + circle));
			_map.SetOnMarkerClickListener(marker => Debug.Log("Marker clicked: " + marker), false);
			_map.SetOnMapClickListener(point =>
			{
				Debug.Log("Map clicked: " + point);
				_map.AddMarker(DemoUtils.RandomColorMarkerOptions(point));
			});
			_map.SetOnLongMapClickListener(point =>
			{
				Debug.Log("Map long clicked: " + point);
				_map.AddCircle(DemoUtils.RandomColorCircleOptions(point));
			});

			// When the map is ready we can start drawing on it
			AddCircle();
			AddMarker();
			AddGroundOverlay();
			AddPolyline();
			AddPolygon();
		}

		void AddCircle()
		{
			_circle = _map.AddCircle(DemoUtils.CreateInitialCircleOptions());
		}

		void AddMarker()
		{
			_marker = _map.AddMarker(DemoUtils.CreateInitialMarkerOptions());
		}

		void AddGroundOverlay()
		{
			_groundOverlay = _map.AddGroundOverlay(DemoUtils.CreateInitialGroundOverlayOptions());
		}

		void AddPolyline()
		{
			_polyline = _map.AddPolyline(DemoUtils.CreateInitialPolylineOptions());
		}

		void AddPolygon()
		{
			_coloradoPolygon = _map.AddPolygon(DemoUtils.CreateColoradoStatePolygonOptions());
		}

		GoogleMapsOptions CreateMapViewOptions()
		{
			var options = new GoogleMapsOptions();

			options.MapType((GoogleMapType) mapType.value);

			// Camera position
			options.Camera(CameraPosition);

			// Bounds
			if (boundsToggle.isOn)
			{
				options.LatLngBoundsForCameraTarget(Bounds);
			}

			options.AmbientEnabled(ambientToggle.isOn);
			options.CompassEnabled(compassToggle.isOn);
			options.LiteMode(liteModeToggle.isOn);
			options.MapToolbarEnabled(mapToolbarToggle.isOn);
			options.RotateGesturesEnabled(rotateGesturesToggle.isOn);
			options.ScrollGesturesEnabled(scrollGesturesToggle.isOn);
			options.TiltGesturesEnabled(tiltGesturesToggle.isOn);
			options.ZoomGesturesEnabled(zoomGesturesToggle.isOn);
			options.ZoomControlsEnabled(zoomControlsToggle.isOn);

			options.MinZoomPreference(float.Parse(minZoom.text));
			options.MaxZoomPreference(float.Parse(maxZoom.text));

			return options;
		}

		LatLngBounds Bounds
		{
			get
			{
				var southWest = new LatLng(_boundsSouthWestPosLat, _boundsSouthWestPosLng);
				var northEast = new LatLng(_boundsNorthEastPosLat, _boundsNorthEastPosLng);
				return new LatLngBounds(southWest, northEast);
			}
		}

		CameraPosition CameraPosition
		{
			get
			{
				return new CameraPosition(
					new LatLng(camPosLat.value, camPosLng.value),
					camPosZoom.value,
					camPosTilt.value,
					camPosBearing.value);
			}
		}

		#region update_buttons_click

		public void OnUpdateCircleButtonClick()
		{
			if (_circle == null)
			{
				AddCircle();
				return;
			}

			Debug.Log("Current circle: " + _circle + ", updating properties...");
			UpdateCircleProperties();
		}

		public void OnRemoveCircleClick()
		{
			if (_circle != null)
			{
				_circle.Remove();
				_circle = null;
				Debug.Log("Circle was removed.");
			}
		}

		public void OnUpdateMarkerButtonClick()
		{
			if (_marker == null)
			{
				AddMarker();
				return;
			}

			Debug.Log("Current marker: " + _marker + ", updating properties...");
			UpdateMarkerProperties();
		}

		public void OnShowMarkerInfoWindow()
		{
			if (_marker == null)
			{
				return;
			}

			_marker.ShowInfoWindow();
		}

		public void OnRemoveMarkerClick()
		{
			if (_marker == null)
			{
				return;
			}

			_marker.Remove();
			_marker = null;
			Debug.Log("Marker was removed.");
		}

		public void OnUpdateGroundOverlayClick()
		{
			if (_groundOverlay == null)
			{
				AddGroundOverlay();
				return;
			}

			Debug.Log("Current ground overlay: " + _groundOverlay + ", updating properties...");
			UpdateGrondOverlayProperties();
		}

		public void OnRemoveGroundOverlayClick()
		{
			if (_groundOverlay == null)
			{
				return;
			}

			_groundOverlay.Remove();
			_groundOverlay = null;
			Debug.Log("Ground overlay was removed.");
		}

		public void OnUpdatePolylineClick()
		{
			if (_polyline == null)
			{
				AddPolyline();
				return;
			}

			Debug.Log("Current polyline: " + _polyline + ", updating properties...");
			UpdatePolylineProperties();
		}

		public void OnRemovePolylineClick()
		{
			if (_polyline == null)
			{
				return;
			}

			_polyline.Remove();
			_polyline = null;
			Debug.Log("Polyline was removed.");
		}

		public void OnUpdatePolygonClick()
		{
			if (_coloradoPolygon == null)
			{
				AddPolygon();
				return;
			}
			
			Debug.Log("Current polygon: " + _coloradoPolygon + ", updating properties...");
			UpdatePolygonProperties();
		}

		public void OnRemovePolygonClick()
		{
			if (_coloradoPolygon == null)
			{
				return;
			}

			_coloradoPolygon.Remove();
			_coloradoPolygon = null;
			Debug.Log("Polygon was removed.");
		}

		/// <summary>
		/// Removes all markers, polylines, polygons, overlays, etc from the map.
		/// </summary>
		public void OnClearMapClick()
		{
			if (_map == null)
			{
				return;
			}

			_map.Clear();
			// All the elemnts are now removed, we cannot access them any more
			_circle = null;
			_marker = null;
			_groundOverlay = null;
			_polyline = null;
		}

		public void OnTestUiSettingsButtonClick(bool enable)
		{
			if (_map == null)
			{
				return;
			}

			EnableAllSettings(_map.UiSettings, enable);
		}

		static void EnableAllSettings(UiSettings settings, bool enable)
		{
			Debug.Log("Current Ui Settings: " + settings);

			// Buttons/other
			settings.IsCompassEnabled = enable;
			settings.IsIndoorLevelPickerEnabled = enable;
			settings.IsMapToolbarEnabled = enable;
			settings.IsMyLocationButtonEnabled = enable;
			settings.IsZoomControlsEnabled = enable;

			// Gestures
			settings.IsRotateGesturesEnabled = enable;
			settings.IsScrollGesturesEnabled = enable;
			settings.IsTiltGesturesEnabled = enable;
			settings.IsZoomGesturesEnabled = enable;
			settings.SetAllGesturesEnabled(enable);
		}

		#endregion

		void UpdateCircleProperties()
		{
			_circle.Center = new LatLng(circleLat.value, circleLng.value);
			_circle.FillColor = ColorUtils.RandomColor();
			_circle.StrokeColor = ColorUtils.RandomColor();
			_circle.StrokeWidth = circleStokeWidth.value;
			_circle.Radius = circleRadius.value;
			_circle.ZIndex = 1f;
			_circle.IsVisible = circleVisibilityToggle.isOn;
			_circle.IsClickable = true;
		}

		void UpdateMarkerProperties()
		{
			_marker.Position = new LatLng(markerLat.value, markerLng.value);
			_marker.Alpha = 1f;
			_marker.DragStatus = true;
			_marker.Flat = true;
			_marker.IsVisible = true;
			_marker.Rotation = 0;
			_marker.SetAnchor(0.5f, 1f);
			_marker.SetInfoWindowAnchor(0.5f, 1f);
			_marker.Snippet = "Updated Marker";
			_marker.Title = "You can drag this marker";
			_marker.ZIndex = 2;
		}

		void UpdateGrondOverlayProperties()
		{
			_groundOverlay.Bearing = 135;
			_groundOverlay.IsClickable = true;
			_groundOverlay.IsVisible = true;
			_groundOverlay.Position = DemoUtils.BerlinLatLng;
			_groundOverlay.Transparency = 0.5f;
			_groundOverlay.ZIndex = 3;
			_groundOverlay.SetDimensions(200000); // Just setting twice to test
			_groundOverlay.SetDimensions(200000, 200000);
			_groundOverlay.SetImage(ImageDescriptor.FromAsset("overlay.png"));
			_groundOverlay.SetPositionFromBounds(DemoUtils.BerlinLatLngBounds);
		}

		void UpdatePolylineProperties()
		{
			_polyline.Points = DemoUtils.UsaPolylinePoints;
			_polyline.StartCap = new RoundCap();
			_polyline.StartCap = new SquareCap();
			_polyline.JointType = JointType.Bevel;
			_polyline.Width = 20f;
			_polyline.Color = ColorUtils.RandomColor();
			_polyline.IsGeodesic = false;
			_polyline.IsVisible = true;
			_polyline.IsClickable = true;
			_polyline.ZIndex = 1f;
		}

		void UpdatePolygonProperties()
		{
			_coloradoPolygon.Points = DemoUtils.ColoradoBorders;
			_coloradoPolygon.Holes = DemoUtils.ColoradoHoles;
			_coloradoPolygon.FillColor = Color.yellow;
			_coloradoPolygon.Color = Color.blue;
			_coloradoPolygon.StrokeJointType = JointType.Bevel;
			_coloradoPolygon.StrokeWidth = 25f;
			_coloradoPolygon.IsGeodesic = false;
			_coloradoPolygon.IsVisible = true;
			_coloradoPolygon.IsClickable = true;
			_coloradoPolygon.ZIndex = 1f;
		}

		void Dismiss()
		{
			if (_map != null)
			{
				_map.Dismiss();
				_map = null;
			}
		}

		#region camera_animations

		public void AnimateCameraNewCameraPosition()
		{
			AnimateCamera(CameraUpdate.NewCameraPosition(CameraPosition));
		}

		public void AnimateCameraNewLatLng()
		{
			AnimateCamera(CameraUpdate.NewLatLng(new LatLng(camPosLat.value, camPosLng.value)));
		}

		public void AnimateCameraNewLatLngBounds1()
		{
			AnimateCamera(CameraUpdate.NewLatLngBounds(Bounds, 10));
		}

		public void AnimateCameraNewLatLngBounds2()
		{
			AnimateCamera(CameraUpdate.NewLatLngBounds(Bounds, 100, 100, 10));
		}

		public void AnimateCameraNewLatLngZoom()
		{
			const int zoom = 10;
			AnimateCamera(CameraUpdate.NewLatLngZoom(new LatLng(camPosLat.value, camPosLng.value), zoom));
		}

		public void AnimateCameraScrollBy()
		{
			const int xPixel = 250;
			const int yPixel = 250;
			AnimateCamera(CameraUpdate.ScrollBy(xPixel, yPixel));
		}

		public void AnimateCameraZoomByWithFixedLocation()
		{
			const int amount = 5;
			const int x = 100;
			const int y = 100;
			AnimateCamera(CameraUpdate.ZoomBy(amount, x, y));
		}

		public void AnimateCameraZoomByAmountOnly()
		{
			const int amount = 5;
			AnimateCamera(CameraUpdate.ZoomBy(amount));
		}

		public void AnimateCameraZoomIn()
		{
			AnimateCamera(CameraUpdate.ZoomIn());
		}

		public void AnimateCameraZoomOut()
		{
			AnimateCamera(CameraUpdate.ZoomOut());
		}

		public void AnimateCameraZoomTo()
		{
			const int zoom = 10;
			AnimateCamera(CameraUpdate.ZoomTo(zoom));
		}

		void AnimateCamera(CameraUpdate cameraUpdate)
		{
			if (_map == null)
			{
				return;
			}

			_map.AnimateCamera(cameraUpdate);
		}

		#endregion

		#region helpers

		static Rect RectTransformToScreenSpace(RectTransform transform)
		{
			Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
			Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
			rect.x -= transform.pivot.x * size.x;
			rect.y -= (1.0f - transform.pivot.y) * size.y;
			rect.x = Mathf.CeilToInt(rect.x);
			rect.y = Mathf.CeilToInt(rect.y);
			return rect;
		}

		#endregion
	}
}