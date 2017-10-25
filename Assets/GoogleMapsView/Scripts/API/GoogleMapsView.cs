namespace DeadMosquito.GoogleMapsView
{
	using System;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// Main class to create and show GoogleMapsView
	/// </summary>
	public sealed class GoogleMapsView
	{
		const string ClassName = "com.deadmosquitogames.gmaps.GoogleMapsManager";

		bool _wasDismissed;
		readonly GoogleMapsOptions _options;

		AndroidJavaObject _ajo;

		AndroidJavaObject GoogleMapAJO
		{
			get { return _ajo.CallAJO("getMap"); }
		}

		/// <summary>
		/// Gets the user interface settings for the map.
		/// </summary>
		public UiSettings UiSettings
		{
			get
			{
				return GoogleMapUtils.IsAndroidRuntime
					? new UiSettings(GoogleMapAJO.MainThreadCallAJO("getUiSettings"))
					: new UiSettings();
			}
		}

		/// <summary>
		/// While enabled and the location is available, the my-location layer continuously draws an indication of a user's current location and bearing, and displays UI controls that allow a user to interact with their location (for example, to enable or disable camera tracking of their location and bearing).
		/// In order to use the my-location-layer feature you need to request permission for either ACCESS_COARSE_LOCATION or ACCESS_FINE_LOCATION unless you have set a custom location source.
		/// </summary>
		public bool IsMyLocationEnabled
		{
			get { return GetValue<bool>("isMyLocationEnabled"); }
			set { SetValue("setMyLocationEnabled", value); }
		}

		public GoogleMapsView(GoogleMapsOptions options)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			_options = options ?? new GoogleMapsOptions();
#if UNITY_ANDROID
			GoogleMapsSceneHelper.Init();
#endif
			JniToolkitUtils.RunOnUiThread(() => { _ajo = new AndroidJavaObject(ClassName, JniToolkitUtils.Activity); });
		}

		/// <summary>
		/// Show the view on the screen.
		/// </summary>
		/// <param name="rect">Rect representing position on the screen.</param>
		/// <param name="onMapReady">Optional callback executed when maps is ready.</param>
		public void Show(Rect rect, Action onMapReady = null)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}
#if UNITY_ANDROID
			JniToolkitUtils.RunOnUiThread(() => _ajo.Call("show",
				(int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height,
				_options.AJO, new OnMapReadyCallbackProxy(onMapReady)));
#endif
		}

		/// <summary>
		/// Dismisses this view.
		/// </summary>
		public void Dismiss()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			if (_wasDismissed)
			{
				Debug.LogError("Calling Dismiss() on GoogleMapsView twice. This view was alredy dismissed");
				return;
			}

			_ajo.Call("dismiss");
			_options.Dispose();
			_ajo.Dispose();
			_wasDismissed = true;
		}

		/// <summary>
		/// Sets padding on the map.
		/// This method allows you to define a visible region on the map, to signal to the map that portions of the map around the edges may be obscured, by setting padding on each of the four edges of the map. Map functions will be adapted to the padding. For example, the zoom controls, compass, copyright notices and Google logo will be moved to fit inside the defined region, camera movements will be relative to the center of the visible region, etc.
		/// </summary>
		/// <param name="left">The number of pixels of padding to be added on the left of the map.</param>
		/// <param name="top">The number of pixels of padding to be added on the top of the map.</param>
		/// <param name="right">The number of pixels of padding to be added on the right of the map.</param>
		/// <param name="bottom">The number of pixels of padding to be added on the bottom of the map.</param>
		void SetPadding(int left, int top, int right, int bottom)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			CheckIfDismissed();
			GoogleMapAJO.MainThreadCall("setPadding", left, top, right, bottom);
		}

		#region draw_on_map

		/// <summary>
		/// Add a circle to this map.
		/// </summary>
		/// <returns>The circle added.</returns>
		/// <param name="circleOptions">Circle options.</param>
		public Circle AddCircle(CircleOptions circleOptions)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return new Circle();
			}

			CheckIfDismissed();
			var circleAJO = _ajo.MainThreadCallAJO("addCircle", circleOptions.ToAJO());
			return new Circle(circleAJO);
		}

		/// <summary>
		/// Adds the marker to this map.
		/// </summary>
		/// <returns>The marker added.</returns>
		/// <param name="markerOptions">Marker options.</param>
		public Marker AddMarker(MarkerOptions markerOptions)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return new Marker();
			}

			CheckIfDismissed();
			var markerAJO = _ajo.MainThreadCallAJO("addMarker", markerOptions.ToAJO());
			return new Marker(markerAJO);
		}


		/// <summary>
		/// Adds the ground overlay to this map.
		/// </summary>
		/// <returns>The ground overlay added.</returns>
		/// <param name="overlayOptions">Ground overlay options.</param>
		public GroundOverlay AddGroundOverlay(GroundOverlayOptions overlayOptions)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return new GroundOverlay();
			}

			CheckIfDismissed();
			var groundOverlayAJO = _ajo.MainThreadCallAJO("addGroundOverlay", overlayOptions.ToAJO());
			return new GroundOverlay(groundOverlayAJO);
		}
		
		/// <summary>
		/// Adds the polyline to this map.
		/// </summary>
		/// <param name="polylineOptions">Polyline options.</param>
		/// <returns>Polyline options.</returns>
		public Polyline AddPolyline(PolylineOptions polylineOptions)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return new Polyline();
			}

			CheckIfDismissed();
			var polylineAJO = _ajo.MainThreadCallAJO("addPolyline", polylineOptions.ToAJO());
			return new Polyline(polylineAJO);
		}

		/// <summary>
		/// Adds the polygon to this map.
		/// </summary>
		/// <param name="polygonOptions">Polygon options.</param>
		/// <returns>Polygon options.</returns>
		public Polygon AddPolygon(PolygonOptions polygonOptions)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return new Polygon();
			}
			
			CheckIfDismissed();
			var polygonAJO = _ajo.MainThreadCallAJO("addPolygon", polygonOptions.ToAJO());
			return new Polygon(polygonAJO);
		}

		/// <summary>
		/// Removes all markers, polylines, polygons, overlays, etc from the map.
		/// </summary>
		public void Clear()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			CheckIfDismissed();
			_ajo.MainThreadCall("clear");
		}

		#endregion

		/// <summary>
		/// Animates the movement of the camera from the current position to the position defined in the update.
		/// See <see cref="CameraUpdate"/> for a set of updates.
		/// </summary>
		/// <param name="cameraUpdate">See <see cref="CameraUpdate"/> for a set of updates.</param>
		public void AnimateCamera(CameraUpdate cameraUpdate)
		{
			if (cameraUpdate == null)
			{
				throw new ArgumentNullException("cameraUpdate");
			}

			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			CheckIfDismissed();
			GoogleMapAJO.MainThreadCall("animateCamera", cameraUpdate.ToAJO());
		}

		#region listeners

		/// <summary>
		/// Sets a callback that's invoked when a circle is clicked.
		/// </summary>
		/// <param name="listener">The callback that's invoked when a circle is clicked. To unset the callback, use null.</param>
		public void SetOnCircleClickListener(Action<Circle> listener)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			CheckIfDismissed();
