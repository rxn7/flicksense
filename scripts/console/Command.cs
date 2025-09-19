using System.Text;
using System.Collections.Generic;

public delegate string CommandHandler(Command cmd, string[] args);

public class Command {
	public CommandHandler Handler { get; }
	public Dictionary<string, CommandArgument> Arguments { get; }
	public string Name { get; }
	public string Description { get; }

	public Command(string name, string description, CommandHandler handler, params CommandArgument[] arguments) {
		Handler = handler;
		Name = name;
		Description = description;

		Arguments = new(arguments.Length);
		foreach(CommandArgument arg in arguments) {
			Arguments[arg.Name] = arg;
		}
	}

	public string GetUsage() {
		StringBuilder strBuilder = new(50);

		if(Arguments != null) {
			foreach(KeyValuePair<string, CommandArgument> pair in Arguments) {
				strBuilder.AppendFormat("[{0}{1}]", pair.Key, pair.Value.Required ? "*" : "");
			}
		}

		return strBuilder.ToString();
	}
}
