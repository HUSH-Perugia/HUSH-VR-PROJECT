namespace DeadMosquito.GoogleMapsView
{
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// A circle on the earth's surface (spherical cap).
	/// 
	/// For more details visit: https://developers.google.com/android/reference/com/google/android/gms/maps/model/Circle
	/// </summary>
	public sealed class Circle
	{
		bool _wasRemoved;

		public Circle()
		{
		}

		readonly AndroidJavaObject _ajo;

		public Circle(AndroidJavaObject ajo)
		{
			_ajo = ajo;
		}

		/// <summary>
		/// Gets this circle's id. The id will be unique amongst all Circles on a map.
		/// </summary>
		/// <value>The circle identifier.</value>
		public string Id
		{
			get { return GetValue<string>("getId"); }
		}

		/// <summary>
		/// Returns the center as a <see cref="LatLng"/>.
		/// </summary>
		/// <value>The geographic center as a <see cref="LatLng"/></value>
		public LatLng Center
		{
			get
			{
				var ajo = GetValue<AndroidJavaObject>("getCenter");
				return LatLng.FromAJO(ajo);
			}
			set { SetValue("setCenter", value.ToAJO()); }
		}

		/// <summary>
		/// Gets or sets the color of the fill.
		/// </summary>
		/// <value>The color of the fill.</value>
		public Color FillColor
		{
			get { return GetValue<int>("getFillColor").ToUnityColor(); }
			set { SetValue("setFillColor", value.ToAndroidColor()); }
		}

		/// <summary>
		/// Gets or sets the color of the stroke.
		/// </summary>
		/// <value>The color of the stroke.</value>
		public Color StrokeColor
		{
			get { return GetValue<int>("getStrokeColor").ToUnityColor(); }
			set { SetValue("setStrokeColor", value.ToAndroidColor()); }
		}

		/// <summary>
		/// Gets or sets the width of the stroke.
		/// </summary>
		/// <value>The width of the stroke.</value>
		public float StrokeWidth
		{
			get { return GetValue<float>("getStrokeWidth"); }
			set { SetValue("setStrokeWidth", value); }
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
		/// Gets or sets a value indicating whether this circle is visible.
		/// If this circle is not visible then it will not be drawn. All other state is preserved. Circles are visible by default.
		/// </summary>
		/// <value><c>true</c> if this circle is visible; otherwise, <c>false</c>.</value>
		public bool IsVisible
		{
			get { return GetValue<bool>("isVisible"); }
			set { SetValue("setVisible", value); }
		}

		/// <summary>
		/// Gets/sets the clickability of the circle.
		/// </summary>
		public bool IsClickable
		{
			get { return GetValue<bool>("isClickable"); }
			set { SetValue("setClickable", value); }
		}

		/// <summary>
		/// Gets or sets the radius of the circle.
		/// </summary>
		/// <value>The radius of the circle.</value>
		public double Radius
		{
			get { return GetValue<double>("getRadius"); }
			set { SetValue("setRadius", value); }
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
				Debug.LogError("This circle was already removed from the map. You can't perform any more operations on it.");
			}
		}

		public override string ToString()
		{
			return string.Format(
				"[Circle: Id={0}, Center={1}, FillColor={2}, StrokeColor={3}, StrokeWidth={4}, ZIndex={5}, IsVisible={6}, IsClickable={7}, Radius={8}]",
				Id, Center, FillColor, StrokeColor, StrokeWidth, ZIndex, IsVisible, IsClickable, Radius);
		}
	}
}