#if UNITY_ANDROID
			GoogleMapAJO.MainThreadCall("setOnCircleClickListener",
				listener == null ? null : new OnCircleClickListenerProxy(listener));
#endif
		}

		/// <summary>
		/// Sets a callback that's invoked when a marker is clicked or tapped.
		/// </summary>
		/// <param name="listener">The callback that's invoked when a marker is clicked or tapped. To unset the callback, use null.</param>
		/// <param name="defaultClickBehaviour">
		/// true if the listener has consumed the event (i.e., the default behavior should not occur); false otherwise (i.e., the default behavior should occur). 
		/// The default behavior is for the camera to move to the marker and an info window to appear.
		/// </param>
		public void SetOnMarkerClickListener(Action<Marker> listener, bool defaultClickBehaviour = true)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			CheckIfDismissed();

#if UNITY_ANDROID
			GoogleMapAJO.MainThreadCall("setOnMarkerClickListener",
				listener == null ? null : new OnMarkerClickListenerProxy(listener, defaultClickBehaviour));
#endif
		}

		/// <summary>
		/// Sets a callback that's invoked when the map is tapped.
		/// </summary>
		/// <param name="onMapClicked">The callback that's invoked when the map is tapped. To unset the callback, use null.</param>
		public void SetOnMapClickListener(Action<LatLng> onMapClicked)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			CheckIfDismissed();
#if UNITY_ANDROID
			GoogleMapAJO.MainThreadCall("setOnMapClickListener",
				onMapClicked == null ? null : new OnMapClickListenerProxy(onMapClicked));
#endif
		}

		/// <summary>
		/// Sets a callback that's invoked when the map is long tapped.
		/// </summary>
		/// <param name="onMapLongClicked">The callback that's invoked when the map is long tapped. To unset the callback, use null.</param>
		public void SetOnLongMapClickListener(Action<LatLng> onMapLongClicked)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			CheckIfDismissed();
#if UNITY_ANDROID
			GoogleMapAJO.MainThreadCall("setOnMapLongClickListener",
				onMapLongClicked == null ? null : new OnMapLongClickListenerProxy(onMapLongClicked));
#endif
		}

		#endregion

		T GetValue<T>(string methodName)
		{
			CheckIfDismissed();
			return GoogleMapUtils.IsAndroidRuntime ? GoogleMapAJO.MainThreadCall<T>(methodName) : default(T);
		}

		void SetValue(string methodName, params object[] args)
		{
			CheckIfDismissed();
			if (GoogleMapUtils.IsAndroidRuntime)
			{
				GoogleMapAJO.MainThreadCallNonBlocking(methodName, args);
			}
		}

		void CheckIfDismissed()
		{
			if (_wasDismissed)
			{
				throw new Exception(
					"Current GoogleMapsView object was already dismissed. You can no longer perform any actions on it. Errors ahead.");
			}
		}

		public override string ToString()
		{
			return string.Format("[GoogleMapsView: UiSettings={0}, IsMyLocationEnabled={1}]", UiSettings, IsMyLocationEnabled);
		}
	}
}