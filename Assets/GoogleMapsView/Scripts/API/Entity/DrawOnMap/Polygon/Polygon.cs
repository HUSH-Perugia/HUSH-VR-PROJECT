namespace DeadMosquito.GoogleMapsView
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// A polygon on the earth's surface. A polygon can be convex or concave, it may span the 180 meridian and it can have holes that are not filled in. It has the following properties:
	/// 
	/// See: https://developers.google.com/android/reference/com/google/android/gms/maps/model/Polygon
	/// </summary>
	public sealed class Polygon
	{
		bool _wasRemoved = false;
		readonly AndroidJavaObject _ajo;

		public Polygon(AndroidJavaObject ajo)
		{
			_ajo = ajo;
		}

		public Polygon()
		{
		}

		/// <summary>
		/// The outline is specified by a list of vertices in clockwise or counterclockwise order. 
		/// It is not necessary for the start and end points to coincide; if they do not, the polygon will be automatically closed. 
		/// Line segments are drawn between consecutive points in the shorter of the two directions (east or west).
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
		/// Gets/sets holes of the polygon
		/// </summary>
		public List<List<LatLng>> Holes
		{
			get
			{
				var listAJO = GetValue<AndroidJavaObject>("getHoles");
				var holesAJO = listAJO.FromJavaList<AndroidJavaObject>();
				return holesAJO.Select(holeAJO => holeAJO.FromJavaList<LatLng>(LatLng.FromAJO)).ToList();
			}
			set
			{
				var holesAJO = value.ToJavaList(holes => holes.ToJavaList(latLng => latLng.ToAJO()));
				SetValue("setHoles", holesAJO);
			}
		}

		/// <summary>
		/// Gets/sets the polygon stroke width
		/// </summary>
		public float StrokeWidth
		{
			get { return GetValue<float>("getStrokeWidth"); }
			set { SetValue("setStrokeWidth", value); }
		}

		/// <summary>
		/// Gets/sets the polygon stroke color
		/// </summary>
		public Color Color
		{
			get { return GetValue<int>("getStrokeColor").ToUnityColor(); }
			set { SetValue("setStrokeColor", value.ToAndroidColor()); }
		}
		
		/// <summary>
		/// Gets/sets the polygon fill color
		/// </summary>
		public Color FillColor
		{
			get { return GetValue<int>("getFillColor").ToUnityColor(); }
			set { SetValue("setFillColor", value.ToAndroidColor()); }
		}
		
		/// <summary>
		/// Gets/sets the joint type for all vertices of the polygon's outline.
		/// </summary>
		public JointType StrokeJointType
		{
			get { return (JointType) GetValue<int>("getStrokeJointType"); }
			set { SetValue("setStrokeJointType", (int) value); }
		}

		/// <summary>
		/// Gets or sets the index of the Z. Polygons with higher zIndices are drawn above those with lower indices.
		/// </summary>
		/// <value>The index of the Z.</value>
		public float ZIndex
		{
			get { return GetValue<float>("getZIndex"); }
			set { SetValue("setZIndex", value); }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this polygon is visible.
		/// If this polygon is not visible then it will not be drawn. All other state is preserved. Polygons are visible by default.
		/// </summary>
		/// <value><c>true</c> if this polygon is visible; otherwise, <c>false</c>.</value>
		public bool IsVisible
		{
			get { return GetValue<bool>("isVisible"); }
			set { SetValue("setVisible", value); }
		}

		/// <summary>
		/// Indicates whether the segments of the polygon should be drawn as geodesics, as opposed to straight lines on the Mercator projection. 
		/// A geodesic is the shortest path between two points on the Earth's surface. The geodesic curve is constructed assuming the Earth is a sphere
		/// </summary>
		public bool IsGeodesic
		{
			get { return GetValue<bool>("isGeodesic"); }
			set { SetValue("setGeodesic", value); }
		}

		/// <summary>
		/// Gets/sets the clickability of the polygon.
		/// </summary>
		public bool IsClickable
		{
			get { return GetValue<bool>("isClickable"); }
			set { SetValue("setClickable", value); }
		}

		/// <summary>
		/// Removes this polygon from the map.
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
					"This polygon was already removed from the map. You can't perform any more operations on it.");
			}
		}

		public override string ToString()
		{
			var pointsStr = Points.ConvertAll(latLng => latLng.ToString());
			var points = pointsStr.Aggregate((acc, next) => acc + ", " + next);

			var holesStringBuilder = new StringBuilder();
			Holes.ForEach(hole => hole.ForEach(point => holesStringBuilder.Append(point + ", ")));

			return string.Format("[Polygon: Points={0}, Holes={1}, StrokeWidth={2}, Color={3}, FillColor={4}, StrokeJointType={5}, ZIndex={6}, IsVisible={7}, IsGeodesic={8}, IsClickable={9}]",
				points, holesStringBuilder, StrokeWidth, Color, FillColor, StrokeJointType, ZIndex, IsVisible, IsGeodesic, IsClickable);
		}
	}
}