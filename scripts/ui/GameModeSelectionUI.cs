using Godot;

public partial class GameModeSelectionUI : Control {
	[Export] private Button m_endlessModeButton;
	[Export] private Button m_timeLimitModeButton;
	[Export] private Button m_survivalModeButton;
	[Export] private Button m_goBackButton;

	public override void _Ready() {
		m_endlessModeButton.Pressed += () => SceneSwitcher.SwitchToGame(EGameMode.Endless);
		m_timeLimitModeButton.Pressed += () => SceneSwitcher.SwitchToGame(EGameMode.TimeLimit);
		m_survivalModeButton.Pressed += () => SceneSwitcher.SwitchToGame(EGameMode.Survival);

		m_goBackButton.Pressed += () => {
			Hide();
		};
	}
}
