using Godot;

public static class SettingsManager {
	private static string s_configPath = "user://settings.cfg";

	public static void Save(ref Settings settings) {
		ConfigFile s_file = new ConfigFile();

		// TODO: Use serialization
		
		s_file.SetValue("video", "max_fps", settings.maxFps);
		s_file.SetValue("input", "sens", settings.sensitivity);
		s_file.SetValue("audio", "volume", settings.audioVolume);


		s_file.Save(s_configPath);
	}

	public static void Load(ref Settings settings) {
		ConfigFile s_file = new ConfigFile();

		if(s_file.Load(s_configPath) != Error.Ok) {
			settings = Settings.Default;
			Logger.Warn("Failed to load settings, if it's the first time running the game, you can ignore this warning.");
			return;
		}
		
		settings.maxFps = s_file.GetValue("video", "max_fps", settings.maxFps).AsUInt32();
		settings.sensitivity = s_file.GetValue("input", "sens", settings.sensitivity).AsSingle();
		settings.audioVolume = s_file.GetValue("audio", "volume", settings.audioVolume).AsSingle();

		ApplySettings(ref settings);
	}

	public static void ApplySettings(ref Settings settings) {
		Engine.MaxFps = (int)settings.maxFps;
		SfxManager.SetMasterVolume(settings.audioVolume);
	}
}
