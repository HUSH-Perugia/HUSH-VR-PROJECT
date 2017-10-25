namespace DeadMosquito.GoogleMapsView
{
	/// <summary>
	/// Bitmap overlay centered at the start or end vertex of a <see cref="Polyline"/>, orientated according to the direction of the line's first or last edge and scaled with respect to the line's stroke width.
	///  CustomCap can be applied to Polyline with any stroke pattern.
	/// 
	/// See: https://developers.google.com/android/reference/com/google/android/gms/maps/model/CustomCap
	/// </summary>
	public sealed class CustomCap : Cap
	{
		public ImageDescriptor Image { get; private set; }
		public float RefWidth { get; private set; }

		/// <summary>
		/// Constructs a new <see cref="CustomCap"/>.
		/// </summary>
		/// <param name="image">Descriptor of the image to be overlaid at the start or end vertex.</param>
		/// <param name="refWidth">Reference stroke width (in pixels) - the stroke width for which the cap image at its native dimension is designed.</param>
		public CustomCap(ImageDescriptor image, float refWidth)
		{
			Image = image;
			RefWidth = refWidth;
		}

		/// <summary>
		/// Constructs a new <see cref="CustomCap"/>.
		/// </summary>
		/// <param name="image">Descriptor of the image to be overlaid at the start or end vertex.</param>
		public CustomCap(ImageDescriptor image) : this(image, 10f)
		{
		}
	}
}