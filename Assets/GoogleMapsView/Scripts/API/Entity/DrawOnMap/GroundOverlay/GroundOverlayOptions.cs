namespace DeadMosquito.GoogleMapsView
{
	using System;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// Defines options for a ground overlay.
	/// 
	/// For detailed documentation visit: https://developers.google.com/android/reference/com/google/android/gms/maps/model/GroundOverlayOptions
	/// </summary>
	public sealed class GroundOverlayOptions
	{
		const string GroundOverlayOptionsClass = "com.google.android.gms.maps.model.GroundOverlayOptions";

		#region position

		float _width;
		float _height;
		LatLng _latLng;
		LatLngBounds _latLngBounds;
		float _bearing;

		#endregion

		float _anchorU = 0.5f;
		float _anchorV = 0.5f;
		ImageDescriptor _imageDescriptor;
		bool _clickable = true;
		bool _visible = true;

		float _transparency = 0.0F;
		float _zIndex;

		SetPositionMethod _setPositionMethod = SetPositionMethod.NotSet;

		enum SetPositionMethod
		{
			NotSet,
			LatLngWidthOnly,
			LatLngWidthHeight,
			LatLngBounds
		}

		/// <summary>
		/// Specifies the anchor to be at a particular point in the ground overlay image.
		/// The anchor specifies the point in the icon image that is anchored to the ground overlay's position on the Earth's surface.
		/// </summary>
		/// <param name="u">u-coordinate of the anchor, as a ratio of the image width (in the range [0, 1])</param>
		/// <param name="v">v-coordinate of the anchor, as a ratio of the image height (in the range [0, 1])</param>
		public GroundOverlayOptions Anchor(float u, float v)
		{
			_anchorU = Mathf.Clamp01(u);
			_anchorV = Mathf.Clamp01(v);
			return this;
		}

		/// <summary>
		/// Specifies the bearing of the ground overlay in degrees clockwise from north. The rotation is performed about the anchor point. If not specified, the default is 0 (i.e., up on the image points north).
		///
		// Note that latitude-longitude bound applies before the rotation.
		/// </summary>
		/// <param name="bearing">the bearing in degrees clockwise from north. Values outside the range [0, 360) will be normalized.</param>
		public GroundOverlayOptions Bearing(float bearing)
		{
			_bearing = Mathf.Clamp(bearing, 0f, 360f);
			return this;
		}

		/// <summary>
		/// Sets the icon for the ground overlay.
		/// </summary>
		/// <param name="imageDescriptor">Image descriptor. If null, the default ground overlay is used.</param>
		public GroundOverlayOptions Image(ImageDescriptor imageDescriptor)
		{
			_imageDescriptor = imageDescriptor;
			return this;
		}

		/// <summary>
		/// Specifies whether this ground overlay is clickable. The default setting is <code>false<code>/.
		/// </summary>
		/// <param name="clickable">Whether the ground overlay is cickable.</param>
		public GroundOverlayOptions Clickable(bool clickable)
		{
			_clickable = clickable;
			return this;
		}

		/// <summary>
		/// Sets the visibility for the ground overlay.
		/// </summary>
		/// <param name="visible">If set to <c>true</c> visible.</param>
		public GroundOverlayOptions Visible(bool visible)
		{
			_visible = visible;
			return this;
		}

		/// <summary>
		/// Specifies the position for this ground overlay using an anchor point (a <see cref="LatLng"/>) and the width (in meters). 
		/// The height will be adapted accordingly to preserve aspect ratio.
		/// </summary>
		/// <param name="latLng">the location on the map <see cref="LatLng"/> to which the anchor point in the given image will remain fixed. The anchor will remain fixed to the position on the ground when transformations are applied (e.g., setDimensions, setBearing, etc.).</param>
		/// <param name="width">the width of the overlay (in meters). The height will be determined automatically based on the image aspect ratio.</param>
		public GroundOverlayOptions Position(LatLng latLng, float width)
		{
			if (latLng == null)
			{
				throw new ArgumentNullException("latLng");
			}

			_setPositionMethod = SetPositionMethod.LatLngWidthOnly;

			_latLng = latLng;
			_width = width;
			return this;
		}

		/// <summary>
		/// Specifies the position for this ground overlay using an anchor point (a <see cref="LatLng"/>), width and height (both in meters). When rendered, the image will be scaled to fit the dimensions specified.
		/// </summary>
		/// <param name="latLng">the location on the map <see cref="LatLng"/> to which the anchor point in the given image will remain fixed. The anchor will remain fixed to the position on the ground when transformations are applied (e.g., setDimensions, setBearing, etc.).</param>
		/// <param name="width">the width of the overlay (in meters)</param>
		/// <param name="height">the height of the overlay (in meters)</param>
		public GroundOverlayOptions Position(LatLng latLng, float width, float height)
		{
			if (latLng == null)
			{
				throw new ArgumentNullException("latLng");
			}

			_setPositionMethod = SetPositionMethod.LatLngWidthHeight;

			_latLng = latLng;
			_width = width;
			_height = height;
			return this;
		}

		/// <summary>
		/// Specifies the position for this ground overlay.
		/// </summary>
		/// <param name="latLngBounds">a <see cref="LatLngBounds"/> in which to place the ground overlay</param>
		public GroundOverlayOptions PositionFromBounds(LatLngBounds latLngBounds)
		{
			if (latLngBounds == null)
			{
				throw new ArgumentNullException("latLngBounds");
			}

			_setPositionMethod = SetPositionMethod.LatLngBounds;

			_latLngBounds = latLngBounds;
			return this;
		}

		/// <summary>
		/// Specifies the transparency of the ground overlay. The default transparency is 0 (opaque).
		/// </summary>
		/// <param name="transparency">A float in the range [0..1] where 0 means that the ground overlay is opaque and 1 means that the ground overlay is transparent.</param>
		public GroundOverlayOptions Transparency(float transparency)
		{
			_transparency = Mathf.Clamp01(transparency);
			return this;
		}

		/// <summary>
		/// Sets the zIndex for the ground overlay.
		/// </summary>
		/// <param name="zIndex">Sets the zIndex for the ground overlay.</param>
		public GroundOverlayOptions ZIndex(float zIndex)
		{
			_zIndex = zIndex;
			return this;
		}

		public AndroidJavaObject ToAJO()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return null;
			}

			var ajo = new AndroidJavaObject(GroundOverlayOptionsClass);
			SetPosition(ajo);
			ajo.CallAJO("zIndex", _zIndex);
			ajo.CallAJO("visible", _visible);
			ajo.CallAJO("clickable", _clickable);
			ajo.CallAJO("transparency", _transparency);
			ajo.CallAJO("anchor", _anchorU, _anchorV);
			ajo.CallAJO("bearing", _bearing);

			if (_imageDescriptor != null)
			{
				ajo.CallAJO("image", _imageDescriptor.ToAJO());
			}

			return ajo;
		}

		void SetPosition(AndroidJavaObject ajo)
		{
			switch (_setPositionMethod)
			{
				case SetPositionMethod.LatLngWidthOnly:
					ajo.CallAJO("position", _latLng.ToAJO(), _width);
					break;
				case SetPositionMethod.LatLngWidthHeight:
					ajo.CallAJO("position", _latLng.ToAJO(), _width, _height);
					break;
				case SetPositionMethod.LatLngBounds:
					ajo.CallAJO("positionFromBounds", _latLngBounds.ToAJO());
					break;
				case SetPositionMethod.NotSet:
					break;
				default:
					Debug.LogError("GroundOverlayOptions must have position set");
					break;
			}
		}
	}
}