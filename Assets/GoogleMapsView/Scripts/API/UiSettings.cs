namespace DeadMosquito.GoogleMapsView
{
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// Settings for the user interface of a GoogleMap. To obtain this object, call <see cref="GoogleMapsView.UiSettings"/>.
	/// </summary>
	public sealed class UiSettings
	{
		readonly AndroidJavaObject _mapAjo;

		internal UiSettings()
		{
		}

		internal UiSettings(AndroidJavaObject mapAJO)
		{
			_mapAjo = mapAJO;
		}

		/// <summary>
		/// Gets/sets whether the compass is enabled/disabled.
		/// </summary>
		public bool IsCompassEnabled
		{
			get { return GetBool("isCompassEnabled"); }
			set { SetBool("setCompassEnabled", value); }
		}

		/// <summary>
		/// Gets/sets whether the indoor level picker is enabled/disabled.
		/// </summary>
		public bool IsIndoorLevelPickerEnabled
		{
			get { return GetBool("isIndoorLevelPickerEnabled"); }
			set { SetBool("setIndoorLevelPickerEnabled", value); }
		}

		/// <summary>
		/// Gets/sets whether the Map Toolbar is enabled/disabled.
		/// </summary>
		public bool IsMapToolbarEnabled
		{
			get { return GetBool("isMapToolbarEnabled"); }
			set { SetBool("setMapToolbarEnabled", value); }
		}

		/// <summary>
		/// Gets/sets whether the my-location button is enabled/disabled.
		/// </summary>
		public bool IsMyLocationButtonEnabled
		{
			get { return GetBool("isMyLocationButtonEnabled"); }
			set { SetBool("setMyLocationButtonEnabled", value); }
		}

		/// <summary>
		/// Gets/sets whether rotate gestures are enabled/disabled.
		/// </summary>
		public bool IsRotateGesturesEnabled
		{
			get { return GetBool("isRotateGesturesEnabled"); }
			set { SetBool("setRotateGesturesEnabled", value); }
		}

		/// <summary>
		/// Gets/sets whether scroll gestures are enabled/disabled.
		/// </summary>
		public bool IsScrollGesturesEnabled
		{
			get { return GetBool("isScrollGesturesEnabled"); }
			set { SetBool("setScrollGesturesEnabled", value); }
		}

		/// <summary>
		/// Gets whether tilt gestures are enabled/disabled.
		/// </summary>
		public bool IsTiltGesturesEnabled
		{
			get { return GetBool("isTiltGesturesEnabled"); }
			set { SetBool("setTiltGesturesEnabled", value); }
		}

		/// <summary>
		/// Gets/sets whether the zoom controls are enabled/disabled.
		/// </summary>
		public bool IsZoomControlsEnabled
		{
			get { return GetBool("isZoomControlsEnabled"); }
			set { SetBool("setZoomControlsEnabled", value); }
		}

		/// <summary>
		/// Gets/sets whether zoom gestures are enabled/disabled.
		/// </summary>
		public bool IsZoomGesturesEnabled
		{
			get { return GetBool("isZoomGesturesEnabled"); }
			set { SetBool("setZoomGesturesEnabled", value); }
		}

		/// <summary>
		/// Sets the preference for whether all gestures should be enabled or disabled.
		/// </summary>
		/// <param name="enabled">Whether all gestures should be enabled</param>
		public void SetAllGesturesEnabled(bool enabled)
		{
			SetBool("setAllGesturesEnabled", enabled);
		}

		public override string ToString()
		{
			return string.Format(
				"[UiSettings: IsCompassEnabled={0}, IsIndoorLevelPickerEnabled={1}, IsMapToolbarEnabled={2}, IsMyLocationButtonEnabled={3}, IsRotateGesturesEnabled={4}, IsScrollGesturesEnabled={5}, IsTiltGesturesEnabled={6}, IsZoomControlsEnabled={7}, IsZoomGesturesEnabled={8}]",
				IsCompassEnabled, IsIndoorLevelPickerEnabled, IsMapToolbarEnabled, IsMyLocationButtonEnabled,
				IsRotateGesturesEnabled, IsScrollGesturesEnabled, IsTiltGesturesEnabled, IsZoomControlsEnabled,
				IsZoomGesturesEnabled);
		}

		void SetBool(string methodname, bool value)
		{
			if (GoogleMapUtils.IsAndroidRuntime)
			{
				_mapAjo.MainThreadCall(methodname, value);
			}
		}

		bool GetBool(string methodName)
		{
			return GoogleMapUtils.IsAndroidRuntime && _mapAjo.MainThreadCallBool(methodName);
		}
	}
}