namespace DeadMosquito.GoogleMapsView
{
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// A polyline is a list of points, where line segments are drawn between consecutive points
	/// </summary>
	public sealed class Polyline
	{
		bool _wasRemoved;

		readonly AndroidJavaObject _ajo;

		public Polyline(AndroidJavaObject ajo)
		{
			_ajo = ajo;
		}

		public Polyline()
		{
		}

		/// <summary>
		/// The vertices of the line. Line segments are drawn between consecutive points. A polyline is not closed by default; to form a closed polyline, the start and end points must be the same.
		/// </summary>
		public List<LatLng> Points
		{
			get
			{
				var listAJO = GetValue<AndroidJavaObject>("getPoints");
				return listAJO.FromJavaList<LatLng>(LatLng.FromAJO);
			}
			set
			{
				var listAJO = value.ToJavaList(latLng => latLng.ToAJO());
				SetValue("setPoints", listAJO);
			}
		}

		/// <summary>
		/// Gets/sets the polyline color
		/// </summary>
		public Color Color
		{
			get { return GetValue<int>("getColor").ToUnityColor(); }
			set { SetValue("setColor", value.ToAndroidColor()); }
		}

		/// <summary>
		/// Gets/sets the polyline width
		/// </summary>
		public float Width
		{
			get { return GetValue<float>("getWidth"); }
			set { SetValue("setWidth", value); }
		}

		/// <summary>
		/// Defines the shape to be used at the start of a polyline. Supported cap types: <see cref="ButtCap"/>, <see cref="SquareCap"/>, <see cref="RoundCap"/> (applicable for solid stroke pattern) and <see cref="CustomCap"/> (applicable for any stroke pattern). 
		/// Default for both start and end: <see cref="ButtCap"/>.
		/// </summary>
		public Cap StartCap
		{
			set { SetValue("setStartCap", value.ToAJO()); }
		}

		/// <summary>
		/// Defines the shape to be used at the end of a polyline. Supported cap types: <see cref="ButtCap"/>, <see cref="SquareCap"/>, <see cref="RoundCap"/> (applicable for solid stroke pattern) and <see cref="CustomCap"/> (applicable for any stroke pattern). 
		/// Default for both start and end: <see cref="ButtCap"/>.
		/// </summary>
		public Cap EndCap
		{
			set { SetValue("setStartCap", value.ToAJO()); }
		}

		/// <summary>
		/// Sets/gets the joint type for all vertices of the polyline except the start and end vertices.
		/// </summary>
		public JointType JointType
		{
			get { return (JointType) GetValue<int>("getJointType"); }
			set { SetValue("setJointType", (int) value); }
		}

		/// <summary>
		/// Gets or sets the index of the Z. Polylines with higher zIndices are drawn above those with lower indices.
		/// </summary>
		/// <value>The index of the Z.</value>
		public float ZIndex
		{
			get { return GetValue<float>("getZIndex"); }
			set { SetValue("setZIndex", value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this polyline is visible.
		/// If this polyline is not visible then it will not be drawn. All other state is preserved. Polylines are visible by default.
		/// </summary>
		/// <value><c>true</c> if this polyline is visible; otherwise, <c>false</c>.</value>
		public bool IsVisible
		{
			get { return GetValue<bool>("isVisible"); }
			set { SetValue("setVisible", value); }
		}

		/// <summary>
		/// Indicates whether the segments of the polyline should be drawn as geodesics, as opposed to straight lines on the Mercator projection. 
		/// A geodesic is the shortest path between two points on the Earth's surface. The geodesic curve is constructed assuming the Earth is a sphere
		/// </summary>
		public bool IsGeodesic
		{
			get { return GetValue<bool>("isGeodesic"); }
			set { SetValue("setGeodesic", value); }
		}

		/// <summary>
		/// Gets/sets the clickability of the polyline.
		/// </summary>
		public bool IsClickable
		{
			get { return GetValue<bool>("isClickable"); }
			set { SetValue("setClickable", value); }
		}

		/// <summary>
		/// Removes this polyline from the map.
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
					"This polyline was already removed from the map. You can't perform any more operations on it.");
			}
		}

		public override string ToString()
		{
			var pointsStr = Points.ConvertAll(latLng => latLng.ToString());
			var points = pointsStr.Aggregate((acc, next) => acc + ", " + next);
			return string.Format("[Polyline: Points={0}, Color={1}, Width={2}, JointType={3}, ZIndex={4}, IsVisible={5}, IsGeodesic={6}, IsClickable={7}]",
				points, Color, Width, JointType, ZIndex, IsVisible, IsGeodesic, IsClickable);
		}
	}
}