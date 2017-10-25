#if UNITY_ANDROID
namespace DeadMosquito.GoogleMapsView.Internal
{
	using System;
	using UnityEngine;

	public sealed class OnMapReadyCallbackProxy : AndroidJavaProxy
	{
		readonly Action _onMapReady;

		public OnMapReadyCallbackProxy(Action onMapReady)
			: base("com.google.android.gms.maps.OnMapReadyCallback")
		{
			_onMapReady = onMapReady;
		}

		public void onMapReady(AndroidJavaObject map)
		{
			GoogleMapsSceneHelper.Queue(_onMapReady);
		}
	}
}
#endif