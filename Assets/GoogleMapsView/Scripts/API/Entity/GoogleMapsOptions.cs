namespace DeadMosquito.GoogleMapsView
{
	using System;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// Defines configuration GoogleMapOptions for a GoogleMap. 
	/// 
	/// See https://developers.google.com/android/reference/com/google/android/gms/maps/GoogleMapOptions.html for reference.
	/// </summary>
	public class GoogleMapsOptions : IDisposable
	{
		const string GoogleMapsOptionsJavaClass = "com.google.android.gms.maps.GoogleMapOptions";

		readonly AndroidJavaObject _ajo;

		public AndroidJavaObject AJO
		{
			get { return _ajo; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeadMosquito.GoogleMapsView.GoogleMapsOptions"/> class.
		/// </summary>
		public GoogleMapsOptions()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			_ajo = new AndroidJavaObject(GoogleMapsOptionsJavaClass);
		}

		/// <summary>
		/// Specifies whether ambient-mode styling should be enabled.
		/// </summary>
		/// <param name="enabled">Whether ambient-mode styling should be enabled.</param>
		public GoogleMapsOptions AmbientEnabled(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("ambientEnabled", enabled);
			return this;
		}

		/// <summary>
		/// Specifies a the initial camera position for the map.
		/// </summary>
		/// <param name="camera">The initial camera position for the map.</param>
		public GoogleMapsOptions Camera(CameraPosition camera)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("camera", camera.ToAJO());
			return this;
		}

		/// <summary>
		/// Specifies whether the compass should be enabled. The default value is true.
		/// </summary>
		/// <param name="enabled">Whether the compass should be enabled. The default value is true.</param>
		public GoogleMapsOptions CompassEnabled(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("compassEnabled", enabled);
			return this;
		}

		/// <summary>
		/// Specifies LatLngBounds used to constrain the camera target.
		/// </summary>
		/// <param name="llbounds">LatLngBounds used to constrain the camera target.</param>
		public GoogleMapsOptions LatLngBoundsForCameraTarget(LatLngBounds llbounds)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("latLngBoundsForCameraTarget", llbounds.ToAJO());
			return this;
		}

		/// <summary>
		/// Specifies if lite mode enabled
		/// </summary>
		/// <param name="enabled">If set to <c>true</c> lite mode will be enabled.</param>
		public GoogleMapsOptions LiteMode(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("liteMode", enabled);
			return this;
		}

		/// <summary>
		/// Specifies if map toolbar enabled.
		/// </summary>
		/// <returns>The toolbar enabled.</returns>
		/// <param name="enabled">If map toolbar enabled.</param>
		public GoogleMapsOptions MapToolbarEnabled(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("mapToolbarEnabled", enabled);
			return this;
		}

		/// <summary>
		/// Specifies the map type.
		/// </summary>
		/// <param name="mapType">Map type.</param>
		public GoogleMapsOptions MapType(GoogleMapType mapType)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("mapType", (int) mapType);
			return this;
		}

		/// <summary>
		/// the maximum zoom level preference.
		/// </summary>
		/// <param name="maxZoomPreference">The maximum zoom level preference.</param>
		public GoogleMapsOptions MaxZoomPreference(float maxZoomPreference)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("maxZoomPreference", maxZoomPreference);
			return this;
		}

		/// <summary>
		/// the minimum zoom level preference.
		/// </summary>
		/// <param name="minZoomPreference">The minimum zoom level preference.</param>
		public GoogleMapsOptions MinZoomPreference(float minZoomPreference)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("minZoomPreference", minZoomPreference);
			return this;
		}

		/// <summary>
		/// Specifies if the rotate gestures are enabled.
		/// </summary>
		/// <param name="enabled">If the rotate gestures are enabled.</param>
		public GoogleMapsOptions RotateGesturesEnabled(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("rotateGesturesEnabled", enabled);
			return this;
		}

		/// <summary>
		/// Specifies if the scroll gestures are enabled.
		/// </summary>
		/// <param name="enabled">If the scroll gestures are enabled.</param>
		public GoogleMapsOptions ScrollGesturesEnabled(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("scrollGesturesEnabled", enabled);
			return this;
		}

		/// <summary>
		/// Specifies if the tilt gestures are enabled.
		/// </summary>
		/// <param name="enabled">If the tilt gestures are enabled.</param>
		public GoogleMapsOptions TiltGesturesEnabled(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("tiltGesturesEnabled", enabled);
			return this;
		}

		/// <summary>
		/// Specifies if the zoom controls are enabled.
		/// </summary>
		/// <param name="enabled">If the zoom controls are enabled.</param>
		public GoogleMapsOptions ZoomControlsEnabled(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("zoomControlsEnabled", enabled);
			return this;
		}

		/// <summary>
		/// Specifies if the zoom gestures are enabled.
		/// </summary>
		/// <param name="enabled">If the zoom gestures are enabled.</param>
		public GoogleMapsOptions ZoomGesturesEnabled(bool enabled)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return this;
			}

			_ajo.CallAJO("zoomGesturesEnabled", enabled);
			return this;
		}

		public override string ToString()
		{
			return string.Format("[GoogleMapsOptions]");
		}

		#region IDisposable implementation

		public void Dispose()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}

			_ajo.Dispose();
		}

		#endregion
	}
}