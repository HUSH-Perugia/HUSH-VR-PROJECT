namespace DeadMosquito.GoogleMapsView
{
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// An immutable class representing a latitude/longitude aligned rectangle.
	/// </summary>
	public sealed class LatLngBounds
	{
		readonly LatLng _southwest;
		readonly LatLng _northeast;

		/// <summary>
		/// Creates a new bounds based on a southwest and a northeast corner.
		/// </summary>
		/// <param name="southwest">Southwest corner.</param>
		/// <param name="northeast">Northeast corner.</param>
		public LatLngBounds(LatLng southwest, LatLng northeast)
		{
			_southwest = southwest;
			_northeast = northeast;
		}

		public AndroidJavaObject ToAJO()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return null;
			}

			return new AndroidJavaObject("com.google.android.gms.maps.model.LatLngBounds", _southwest.ToAJO(),
				_northeast.ToAJO());
		}

		public static LatLngBounds FromAJO(AndroidJavaObject ajo)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return new LatLngBounds(LatLng.Zero, LatLng.Zero);
			}

			var northeast = LatLng.FromAJO(ajo.GetAJO("northeast"));
			var southwest = LatLng.FromAJO(ajo.GetAJO("southwest"));
			return new LatLngBounds(southwest, northeast);
		}

		public override string ToString()
		{
			return string.Format("[LatLngBounds SW: {0}, NE: {1}]", _southwest, _northeast);
		}
	}
}