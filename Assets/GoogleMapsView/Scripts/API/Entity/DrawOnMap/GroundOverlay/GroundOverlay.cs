namespace DeadMosquito.GoogleMapsView
{
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// A ground overlay is an image that is fixed to a map. 
	/// 
	/// For more details visit: https://developers.google.com/android/reference/com/google/android/gms/maps/model/GroundOverlay
	/// </summary>
	public sealed class GroundOverlay
	{
		bool _wasRemoved;

		public GroundOverlay()
		{
		}

		readonly AndroidJavaObject _ajo;

		public GroundOverlay(AndroidJavaObject ajo)
		{
			_ajo = ajo;
		}

		/// <summary>
		/// Gets this ground overlay's id. The id will be unique amongst all GroundOverlays on a map.
		/// </summary>
		/// <value>The ground overlay identifier.</value>
		public string Id
		{
			get { return GetValue<string>("getId"); }
		}

		/// <summary>
		/// Gets the height of the ground overlay.
		/// </summary>
		public float Height
		{
			get { return GetValue<float>("getHeight"); }
		}

		/// <summary>
		/// Gets the width of the ground overlay.
		/// </summary>
		public float Width
		{
			get { return GetValue<float>("getWidth"); }
		}

		/// <summary>
		/// Gets/sets the bearing of the ground overlay (the direction that the vertical axis of the ground overlay points) in degrees clockwise from north. 
		/// The rotation is performed about the anchor point.
		/// </summary>
		public float Bearing
		{
			get { return GetValue<float>("getBearing"); }
			set { SetValue("setBearing", value); }
		}

		/// <summary>
		/// Gets the bounds for the ground overlay. This ignores the rotation of the ground overlay.
		/// </summary>
		public LatLngBounds Bounds
		{
			get { return LatLngBounds.FromAJO(GetValue<AndroidJavaObject>("getBounds")); }
		}

		/// <summary>
		/// Returns the position as a <see cref="LatLng"/>.
		/// </summary>
		/// <value>The geographic position as a <see cref="LatLng"/></value>
		public LatLng Position
		{
			get { return LatLng.FromAJO(GetValue<AndroidJavaObject>("getPosition")); }
			set { SetValue("setPosition", value.ToAJO()); }
		}

		/// <summary>
		/// Gets/sets the transparency of this ground overlay.
		/// Transparency of the ground overlay in the range [0..1] where 0 means the overlay is opaque and 1 means the overlay is fully transparent.
		/// </summary>
		/// <value>The transparency of the marker.</value>
		public float Transparency
		{
			get { return GetValue<float>("getTransparency"); }
			set { SetValue("setTransparency", value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this ground overlay is visible.
		/// If this ground overlay is not visible then it will not be drawn. All other state is preserved. Ground overlays are visible by default.
		/// </summary>
		/// <value><c>true</c> if this ground overlay is visible; otherwise, <c>false</c>.</value>
		public bool IsVisible
		{
			get { return GetValue<bool>("isVisible"); }
			set { SetValue("setVisible", value); }
		}

		/// <summary>
		/// Gets/sets the clickability of the ground overlay.
		/// </summary>
		public bool IsClickable
		{
			get { return GetValue<bool>("isClickable"); }
			set { SetValue("setClickable", value); }
		}

		/// <summary>
		/// Gets or sets the index of the Z. Overlays (such as circles) with higher zIndices are drawn above those with lower indices.
		/// </summary>
		/// <value>The index of the Z.</value>
		public float ZIndex
		{
			get { return GetValue<float>("getZIndex"); }
			set { SetValue("setZIndex", value); }
		}

		/// <summary>
		/// Sets the width of the ground overlay. The height of the ground overlay will be adapted accordingly to preserve aspect ratio.
		/// </summary>
		/// <param name="width">width in meters</param>
		public void SetDimensions(float width)
		{
			SetValue("setDimensions", width);
		}

		/// <summary>
		/// Sets the dimensions of the ground overlay. The image will be stretched to fit the dimensions.
		/// </summary>
		/// <param name="width">width in meters</param>
		/// <param name="height">height in meters</param>
		public void SetDimensions(float width, float height)
		{
			SetValue("setDimensions", width, height);
		}

		/// <summary>
		/// Sets the image for the Ground Overlay. The new image will occupy the same bounds as the old image.
		/// </summary>
		/// <param name="imageDescriptor">Image descriptor</param>
		public void SetImage(ImageDescriptor imageDescriptor)
		{
			SetValue("setImage", imageDescriptor.ToAJO());
		}

		/// <summary>
		/// Sets the position of the ground overlay by fitting it to the given <see cref="LatLngBounds"/>. 
		/// This method will ignore the rotation (bearing) of the ground overlay when positioning it, but the bearing will still be used when drawing it.
		/// </summary>
		/// <param name="bounds">a <see cref="LatLngBounds"/> in which to place the ground overlay</param>
		public void SetPositionFromBounds(LatLngBounds bounds)
		{
			SetValue("setPositionFromBounds", bounds.ToAJO());
		}

		/// <summary>
		/// Removes this circle from the map.
		/// </summary>
		public void Remove()
		{
			if (GoogleMapUtils.IsAndroidRuntime)
			{
				_ajo.MainThreadCall("remove");
			}
			_wasRemoved = true;
		}

		T GetValue<T>(string methodName)
		{
			CheckIfRemoved();
			return GoogleMapUtils.IsAndroidRuntime ? _ajo.MainThreadCall<T>(methodName) : default(T);
		}

		void SetValue(string methodName, params object[] args)
		{
			CheckIfRemoved();
			if (GoogleMapUtils.IsAndroidRuntime)
			{
				_ajo.MainThreadCallNonBlocking(methodName, args);
			}
		}

		void CheckIfRemoved()
		{
			if (_wasRemoved)
			{
				Debug.LogError(
					"This circle was already removed from the map. You can't perform any more operations on it.");
			}
		}

		public override string ToString()
		{
			return string.Format(
				"[GroundOverlay: Id={0}, Height={1}, Width={2}, Bearing={3}, Bounds={4}, Position={5}, Transparency={6}, IsVisible={7}, IsClickable={8}, ZIndex={9}]",
				Id, Height, Width, Bearing, Bounds, Position, Transparency, IsVisible, IsClickable, ZIndex);
		}
	}
}