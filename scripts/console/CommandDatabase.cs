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
			Logger.Info($"Current sensitivity: {SettingsManager.settings.sensitivity}");
			return null;
		}

		float sens = cmd.Arguments["value"].GetFloat(args[0]);

		SettingsManager.settings.sensitivity = sens;
		SettingsManager.Save();

		return null;
	}

	[Command("volume", "Sets audio volume", "value*=1.0")]
	public static string VolumeCommand(Command cmd, string[] args) {
		if(args.Length == 0) {
			Logger.Info($"Current volume: {SettingsManager.settings.audioVolume}");
			return null;
		}

		float volume = cmd.Arguments["value"].GetFloat(args[0]);

		SfxManager.SetMasterVolume(volume);

		SettingsManager.settings.audioVolume = volume;
		SettingsManager.Save();

		return null;
	}

	[Command("max_fps", "Sets max FPS", "value*=0")]
	public static string MaxFpsCommand(Command cmd, string[] args) {
		if(args.Length == 0) {
			Logger.Info($"Current max FPS: {SettingsManager.settings.maxFps}");
			return null;
		}

		int maxFps = cmd.Arguments["value"].GetInt(args[0]);
		Engine.MaxFps = maxFps;

		SettingsManager.settings.maxFps = (uint)maxFps;
		SettingsManager.Save();

		return null;
	}

	[Command("cfg_reset", "Resets the config file")]
	public static string ConfigResetCommand(Command cmd, string[] args) {
		SettingsManager.settings = Settings.Default;

		SettingsManager.Save();
		SettingsManager.ApplySettings();

		return null;
	}
}
