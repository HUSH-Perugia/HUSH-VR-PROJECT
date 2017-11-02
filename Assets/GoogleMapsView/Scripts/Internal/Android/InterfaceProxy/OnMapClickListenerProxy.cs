#if UNITY_ANDROID
namespace DeadMosquito.GoogleMapsView.Internal
{
	using System;
	using UnityEngine;

	public sealed class OnMapClickListenerProxy : AndroidJavaProxy
	{
		readonly Action<LatLng> _onMapClick;

		public OnMapClickListenerProxy(Action<LatLng> onMapClick)
			: base("com.google.android.gms.maps.GoogleMap$OnMapClickListener")
		{
			_onMapClick = onMapClick;
		}

		public void onMapClick(AndroidJavaObject pointAJO)
		{
			GoogleMapsSceneHelper.Queue(() => _onMapClick(LatLng.FromAJO(pointAJO)));
		}
	}
}
#endif