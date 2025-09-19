using System.Linq;
using System;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute {
	public string Name { get; }
	public string Description { get; }
	public CommandArgument[] Arguments { get; }

	public CommandAttribute(string name, string desc, params string[] args) {
		Name = name;
		Description = desc;

		Arguments = new CommandArgument[args.Length];

		for(int i=0; i<args.Length; ++i) {
			string arg = args[i];

			bool required = arg.Contains('*');
			string clean = required ? arg.Replace("*", String.Empty) : arg;

			string[] parts = clean.Split('=');

			string argName = parts[0];
			object defaultValue = parts.Length > 1 ? parts[1] : null;

			Arguments[i] = new CommandArgument(argName, defaultValue, required);
		}
	}
}
