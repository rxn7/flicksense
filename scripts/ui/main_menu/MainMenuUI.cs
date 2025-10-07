using Godot;

public partial class MainMenuUI : CanvasLayer {
	[Export] private Label m_versionLabel;

	[Export] private Control m_mainPanel;
	[Export] private GameModeSelectionUI m_gameModeSelection;
	// TODO: Settings

	[Export] private Button m_playButton;
	[Export] private Button m_settingsButton;
	[Export] private Button m_exitButton;

	public override void _Ready() {
		m_mainPanel.Show();
		Input.MouseMode = Input.MouseModeEnum.Visible;

		m_versionLabel.Text = $"v{ProjectSettings.GetSetting("application/config/version")}";

		m_playButton.Pressed += OnPlayButtonPressed;
		m_exitButton.Pressed += () => GetTree().Quit();
	}

	private void OnPlayButtonPressed() {
		m_gameModeSelection.Show();
	}
}
