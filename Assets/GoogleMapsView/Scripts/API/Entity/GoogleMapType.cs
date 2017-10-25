namespace DeadMosquito.GoogleMapsView
{
	/// <summary>
	/// Google map type.
	/// </summary>
	public enum GoogleMapType
	{
		/// <summary>
		/// Satellite maps with a transparent layer of major streets.
		/// </summary>
		Hybrid = 4,

		/// <summary>
		/// No base map tiles.
		/// </summary>
		None = 0,

		/// <summary>
		/// Basic maps.
		/// </summary>
		Normal = 1,

		/// <summary>
		/// Satellite maps with no labels.
		/// </summary>
		Satellite = 2,

		/// <summary>
		/// Terrain maps.
		/// </summary>
		Terrain = 3,
	}
}