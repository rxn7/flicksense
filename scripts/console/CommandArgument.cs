public class CommandArgument {
	public string Name { get; }
	public object DefaultValue { get; }
	public bool Required { get; }

	public CommandArgument(string name, object defaultValue = default, bool required = false) {
		Name = name;
		Required = required;
		DefaultValue = defaultValue;
	}

	public float GetInt(string arg) {
		if(int.TryParse(arg, out int result)) {
			return result;
		}

		if(Required) {
			Logger.Error($"Argument {Name} is required");
		}

		return (int)DefaultValue;
	}

	public float GetFloat(string arg) {
		if(float.TryParse(arg, out float result)) {
			return result;
		}

		if(Required) {
			Logger.Error($"Argument {Name} is required");
		}

		return (float)DefaultValue;
	}
}
