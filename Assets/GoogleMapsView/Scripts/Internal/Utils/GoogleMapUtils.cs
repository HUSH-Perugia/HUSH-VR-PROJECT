namespace DeadMosquito.GoogleMapsView.Internal
{
	using UnityEngine;

	public static class GoogleMapUtils
	{
		public static bool IsAndroidRuntime
		{
			get { return Application.platform == RuntimePlatform.Android; }
		}

		public static bool IsNotAndroidRuntime
		{
			get { return !IsAndroidRuntime; }
		}
	}
}