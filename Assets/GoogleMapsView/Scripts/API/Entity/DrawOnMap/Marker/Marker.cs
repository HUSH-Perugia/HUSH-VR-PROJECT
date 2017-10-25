namespace DeadMosquito.GoogleMapsView
{
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// An icon placed at a particular point on the map's surface. A marker icon is drawn oriented against the device's screen rather than the map's surface; i.e., it will not necessarily change orientation due to map rotations, tilting, or zooming.
	/// 
	/// For more details visit: https://developers.google.com/android/reference/com/google/android/gms/maps/model/Marker
	/// </summary>
	public sealed class Marker
	{
		bool _wasRemoved;

		public Marker()
		{
		}

		readonly AndroidJavaObject _ajo;

		public Marker(AndroidJavaObject ajo)
		{
			_ajo = ajo;
		}

		/// <summary>
		/// Gets this marker's id. The id will be unique amongst all Markers on a map.
		/// </summary>
		/// <value>The marker identifier.</value>
		public string Id
		{
			get { return GetValue<string>("getId"); }
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
		/// Gets or sets the opacity of the marker. Defaults to 1.0.
		/// </summary>
		/// <value>The opacity of the marker.</value>
		public float Alpha
		{
			get { return GetValue<float>("getAlpha"); }
			set { SetValue("setAlpha", value); }
		}

		/// <summary>
		/// See https://developers.google.com/android/reference/com/google/android/gms/maps/model/Marker.html setAnchor method for more detailed description
		/// 
		/// Sets the anchor point for the marker. The anchor specifies the point in the icon image that is anchored to the marker's position on the Earth's surface.
		/// </summary>
		/// <param name="anchorU">u-coordinate of the anchor, as a ratio of the image width (in the range [0, 1]).</param>
		/// <param name="anchorV">v-coordinate of the anchor, as a ratio of the image height (in the range [0, 1]).</param>
		public void SetAnchor(float anchorU, float anchorV)
		{
			SetValue("setAnchor", anchorU, anchorV);
		}

		/// <summary>
		/// Specifies the point in the marker image at which to anchor the info window when it is displayed.
		/// </summary>
		/// <param name="anchorU">u-coordinate of the info window anchor, as a ratio of the image width (in the range [0, 1]).</param>
		/// <param name="anchorV">v-coordinate of the info window anchor, as a ratio of the image height (in the range [0, 1]).</param>
		public void SetInfoWindowAnchor(float anchorU, float anchorV)
		{
			SetValue("setInfoWindowAnchor", anchorU, anchorV);
		}

		/// <summary>
		/// A text string that's displayed in an info window when the user taps the marker. You can change this value at any time.
		/// </summary>
		/// <returns>The title of the marker</returns>
		public string Title
		{
			get { return GetValue<string>("getTitle"); }
			set { SetValue("setTitle", value); }
		}

		/// <summary>
		/// Additional text that's displayed below the title. You can change this value at any time.
		/// </summary>
		/// <returns>Marker snippet text</returns>
		public string Snippet
		{
			get { return GetValue<string>("getSnippet"); }
			set { SetValue("setSnippet", value); }
		}

		/// <summary>
		/// Sets the icon for the marker.
		/// </summary>
		/// <param name="imageDescriptor">Image descriptor. If null, the default marker is used.</param>
		public void SetIcon(ImageDescriptor imageDescriptor)
		{
			SetValue("setIcon", imageDescriptor.ToAJO());
		}

		public bool DragStatus { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this marker is visible.
		/// If this marker is not visible then it will not be drawn. All other state is preserved. Markers are visible by default.
		/// </summary>
		/// <value><c>true</c> if this marker is visible; otherwise, <c>false</c>.</value>
		public bool IsVisible
		{
			get { return GetValue<bool>("isVisible"); }
			set { SetValue("setVisible", value); }
		}


		/// <summary>
		/// Gets or sets the flat setting of the Marker.
		/// </summary>
		/// <returns><code>true</code> the marker is flat against the map; <code>false</code> if the marker should face the camera.</returns>
		public bool Flat
		{
			get { return GetValue<bool>("isFlat"); }
			set { SetValue("setFlat", value); }
		}

		/// <summary>
		/// Gets or sets the rotation of the marker.
		/// </summary>
		/// <returns>The rotation of the marker in degrees clockwise from the default position.</returns>
		public float Rotation
		{
			get { return GetValue<float>("getRotation"); }
			set { SetValue("setRotation", value); }
		}

		/// <summary>
		/// Gets or sets the index of the Z. Overlays (such as markers) with higher zIndices are drawn above those with lower indices.
		/// </summary>
		/// <value>The index of the Z.</value>
		public float ZIndex
		{
			get { return GetValue<float>("getZIndex"); }
			set { SetValue("setZIndex", value); }
		}

		/// <summary>
		/// Removes this marker from the map.
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
				Debug.LogError("This marker was already removed from the map. You can't perform any more operations on it.");
			}
		}

		/// <summary>
		/// Shows the info window of this marker on the map, if this marker <see cref="IsVisible"/>.
		/// </summary>
		public void ShowInfoWindow()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return;
			}
			
			CheckIfRemoved();
			
			_ajo.MainThreadCall("showInfoWindow");
		}

		public override string ToString()
		{
			return string.Format(
				"[Marker: Id={0}, Position={1}, Alpha={2}, Title={3}, Snippet={4}, DragStatus={5}, IsVisible={6}, Flat={7}, Rotation={8}, ZIndex={9}]",
				Id, Position, Alpha, Title, Snippet, DragStatus, IsVisible, Flat, Rotation, ZIndex);
		}
	}
}