namespace DeadMosquito.GoogleMapsView
{
	using System;
	using Internal;
	using JniToolkit;
	using UnityEngine;

	/// <summary>
	/// Defines a Bitmap image. For a marker, this class can be used to set the image of the marker icon. 
	/// For a ground overlay, it can be used to set the image to place on the surface of the earth.
	/// </summary>
	public sealed class ImageDescriptor
	{
		const string ImageDescriptorFactoryClass = "com.google.android.gms.maps.model.BitmapDescriptorFactory";
		const string ExpansionFileUtilsClass = "com.deadmosquitogames.gmaps.ExpansionFileUtils";

		public const float HUE_RED = 0.0F;
		public const float HUE_ORANGE = 30.0F;
		public const float HUE_YELLOW = 60.0F;
		public const float HUE_GREEN = 120.0F;
		public const float HUE_CYAN = 180.0F;
		public const float HUE_AZURE = 210.0F;
		public const float HUE_BLUE = 240.0F;
		public const float HUE_VIOLET = 270.0F;
		public const float HUE_MAGENTA = 300.0F;
		public const float HUE_ROSE = 330.0F;

		public enum ImageDescriptorType
		{
			Default,
			DefaultWithHue,
			AssetName
		}

#pragma warning disable 0414
		ImageDescriptorType _descriptorType = ImageDescriptorType.Default;
		readonly string _assetName;
		readonly float _hue;
#pragma warning restore 0414

		ImageDescriptor(float hue)
		{
			_descriptorType = ImageDescriptorType.DefaultWithHue;
			_hue = hue;
		}

		ImageDescriptor(string assetName)
		{
			_descriptorType = ImageDescriptorType.AssetName;
			_assetName = assetName;
		}

		ImageDescriptor()
		{
		}

		/// <summary>
		/// Creates a <see cref="ImageDescriptor"/> that refers to the default marker image.
		/// </summary>
		/// <returns>The marker image descriptor.</returns>
		public static ImageDescriptor DefaultMarker()
		{
			return new ImageDescriptor();
		}

		/// <summary>
		/// Creates a <see cref="ImageDescriptor"/> that refers to a colorization of the default marker image. For convenience, there is a predefined set of hue values. E.g. <see cref="HUE_RED"/> 
		/// </summary>
		/// <returns>The marker image descriptor.</returns>
		/// <param name="hue">The hue of the marker. Value must be greater or equal to 0 and less than 360.</param>
		public static ImageDescriptor DefaultMarker(float hue)
		{
			return new ImageDescriptor(hue);
		}

		/// <summary>
		/// Creates a <see cref="ImageDescriptor"/> using the name of the image image in the StreamingAssets directory. Must be full image name inside StreamingAssets folder e.g. "my-custom-marker.png"
		/// </summary>
		/// <returns>The marker image descriptor.</returns>
		/// <param name="assetName">Asset name. Must be full image name inside StreamingAssets folder e.g. "my-custom-marker.png"</param>
		public static ImageDescriptor FromAsset(string assetName)
		{
			if (string.IsNullOrEmpty(assetName))
			{
				throw new ArgumentException("Image name cannot be null or empty", "assetName");
			}

			return new ImageDescriptor(assetName);
		}

		public AndroidJavaObject ToAJO()
		{
			if (GoogleMapUtils.IsNotAndroidRuntime)
			{
				return null;
			}

			switch (_descriptorType)
			{
				case ImageDescriptorType.AssetName:
					using (var c = new AndroidJavaClass(ImageDescriptorFactoryClass))
					{
						try
						{
							if (IsAppBinarySplit)
							{
								var bitmap = ExpansionFileUtilsClass.AJCCallStaticOnce<AndroidJavaObject>("getBitmap", JniToolkitUtils.Activity, _assetName);
								return c.CallStaticAJO("fromBitmap", bitmap);
							}

							return c.CallStaticAJO("fromAsset", _assetName);
						}
						catch (Exception e)
						{
							Debug.LogError("Failed to load bitmap from expansion file: " + _assetName);
							Debug.LogException(e);
							return c.CallStaticAJO("defaultMarker");
						}
					}
				case ImageDescriptorType.DefaultWithHue:
					using (var c = new AndroidJavaClass(ImageDescriptorFactoryClass))
					{
						return c.CallStaticAJO("defaultMarker", _hue);
					}
				default:
					using (var c = new AndroidJavaClass(ImageDescriptorFactoryClass))
					{
						return c.CallStaticAJO("defaultMarker");
					}
			}
		}

		static bool IsAppBinarySplit
		{
			get { return Application.streamingAssetsPath.Contains("/obb/"); }
		}
	}
}