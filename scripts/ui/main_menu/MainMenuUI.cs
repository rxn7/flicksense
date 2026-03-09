using Godot;

[GlobalClass]
public partial class MainMenuUI : CanvasLayer {
	[Export] private Label m_versionLabel;

	[Export] private Control m_mainPanel;
	[Export] private GameModeSelectionUI m_gameModeSelection;
	[Export] private SettingsScreenUI m_settingsScreen;

	[Export] private Button m_playButton;
	[Export] private Button m_settingsButton;
	[Export] private Button m_exitButton;

	public override void _Ready() {
		m_mainPanel.Show();
		Input.MouseMode = Input.MouseModeEnum.Visible;

		m_versionLabel.Text = $"v{ProjectSettings.GetSetting("application/config/version")}";

		m_playButton.Pressed += () => m_gameModeSelection.Show();
		m_settingsButton.Pressed += () => m_settingsScreen.Open();
		m_exitButton.Pressed += () => GetTree().Quit();
		
		m_settingsScreen.VisibilityChanged += () => {
			if(!m_settingsScreen.Visible) {
				m_mainPanel.Show();
			}
		};
	}
}
