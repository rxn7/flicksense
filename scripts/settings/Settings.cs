[System.Serializable]
public struct Settings {
	public float sensitivity;
	public float audioVolume;
	public uint maxFps;

	public static Settings Default = new() {
		sensitivity = 1.2f,
		audioVolume = 0.0f,
		maxFps = 0,
	};
};
