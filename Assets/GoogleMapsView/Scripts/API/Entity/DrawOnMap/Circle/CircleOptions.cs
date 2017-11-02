namespace DeadMosquito.GoogleMapsView
{
	using System;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// Circle options. 
	/// 
	/// For detailed documentation please see https://developers.google.com/android/reference/com/google/android/gms/maps/model/CircleOptions.html
	/// </summary>
	public sealed class CircleOptions
	{
		const string CircleOptionsClass = "com.google.android.gms.maps.model.CircleOptions";

#pragma warning disable 0414
		LatLng _latLng = null;
		double _radius = 0.0D;
		float _strokeWidth = 10.0F;
		int _strokeColor = -16777216;
		int _fillColor = 0;
		float _zIndex = 0F;
		bool _visible = true;
		bool _clickable = false;
#pragma warning restore 0414

		/// <summary>
		/// Sets the center using a <see cref="LatLng"/>.
		/// The center must not be null. This method is mandatory because there is no default center.
		/// </summary>
		/// <param name="latLng">The geographic center as a <see cref="LatLng"/></param>
		public CircleOptions Center(LatLng latLng)
		{
			if (latLng == null)
			{
				throw new ArgumentNullException("latLng");
			}

			_latLng = latLng;
			return this;
		}

		/// <summary>
		/// Sets the radius in meters.
		/// The radius must be zero or greater. The default radius is zero.
		/// </summary>
		/// <param name="radius">Circle radius.</param>
		public CircleOptions Radius(double radius)
		{
			_radius = radius;
			return this;
		}

		/// <summary>
		/// Sets the stroke width.
		// The stroke width is the width (in screen pixels) of the circle's outline. It must be zero or greater. If it is zero then no outline is drawn.
		/// The default width is 10 pixels.
		/// </summary>
		/// <param name="strokeWidth">Stroke width.</param>
		public CircleOptions StrokeWidth(float strokeWidth)
		{
			_strokeWidth = strokeWidth;
			return this;
		}

		/// <summary>
		/// Sets the stroke color.
		// The stroke color is the color of this circle's outline. If TRANSPARENT is used then no outline is drawn.
		// By default the stroke color is black.
		/// </summary>
		/// <param name="strokeColor">Stroke color.</param>
		public CircleOptions StrokeColor(Color strokeColor)
		{
			_strokeColor = strokeColor.ToAndroidColor();
			return this;
		}

		/// <summary>
		/// Sets the fill color.
		/// The fill color is the color inside the circle. If TRANSPARENT is used then no fill is drawn.
		/// By default the fill color is transparent.
		/// </summary>
		/// <param name="fillColor">Fill color.</param>
		public CircleOptions FillColor(Color fillColor)
		{
			_fillColor = fillColor.ToAndroidColor();
			return this;
		}

		/// <summary>
		/// Sets the zIndex.
		// Overlays (such as circles) with higher zIndices are drawn above those with lower indices.
		// By default the zIndex is 0.0
		/// </summary>
		/// <param name="zIndex">Z index.</param>
		public CircleOptions ZIndex(float zIndex)
		{
			_zIndex = zIndex;
			return this;
		}

		/// <summary>
		/// Sets the visibility.
		/// If this circle is not visible then it is not drawn, but all other state is preserved.
		/// </summary>
		/// <param name="visible">If set to <c>true</c> visible.</param>
		public CircleOptions Visible(bool visible)
		{
			_visible = visible;
			return this;
		}

		/// <summary>
		/// Specifies whether this circle is clickable. The default setting is <code>false<code>/.
		/// </summary>
		/// <param name="clickable">Whether the circle is cickable.</param>
		public CircleOptions Clickable(bool clickable)
		{
			_clickable = clickable;
			return this;
		}

		public AndroidJavaObject ToAJO()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return null;
			}

			var ajo = new AndroidJavaObject(CircleOptionsClass);
			if (_latLng != null)
			{
				ajo.CallAJO("center", _latLng.ToAJO());
			}
			ajo.CallAJO("radius", _radius);
			ajo.CallAJO("strokeWidth", _strokeWidth);
			ajo.CallAJO("strokeColor", _strokeColor);
			ajo.CallAJO("fillColor", _fillColor);
			ajo.CallAJO("zIndex", _zIndex);
			ajo.CallAJO("visible", _visible);
			ajo.CallAJO("clickable", _clickable);

			return ajo;
		}
	}
}