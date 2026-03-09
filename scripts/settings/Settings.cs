public struct Settings {
	public float sensitivity;
	public float audioVolume;
	public uint maxFps;

	public CrosshairType crosshairType;
	public CrosshairData crosshairData;

	public static Settings Default = new() {
		sensitivity = 1.0f,
		audioVolume = 1.0f,
		maxFps = 0,

		// crosshairType = CrosshairType.Classic,
		// crosshairData = new ClassicCrosshairData(),

		crosshairType = CrosshairType.Dot,
		crosshairData = new DotCrosshairData(),
	};
};
