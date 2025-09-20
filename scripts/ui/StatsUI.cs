using Godot;

public partial class StatsUI : Control {
	[Export] private Label m_missesLabel;
	[Export] private Label m_hitsLabel;
	[Export] private Label m_accuracyLabel;
	[Export] private Label m_scoreLabel;
	[Export] private Label m_hitStreakMultiplierLabel;
	[Export] private Label m_timeLabel;

	private ulong m_startTimeMs = 0;

	public override void _PhysicsProcess(double delta) {
		UpdateTime();
	}

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

	public void SetStartTime(ulong startTimeMs) {
		m_startTimeMs = startTimeMs;
	}

	private void UpdateTime() {
		ulong nowMs = Time.GetTicksMsec();

		ulong minutesElapsed = (nowMs - m_startTimeMs) / 60000;
		ulong seconds = ((nowMs - m_startTimeMs) / 1000) % 60;
		ulong milliseconds = (nowMs - m_startTimeMs) % 1000;

		m_timeLabel.Text = $"{minutesElapsed:00}:{seconds:00}.{milliseconds:000}";
	}
}
