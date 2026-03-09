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
		
		file.SetValue("crosshair", "type", (byte)settings.crosshairType);
		file.SetValue("crosshair", "color", settings.crosshairData.color);
		file.SetValue("crosshair", "outline_thickness", settings.crosshairData.outlineThickness);

		switch(settings.crosshairType) {
			case CrosshairType.Classic: {
				ClassicCrosshairData c = (ClassicCrosshairData)settings.crosshairData;
				file.SetValue("crosshair", "length", c.length);
				file.SetValue("crosshair", "thickness", c.thickness);
				file.SetValue("crosshair", "gap", c.gap);
				break;
			}
				
			case CrosshairType.Dot: {
				DotCrosshairData c = (DotCrosshairData)settings.crosshairData;
				file.SetValue("crosshair", "radius", c.radius);
				break;
			}
		}

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
			audioVolume = file.GetValue("audio", "volume", Settings.Default.audioVolume).AsSingle(),
crosshairType = (CrosshairType)file.GetValue("crosshair", "type", (byte)Settings.Default.crosshairType).AsByte(),
		};

		Color crosshairColor = file.GetValue("crosshair", "color", Settings.Default.crosshairData.color).AsColor();
		float crosshairOutlineThickness = file.GetValue("crosshair", "outline_thickness", Settings.Default.crosshairData.outlineThickness).AsSingle();

		switch(settings.crosshairType) {
			case CrosshairType.Classic:
				settings.crosshairData = new ClassicCrosshairData() {
					color = crosshairColor,
					outlineThickness = crosshairOutlineThickness,
					length = file.GetValue("crosshair", "length", 3.0f).AsSingle(),
					thickness = file.GetValue("crosshair", "thickness", 1.0f).AsSingle(),
					gap = file.GetValue("crosshair", "gap", 2.0f).AsSingle(),
				};
				break;

			case CrosshairType.Dot:
				settings.crosshairData = new DotCrosshairData() {
					color = crosshairColor,
					outlineThickness = crosshairOutlineThickness,
					radius = file.GetValue("crosshair", "radius", 2.0f).AsSingle(),
				};
				break;
		}

		ApplySettings();
	}

	public static void ApplySettings() {
		Engine.MaxFps = (int)settings.maxFps;
		SfxManager.SetMasterVolume(settings.audioVolume);
	}
}
