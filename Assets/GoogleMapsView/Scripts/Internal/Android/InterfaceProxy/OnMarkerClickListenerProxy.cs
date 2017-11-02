#if UNITY_ANDROID
namespace DeadMosquito.GoogleMapsView.Internal
{
	using System;
	using UnityEngine;

	public sealed class OnMarkerClickListenerProxy : AndroidJavaProxy
	{
		readonly Action<Marker> _onMarkerClick;
		readonly bool _defaultClickBehaviour;

		public OnMarkerClickListenerProxy(Action<Marker> onMarkerClick, bool defaultClickBehaviour = true)
			: base("com.google.android.gms.maps.GoogleMap$OnMarkerClickListener")
		{
			_defaultClickBehaviour = defaultClickBehaviour;
			_onMarkerClick = onMarkerClick;
		}

		public bool onMarkerClick(AndroidJavaObject circleAJO)
		{
			GoogleMapsSceneHelper.Queue(() => _onMarkerClick(new Marker(circleAJO)));
			return _defaultClickBehaviour;
		}
	}
}
#endif