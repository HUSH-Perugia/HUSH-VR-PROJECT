#if UNITY_ANDROID
namespace DeadMosquito.GoogleMapsView.Internal
{
	using System;
	using UnityEngine;

	public sealed class OnCircleClickListenerProxy : AndroidJavaProxy
	{
		readonly Action<Circle> _onCircleClick;

		public OnCircleClickListenerProxy(Action<Circle> onCircleClick)
			: base("com.google.android.gms.maps.GoogleMap$OnCircleClickListener")
		{
			_onCircleClick = onCircleClick;
		}

		public void onCircleClick(AndroidJavaObject circleAJO)
		{
			GoogleMapsSceneHelper.Queue(() => _onCircleClick(new Circle(circleAJO)));
		}
	}
}
#endif