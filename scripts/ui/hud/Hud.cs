using Godot;

[GlobalClass]
public partial class Hud : CanvasLayer {
	[Export] private Label m_missesLabel;
	[Export] private Label m_hitsLabel;
	[Export] private Label m_accuracyLabel;
	[Export] private Label m_hitStreakMultiplierLabel;
	[Export] private Label m_timeLabel;
	[Export] private ScoreLabel m_scoreLabel;

	private ulong m_startTimeMs = 0;

	public override void _PhysicsProcess(double delta) {
		UpdateTime();
	}

	public void UpdateStats(ScoreManager scoreManager) {
		ref Stats stats = ref scoreManager.GetStats();

		m_missesLabel.Text = stats.Misses.ToString();
		m_hitsLabel.Text = stats.Hits.ToString();
		m_accuracyLabel.Text = $"{(stats.Accuracy * 100.0f):0}%";
	}

	public void UpdateScore(ulong score, ulong addedScore) {
		m_scoreLabel.UpdateScore(score, addedScore);
	}

	public void UpdateHitStreakMultiplier(float multiplier) {
		m_hitStreakMultiplierLabel.Text = $"x{multiplier:0.00}";
	}

	public void Reset(ulong startTimeMs) {
		m_startTimeMs = startTimeMs;
		m_scoreLabel.Reset();
	}

	private void UpdateTime() {
		ulong nowMs = Time.GetTicksMsec();

		ulong minutesElapsed = (nowMs - m_startTimeMs) / 60000;
		ulong seconds = ((nowMs - m_startTimeMs) / 1000) % 60;
		ulong milliseconds = (nowMs - m_startTimeMs) % 1000;

		m_timeLabel.Text = $"{minutesElapsed:0}:{seconds:00}.{milliseconds:000}";
	}
}
