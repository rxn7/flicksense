using System;
using Godot;

public partial class EndScreenUI : CanvasLayer {
	public event Action onPlayAgainPressed;

	[Export] private Button m_playAgainButton;
	[Export] private Button m_mainMenuButton;

	[Export] private Label m_modeLabel;
	[Export] private Label m_timeLabel;
	[Export] private Label m_scoreLabel;
	[Export] private Label m_accuracyLabel;
	[Export] private Label m_hitsLabel;
	[Export] private Label m_missesLabel;

	public override void _Ready() {
		ProcessMode = ProcessModeEnum.Always;
		Hide();

		m_playAgainButton.Pressed += () => {
			Hide();
			onPlayAgainPressed?.Invoke();
		};

		m_mainMenuButton.Pressed += () => SceneSwitcher.SwitchToMainMenu();
	}

	public void ShowEndScreen(EGameMode gameMode, Stats stats) {
		Input.MouseMode = Input.MouseModeEnum.Visible;

		Show();

		m_modeLabel.Text = gameMode switch {
			EGameMode.Endless => "Endless Mode",
			EGameMode.TimeLimit => "Time Limit Mode",
			EGameMode.Survival => "Survival Mode",
			_ => "???"
		};

		m_timeLabel.Text = StringHelper.TimeStringFromSeconds(stats.timeElapsed);
		m_scoreLabel.Text = stats.score.ToString();
		m_accuracyLabel.Text = StringHelper.Value01ToPercentString(stats.Accuracy);
		m_hitsLabel.Text = stats.hits.ToString();
		m_missesLabel.Text = stats.Misses.ToString();
	}
}
