namespace DeadMosquito.GoogleMapsView.Demo
{
	using System.Collections.Generic;
	using Internal;
	using UnityEngine;

	public static class DemoUtils
	{
		public static readonly LatLng BerlinLatLng = new LatLng(BerlinLatitude, BerlinLongitude);

		public static readonly LatLngBounds BerlinLatLngBounds =
			new LatLngBounds(BerlinLatLng, new LatLng(BerlinLatitude + 10, BerlinLongitude + 10));
		
		public static readonly List<LatLng> UsaPolylinePoints = new List<LatLng>
		{
			new LatLng(37.002361f, -114.110389f), 
			new LatLng(41.958444f, -114.020981f),
			new LatLng(45.008024f, -111.008123f),
			new LatLng(44.987813, -104.078093)
		};

		public static readonly List<LatLng> ColoradoHole = new List<LatLng>
		{
			new LatLng(38.37f, -107.32f),
			new LatLng(40.07f, -105.88f),
			new LatLng(38.09f, -103.40)
		};

		public static readonly List<List<LatLng>> ColoradoHoles = new List<List<LatLng>> {ColoradoHole};

		public static readonly List<LatLng> ColoradoBorders = new List<LatLng>
		{
			new LatLng(37f, -109.04),
			new LatLng(41.05f, -109.04),
			new LatLng(41.05f, -102.04f),
			new LatLng(37f, -102.04)
		};

		const float BerlinLatitude = 52.4965725F;
		const float BerlinLongitude = 13.3933283F;

		public static CircleOptions CreateInitialCircleOptions()
		{
			const float SydneyLatitude = -34;
			const float SydneyLongitude = 151;

			// Create a circle in Sydney, Australia
			return new CircleOptions()
				.Center(new LatLng(SydneyLatitude, SydneyLongitude))
				.Radius(100000f)
				.StrokeWidth(5f)
				.StrokeColor(Color.red)
				.FillColor(Color.green)
				.Visible(true)
				.Clickable(true)
				.ZIndex(1);
		}

		public static MarkerOptions CreateInitialMarkerOptions()
		{
			const float LondonLatitude = 51.5285582f;
			const float LondonLongitude = -0.2417005f;

			// Create a amrker in London, Great Britain
			return new MarkerOptions()
				.Position(new LatLng(LondonLatitude, LondonLongitude))
				.Icon(ImageDescriptor.FromAsset("map-marker-icon.png")) // image must be in StreamingAssets folder!
				.Alpha(0.5f) // make semi-transparent image
				.Anchor(0.5f, 1f) // anchor point of the image
				.InfoWindowAnchor(0.5f, 1f)
				.Draggable(true)
				.Flat(false)
				.Rotation(30f) // Rotate marker a bit
				.Snippet("Snippet Text")
				.Title("Title Text")
				.Visible(true)
				.ZIndex(1f);
		}

		public static GroundOverlayOptions CreateInitialGroundOverlayOptions()
		{
			return new GroundOverlayOptions()
//                .Position(new LatLng(BerlinLatitude, BerlinLongitude), 303000, 150000)
				.PositionFromBounds(BerlinLatLngBounds)
				.Image(ImageDescriptor.FromAsset("overlay.png")) // image must be in StreamingAssets folder!
				.Anchor(1, 1)
				.Bearing(45)
				.Clickable(true)
				.Transparency(0)
				.Visible(true)
				.ZIndex(1);
		}

		public static MarkerOptions RandomColorMarkerOptions(LatLng point)
		{
			return new MarkerOptions()
				.Position(point)
				.Icon(ImageDescriptor.DefaultMarker(Random.Range(0, 360)));
		}

		public static CircleOptions RandomColorCircleOptions(LatLng point)
		{
			return new CircleOptions()
				.Center(point)
				.Radius(1000000)
				.FillColor(ColorUtils.RandomColor())
				.StrokeColor(ColorUtils.RandomColor());
		}

		public static PolylineOptions CreateInitialPolylineOptions()
		{
			return new PolylineOptions()
				.Add(new LatLng(10, 10), new LatLng(30, 30), new LatLng(-30, 30), new LatLng(50, 50))
				.Clickable(true)
				.Color(Color.red)
				.StartCap(new CustomCap(ImageDescriptor.FromAsset("cap.png"), 16f))
				.EndCap(new RoundCap())
				.JointType(JointType.Round)
				.Geodesic(false)
				.Visible(true)
				.Width(20)
				.ZIndex(1f);
		}

		public static PolygonOptions CreateColoradoStatePolygonOptions()
		{
			return new PolygonOptions()
				.Add(ColoradoBorders)
				.FillColor(new Color(0.5f, 0.5f, 0.5f, 0.2f))
				.StrokeColor(new Color(0.5f, 0f, 0f, 0.8f))
				.StrokeWidth(20f)
				.StrokeJointType(JointType.Round)
				.AddHole(ColoradoHole)
				.Visible(true)
				.Clickable(true)
				.Geodesic(false)
				.ZIndex(1f);
		}

		public static LatLng RandomLatLng()
		{
			return new LatLng(Random.Range(-90f, 90f), Random.Range(-180f, 180f));
		}

		public static List<LatLng> RandomLocations(int size)
		{
			var locations = new List<LatLng>(size);
			for (int i = 0; i < size; i++)
			{
				locations.Add(RandomLatLng());
			}

			return locations;
		}
	}
}