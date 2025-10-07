using Godot;

public static class SettingsManager {
	private const string CONFIG_PATH = "user://settings.cfg";
	public static Settings settings = new();

	public static void Save() {
		using ConfigFile file = new ConfigFile();

		// TODO: Use serialization
		
		file.SetValue("video", "max_fps", settings.maxFps);
		file.SetValue("input", "sens", settings.sensitivity);
		file.SetValue("audio", "volume", settings.audioVolume);

		file.Save(CONFIG_PATH);
	}

	public static void Load() {
		using ConfigFile file = new ConfigFile();

		if(file.Load(CONFIG_PATH) != Error.Ok) {
			settings = Settings.Default;
			return;
		}

		settings = new() {
			maxFps = file.GetValue("video", "max_fps", Settings.Default.maxFps).AsUInt32(),
			sensitivity = file.GetValue("input", "sens", Settings.Default.sensitivity).AsSingle(),
			audioVolume = file.GetValue("audio", "volume", Settings.Default.audioVolume).AsSingle()
		};

		ApplySettings();
	}

	public static void ApplySettings() {
		Engine.MaxFps = (int)settings.maxFps;
		SfxManager.SetMasterVolume(settings.audioVolume);
	}
}
