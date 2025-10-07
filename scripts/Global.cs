using Godot;

public partial class Global : Node {
	public static Global Instance { get; private set; }

	public CommandManager CommandManager { get; private set; } = new();
	public ConsoleUI ConsoleUI { get; private set; }

	public override void _EnterTree() {
		Instance = this;

		SettingsManager.Load();
		SaveManager.Load();
	}

	public override void _Ready() {
		ConsoleUI = GD.Load<PackedScene>("uid://cg0n86m4didin").Instantiate<ConsoleUI>();
		AddChild(ConsoleUI);
	}

	public void Exit() {
		GetTree().Quit();
	}
}
