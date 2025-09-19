using Godot;
using System.Collections.Generic;

public static class CommandDatabase {
	[Command("help", "Prints a list of available commands, or the usage of the specified command", "command= ")]
	public static string CommandHelp(Command cmd, string[] args) {
		if(args.Length == 0) {
			foreach(KeyValuePair<string, Command> pair in CommandManager.Instance.Commands) {
				Logger.Info($"{pair.Key}: {pair.Value.Description}");
			}
		} else {
			if(!CommandManager.Instance.Commands.ContainsKey(args[0])) {
				return $"Unknown command '{args[0]}'";
			}
			Command c = CommandManager.Instance.Commands[args[0]];
			Logger.Info($"{c.Name}: {c.Description}, {c.GetUsage()}");
		}

		return null;
	}

	[Command("exit", "Exits the application")]
	public static string ExitCommand(Command cmd, string[] args) {
		Global.Instance.Exit();
		return null;
	}

	[Command("clear", "Clears the console")]
	public static string ClearCommand(Command cmd, string[] args) {
		Global.Instance.ConsoleUI.Clear();
		return null;
	}

	[Command("sens", "Sets mouse sensitivity", "value*=1.0")]
	public static string SensCommand(Command cmd, string[] args) {
		if(args.Length == 0) {
			Logger.Info($"Current sensitivity: {Global.Instance.settings.sensitivity}");
			return null;
		}

		float sens = cmd.Arguments["value"].GetFloat(args[0]);

		Global.Instance.settings.sensitivity = sens;
		SettingsManager.Save(ref Global.Instance.settings);

		return null;
	}

	[Command("volume", "Sets audio volume", "value*=1.0")]
	public static string VolumeCommand(Command cmd, string[] args) {
		if(args.Length == 0) {
			Logger.Info($"Current volume: {Global.Instance.settings.audioVolume}");
			return null;
		}

		float volume = cmd.Arguments["value"].GetFloat(args[0]);

		SfxManager.SetMasterVolume(volume);

		Global.Instance.settings.audioVolume = volume;
		SettingsManager.Save(ref Global.Instance.settings);

		return null;
	}

	[Command("max_fps", "Sets max FPS", "value*=0")]
	public static string MaxFpsCommand(Command cmd, string[] args) {
		if(args.Length == 0) {
			Logger.Info($"Current max FPS: {Global.Instance.settings.maxFps}");
			return null;
		}

		int maxFps = cmd.Arguments["value"].GetInt(args[0]);
		Engine.MaxFps = maxFps;

		Global.Instance.settings.maxFps = (uint)maxFps;
		SettingsManager.Save(ref Global.Instance.settings);

		return null;
	}
}
