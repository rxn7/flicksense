using Godot;

public partial class MainMenuStatsUI : Control {
	[ExportGroup("General")]
	[Export] private Label m_bestStreakLabel;
	[Export] private Label m_bestComboLabel;

	[ExportGroup("Time Limit Mode")]
	[Export] private Label m_timeLimitScoreLabel;

	[ExportGroup("Surival Mode")]
	[Export] private Label m_longestSurvivedLabel;

	public override void _Ready() {
		LoadSaveStats();
	}

	private void LoadSaveStats() {
		m_bestStreakLabel.Text = SaveManager.data.bestHitStreak.ToString();
		m_bestComboLabel.Text = $"x{SaveManager.data.bestStreakMultiplier:0.0}";
		m_timeLimitScoreLabel.Text = SaveManager.data.bestTimeLimitScore.ToString();
	}
}
