using Godot;

public static class SaveManager {
	private const string SAVE_PATH = "user://save.fssave";
	public static SaveData data = new();

	public static void Save() {
		using FileAccess file = FileAccess.OpenCompressed(SAVE_PATH, FileAccess.ModeFlags.Write);
		if(file is null) {
			Logger.Error("Failed to open the save file");
			return;
		}

		// TODO: This might cause problems in the future if the order is changed
		file.Store64(data.bestHitStreak);
		file.StoreFloat(data.bestStreakMultiplier);
		file.Store64(data.bestTimeLimitScore);

		file.Close();
	}

	public static void Load() {
		using FileAccess file = FileAccess.OpenCompressed(SAVE_PATH, FileAccess.ModeFlags.Read);

		if(file is null) {
			data = SaveData.Default;
			return;
		}

		data = new() {
			bestHitStreak = file.Get64(),
			bestStreakMultiplier = file.GetFloat(),
			bestTimeLimitScore = file.Get64()
		};
	}
}
