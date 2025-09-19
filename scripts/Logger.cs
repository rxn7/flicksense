using Godot;

public static class Logger {
	public static void Info(string message) {
		GD.Print($"[INFO] {message}");
		Global.Instance?.ConsoleUI.PrintInfo(message);
	}

	public static void Error(string message) {
		GD.PushError($"[ERROR] {message}");
		Global.Instance?.ConsoleUI.PrintError(message);
	}

	public static void Warn(string message) {
		GD.PushWarning($"[WARNING] {message}");
		Global.Instance?.ConsoleUI.PrintWarning(message);
	}
}
