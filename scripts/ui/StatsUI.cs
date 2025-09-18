using Godot;

public partial class StatsUI : Control {
	[Export] private Label m_missesLabel;
	[Export] private Label m_hitsLabel;
	[Export] private Label m_accuracyLabel;

	public void UpdateStats(ref Stats stats) {
		m_missesLabel.Text = stats.Misses.ToString();
		m_hitsLabel.Text = stats.Hits.ToString();
		m_accuracyLabel.Text = $"{(stats.Accuracy * 100.0f):0}%";
	}
}
