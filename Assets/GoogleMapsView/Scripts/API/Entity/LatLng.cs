namespace DeadMosquito.GoogleMapsView
{
	using System;
	using System.Text;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// https://developers.google.com/android/reference/com/google/android/gms/maps/model/LatLng
	/// 
	/// An immutable class representing a pair of latitude and longitude coordinates, stored as degrees.
	/// </summary>
	public sealed class LatLng
	{
		public static LatLng Zero = new LatLng(0, 0);

		readonly double _latitude;
		readonly double _longitude;

		public double Latitude
		{
			get { return _latitude; }
		}

		public double Longitude
		{
			get { return _longitude; }
		}

		/// <summary>
		/// Constructs a LatLng with the given latitude and longitude, measured in degrees.
		/// </summary>
		/// <param name="latitude">Latitude.</param>
		/// <param name="longitude">Longitude.</param>
		public LatLng(double latitude, double longitude)
		{
			_latitude = Math.Max(-90.0D, Math.Min(90.0D, latitude));

			if (-180.0D <= longitude && longitude < 180.0D)
			{
				_longitude = longitude;
			}
			else
			{
				_longitude = ((longitude - 180.0D) % 360.0D + 360.0D) % 360.0D - 180.0D;
			}
		}

		public override string ToString()
		{
			return new StringBuilder(60).Append("lat/lng: (").Append(_latitude).Append(",").Append(_longitude).Append(")")
				.ToString();
		}

		public static LatLng FromAJO(AndroidJavaObject ajo)
		{
			return GoogleMapUtils.IsAndroidRuntime
				? new LatLng(ajo.GetDouble("latitude"), ajo.GetDouble("longitude"))
				: new LatLng(0, 0);
		}

		public AndroidJavaObject ToAJO()
		{
			return GoogleMapUtils.IsAndroidRuntime
				? new AndroidJavaObject("com.google.android.gms.maps.model.LatLng", _latitude, _longitude)
				: null;
		}
	}
}