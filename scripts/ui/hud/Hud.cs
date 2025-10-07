using Godot;

[GlobalClass]
public partial class Hud : CanvasLayer {
	[Export] private Label m_missesLabel;
	[Export] private Label m_hitsLabel;
	[Export] private Label m_accuracyLabel;
	[Export] private Label m_hitStreakMultiplierLabel;
	[Export] private Label m_timeLabel;
	[Export] private ScoreLabel m_scoreLabel;

	public void UpdateStats(Stats stats) {
		m_missesLabel.Text = stats.Misses.ToString();
		m_hitsLabel.Text = stats.Hits.ToString();
		m_accuracyLabel.Text = StringHelper.Value01ToPercentString(stats.Accuracy);
	}

	public void UpdateScore(ulong score, ulong addedScore) {
		m_scoreLabel.UpdateScore(score, addedScore);
	}

	public void UpdateHitStreakMultiplier(float multiplier) {
		m_hitStreakMultiplierLabel.Text = $"x{multiplier:0.00}";
	}

	public void Reset() {
		m_scoreLabel.Reset();
	}

	public void UpdateTimeText(EGameMode gameMode, float elapsedSeconds) {
		// On time limit mode, the timer acts as a countdown
		if(gameMode == EGameMode.TimeLimit) {
			elapsedSeconds = GameManager.TIME_LIMIT_SECONDS - elapsedSeconds;
		}

		m_timeLabel.Text = StringHelper.TimeStringFromSeconds(elapsedSeconds);
	}
}
