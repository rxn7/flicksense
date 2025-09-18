using Godot;

public partial class StatsUI : Control {
	[Export] private Label m_missesLabel;
	[Export] private Label m_hitsLabel;
	[Export] private Label m_accuracyLabel;
	[Export] private Label m_scoreLabel;
	[Export] private Label m_hitStreakMultiplierLabel;

	public void UpdateStats(ScoreManager scoreManager) {
		ref Stats stats = ref scoreManager.GetStats();

		m_missesLabel.Text = stats.Misses.ToString();
		m_hitsLabel.Text = stats.Hits.ToString();
		m_accuracyLabel.Text = $"{(stats.Accuracy * 100.0f):0}%";
		m_scoreLabel.Text = stats.Score.ToString();
	}

	public void UpdateHitStreakMultiplier(float multiplier) {
		m_hitStreakMultiplierLabel.Text = $"x{multiplier:0.00}";
	}
}
