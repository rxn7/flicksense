using Godot;

[GlobalClass]
public partial class HudUI : CanvasLayer {
	[Export] private Label m_missesLabel;
	[Export] private Label m_hitsLabel;
	[Export] private Label m_accuracyLabel;
	[Export] private Label m_hitStreakMultiplierLabel;
	[Export] private Label m_timeLabel;
	[Export] private Label m_gameModeLabel;
	[Export] private SurvivalHealthUI m_survivalHealth;
	[Export] private ScoreLabelUI m_scoreLabel;

	public void Setup(EGameMode gameMode) {
		m_gameModeLabel.Text = gameMode switch {
			EGameMode.TimeLimit => "Time Limit",
			EGameMode.Endless => "Endless",
			EGameMode.Survival => "Survival",
			_ => ""
		};

		m_scoreLabel.Visible = gameMode switch {
			EGameMode.Endless => true,
			EGameMode.TimeLimit => true,
			EGameMode.Survival => false,
			_ => true,
		};

		m_survivalHealth.Visible = gameMode == EGameMode.Survival;

		Reset();
	}

	public void Reset() {
		m_scoreLabel.Reset();
		UpdateHitStreakMultiplier(1.0f);
	}

	public void UpdateStats(Stats stats) {
		m_missesLabel.Text = stats.Misses.ToString();
		m_hitsLabel.Text = stats.hits.ToString();
		m_accuracyLabel.Text = StringHelper.Value01ToPercentString(stats.Accuracy);
	}

	public void UpdateScore(ulong score, ulong addedScore) {
		m_scoreLabel.UpdateScore(score, addedScore);
	}

	public void UpdateHitStreakMultiplier(float multiplier) {
		m_hitStreakMultiplierLabel.Text = $"x{multiplier:0.00}";
	}

	public void UpdateSurvivalHealth(float health) {
		m_survivalHealth.SetValue(health);
	}

	public void UpdateTimeText(string text) {
		m_timeLabel.Text = text;
	}
}
