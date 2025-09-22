using Godot;

public partial class MainMenu : CanvasLayer {
	[Export] private Label m_versionLabel;

	[Export] private Button m_playButton;
	[Export] private Button m_settingsButton;
	[Export] private Button m_exitButton;

	public override void _EnterTree() {
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	public override void _Ready() {
		m_versionLabel.Text = $"v{ProjectSettings.GetSetting("application/config/version")}";

		m_playButton.Pressed += OnPlayButtonPressed;
		m_exitButton.Pressed += () => GetTree().Quit();
	}

	private void OnPlayButtonPressed() {
		GetTree().ChangeSceneToFile("res://scenes/game.tscn");
	}
}
