namespace DeadMosquito.GoogleMapsView
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// Defines options for a polygon.
	/// 
	/// See: https://developers.google.com/android/reference/com/google/android/gms/maps/model/PolygonOptions
	/// </summary>
	public sealed class PolygonOptions
	{
		const string PolygonOptionsClass = "com.google.android.gms.maps.model.PolygonOptions";

		readonly List<LatLng> _points;
		readonly List<List<LatLng>> _holes;
		
		bool _clickable = false;
		int _fillColor = Color.black.ToAndroidColor();
		int _strokeColor = Color.black.ToAndroidColor();
		float _zIndex = 0f;
		bool _geodesic = false;
		bool _visible = true;
		JointType _strokeJointType = JointType.Default;
		float _strokeWidth = 10f;

		public PolygonOptions()
		{
			_points = new List<LatLng>();
			_holes = new List<List<LatLng>>();
		}

		/// <summary>
		/// Adds vertices to the end of the polygon being built.
		/// </summary>
		public PolygonOptions Add(params LatLng[] points)
		{
			_points.AddRange(points);
			return this;
		}

		/// <summary>
		/// Adds a vertex to the end of the polygon being built.
		/// </summary>
		public PolygonOptions Add(LatLng point)
		{
			_points.Add(point);
			return this;
		}

		/// <summary>
		/// Adds vertices to the end of the polygon being built.
		/// </summary>
		public PolygonOptions Add(IEnumerable<LatLng> points)
		{
			_points.AddRange(points);
			return this;
		}

		/// <summary>
		/// Adds a hole to the polygon being built.
		/// </summary>
		/// <param name="points">Hole to add to the polygon</param>
		public PolygonOptions AddHole(IEnumerable<LatLng> points)
		{
			_holes.Add(points.ToList());
			return this;
		}

		/// <summary>
		/// Specifies whether this polygon is clickable. The default setting is <code>false</code>
		/// </summary>
		public PolygonOptions Clickable(bool clickable)
		{
			_clickable = clickable;
			return this;
		}

		/// <summary>
		/// Sets the fill color of the polygon. The default color is black <see cref="UnityEngine.Color.black"/>.
		/// </summary>
		public PolygonOptions FillColor(Color color)
		{
			_fillColor = color.ToAndroidColor();
			return this;
		}

		/// <summary>
		/// Sets the stroke color of the polygon. The default color is black <see cref="UnityEngine.Color.black"/>.
		/// </summary>
		public PolygonOptions StrokeColor(Color color)
		{
			_strokeColor = color.ToAndroidColor();
			return this;
		}

		/// <summary>
		/// Specifies the polygon's stroke width, in display pixels. The default width is 10.
		/// </summary>
		public PolygonOptions StrokeWidth(float width)
		{
			_strokeWidth = width;
			return this;
		}

		/// <summary>
		/// Specifies whether to draw each segment of this polygon as a geodesic. The default setting is <code>false</code>
		/// </summary>
		public PolygonOptions Geodesic(bool geodesic)
		{
			_geodesic = geodesic;
			return this;
		}

		/// <summary>
		/// Specifies the joint type for all vertices of the polygon's outline.
		/// 
		/// See <see cref="DeadMosquito.GoogleMapsView.JointType"/> for allowed values. The default value <see cref="DeadMosquito.GoogleMapsView.JointType.Default"/> will be used if joint type is undefined or is not one of the allowed values.
		/// </summary>
		public PolygonOptions StrokeJointType(JointType jointType)
		{
			_strokeJointType = jointType;
			return this;
		}

		/// <summary>
		/// Specifies the visibility for the polygon. The default visibility is <code>true</code>.
		/// </summary>
		public PolygonOptions Visible(bool visible)
		{
			_visible = visible;
			return this;
		}

		/// <summary>
		/// Specifies the polygon's zIndex, i.e., the order in which it will be drawn.
		/// </summary>
		public PolygonOptions ZIndex(float zIndex)
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

			var ajo = new AndroidJavaObject(PolygonOptionsClass);

			foreach (var point in _points)
			{
				ajo.CallAJO("add", point.ToAJO());
			}

			ajo.CallAJO("zIndex", _zIndex);
			ajo.CallAJO("visible", _visible);
			ajo.CallAJO("clickable", _clickable);
			ajo.CallAJO("fillColor", _fillColor);
			ajo.CallAJO("strokeColor", _strokeColor);
			ajo.CallAJO("strokeWidth", _strokeWidth);
			ajo.CallAJO("strokeJointType", (int) _strokeJointType);
			ajo.CallAJO("geodesic", _geodesic);

			foreach (var hole in _holes)
			{
				ajo.CallAJO("addHole", hole.ToJavaList(latLng => latLng.ToAJO()));
			}

			return ajo;
		}
	}
}