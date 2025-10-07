public struct Settings {
	public float sensitivity;
	public float audioVolume;
	public uint maxFps;

	public static Settings Default = new() {
		sensitivity = 1.0f,
		audioVolume = 1.0f,
		maxFps = 0,
	};
};
