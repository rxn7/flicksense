using System;
using System.Collections.Generic;
using System.Reflection;

public class CommandManager {
	public static CommandManager Instance { get; private set; }
	public Dictionary<string, Command> Commands { get; private set; } = new();

	public CommandManager() {
		Instance = this;

		MethodInfo[] methods = typeof(CommandDatabase).GetMethods(BindingFlags.Public | BindingFlags.Static);

		foreach(MethodInfo method in methods) {
			CommandAttribute attr = method.GetCustomAttribute<CommandAttribute>();
			if(attr == null) {
				continue;
			}

			CommandHandler handler = (CommandHandler)Delegate.CreateDelegate(typeof(CommandHandler), method);
			Commands[attr.Name] = new Command(attr.Name, attr.Description, handler, attr.Arguments);

			Logger.Info("Registered command: " + attr.Name);
		}

		Logger.Info($"Registered {Commands.Count} commands");
	}

	public string Execute(string command, string[] args) {
		if(!Commands.ContainsKey(command)) {
			Logger.Error($"Unknown command: {command}, type 'help' for a list of commands");
			return null;
		}

		string errorMessage = Commands[command].Handler(Commands[command], args);
		if(errorMessage != null) {
			Logger.Error(errorMessage);
		}

		return errorMessage;
	}
}
