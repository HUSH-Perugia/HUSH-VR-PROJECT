namespace DeadMosquito.GoogleMapsView.Internal
{
	using JniToolkit;
	using UnityEngine;

	public static class ColorUtils
	{
		public static Color RandomColor()
		{
			return new Color(Random.value, Random.value, Random.value);
		}

		public static int ToAndroidColor(this Color color)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return 0;
			}

			Color32 c = color;
			return AndroidColor(c.a, c.r, c.g, c.b);
		}

		public static int AndroidColor(int alpha,
			int red,
			int green,
			int blue)
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return 0;
			}

			using (var c = new AndroidJavaClass("android.graphics.Color"))
			{
				return c.CallStaticInt("argb", alpha, red, green, blue);
			}
		}

		public static Color ToUnityColor(this int aCol)
		{
			Color32 c;
			c.b = (byte) ((aCol) & 0xFF);
			c.g = (byte) ((aCol >> 8) & 0xFF);
			c.r = (byte) ((aCol >> 16) & 0xFF);
			c.a = (byte) ((aCol >> 24) & 0xFF);
			return (Color) c;
		}
	}
}