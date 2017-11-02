namespace DeadMosquito.GoogleMapsView
{
	using System.Collections.Generic;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// Defines PolylineOptions for a polyline.
	/// 
	/// Refer to https://developers.google.com/android/reference/com/google/android/gms/maps/model/PolylineOptions for more detailed documentation
	/// </summary>
	public sealed class PolylineOptions
	{
		const string PolylineOptionsClass = "com.google.android.gms.maps.model.PolylineOptions";

		readonly List<LatLng> _points;
		bool _clickable = false;
		int _color = -16777216;
		Cap _startCap = new ButtCap();
		Cap _endCap = new ButtCap();
		bool _geodesic = false;
		bool _visible = true;
		float _width = 10f;
		float _zIndex = 0f;
		JointType _jointType = DeadMosquito.GoogleMapsView.JointType.Default;

		public PolylineOptions()
		{
			_points = new List<LatLng>();
		}

		/// <summary>
		/// Adds vertices to the end of the polyline being built.
		/// </summary>
		public PolylineOptions Add(params LatLng[] points)
		{
			_points.AddRange(points);
			return this;
		}

		/// <summary>
		/// Adds a vertex to the end of the polyline being built.
		/// </summary>
		public PolylineOptions Add(LatLng point)
		{
			_points.Add(point);
			return this;
		}

		/// <summary>
		/// Adds vertices to the end of the polyline being built.
		/// </summary>
		public PolylineOptions Add(IEnumerable<LatLng> points)
		{
			_points.AddRange(points);
			return this;
		}

		/// <summary>
		/// Specifies whether this polyline is clickable. The default setting is <code>false</code>
		/// </summary>
		public PolylineOptions Clickable(bool clickable)
		{
			_clickable = clickable;
			return this;
		}

		/// <summary>
		/// Sets the color of the polyline. The default color is black <see cref="UnityEngine.Color.black"/>.
		/// </summary>
		public PolylineOptions Color(Color color)
		{
			_color = color.ToAndroidColor();
			return this;
		}

		/// <summary>
		/// Sets the cap at the start vertex of the polyline. The default start cap is <see cref="ButtCap"/>.
		/// </summary>
		public PolylineOptions StartCap(Cap startCap)
		{
			_startCap = startCap;
			return this;
		}

		/// <summary>
		/// Sets the cap at the end vertex of the polyline. The default start cap is <see cref="ButtCap"/>.
		/// </summary>
		public PolylineOptions EndCap(Cap endCap)
		{
			_endCap = endCap;
			return this;
		}

		/// <summary>
		/// Specifies whether to draw each segment of this polyline as a geodesic. The default setting is <code>false</code>
		/// </summary>
		public PolylineOptions Geodesic(bool geodesic)
		{
			_geodesic = geodesic;
			return this;
		}

		/// <summary>
		/// Specifies the visibility for the polyline. The default visibility is <code>true</code>.
		/// </summary>
		public PolylineOptions Visible(bool visible)
		{
			_visible = visible;
			return this;
		}

		/// <summary>
		/// Sets the width of the polyline in screen pixels. The default is 10.
		/// </summary>
		public PolylineOptions Width(float width)
		{
			_width = width;
			return this;
		}

		/// <summary>
		/// Specifies the polyline's zIndex, i.e., the order in which it will be drawn.
		/// </summary>
		public PolylineOptions ZIndex(float zIndex)
		{
			_zIndex = zIndex;
			return this;
		}

		/// <summary>
		/// Sets the joint type for all vertices of the polyline except the start and end vertices.
		/// 
		/// See <see cref="DeadMosquito.GoogleMapsView.JointType"/> for allowed values. The default value <see cref="DeadMosquito.GoogleMapsView.JointType.Default"/> will be used if joint type is undefined or is not one of the allowed values.
		/// </summary>
		public PolylineOptions JointType(JointType jointType)
		{
			_jointType = jointType;
			return this;
		}

		public AndroidJavaObject ToAJO()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return null;
			}

			var ajo = new AndroidJavaObject(PolylineOptionsClass);

			foreach (var point in _points)
			{
				ajo.CallAJO("add", point.ToAJO());
			}

			ajo.CallAJO("startCap", _startCap.ToAJO());
			ajo.CallAJO("endCap", _endCap.ToAJO());

			ajo.CallAJO("zIndex", _zIndex);
			ajo.CallAJO("visible", _visible);
			ajo.CallAJO("clickable", _clickable);
			ajo.CallAJO("color", _color);
			ajo.CallAJO("geodesic", _geodesic);
			ajo.CallAJO("width", _width);
			ajo.CallAJO("jointType", (int) _jointType);

			return ajo;
		}
	}
}