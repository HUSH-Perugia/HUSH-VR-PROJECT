namespace DeadMosquito.GoogleMapsView
{
	using Internal;
	using UnityEngine;

	public sealed class CameraPosition
	{
		const string CameraPositionClass = "com.google.android.gms.maps.model.CameraPosition";

		readonly LatLng _latLng;

		readonly float _zoom;
		readonly float _tilt;
		readonly float _bearing;

		public LatLng LatitudeLongitude
		{
			get { return _latLng; }
		}

		public float Zoom
		{
			get { return _zoom; }
		}

		public float Tilt
		{
			get { return _tilt; }
		}

		public float Bearing
		{
			get { return _bearing; }
		}

		/// <summary>
		/// An immutable class that aggregates all camera position parameters such as location, zoom level, tilt angle, and bearing.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="zoom">Zoom.</param>
		/// <param name="tilt">Tilt.</param>
		/// <param name="bearing">Bearing.</param>
		public CameraPosition(LatLng target, float zoom, float tilt, float bearing)
		{
			_bearing = bearing;
			_tilt = tilt;
			_zoom = zoom;
			_latLng = target;
		}

		public AndroidJavaObject ToAJO()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return null;
			}

			return new AndroidJavaObject(CameraPositionClass, _latLng.ToAJO(), _zoom, _tilt, _bearing);
		}
	}
}