using Godot;

public partial class MainMenuStatsDisplay : Control {
	[ExportGroup("General")]
	[Export] private Label m_averageAccuracyLabel;
	[Export] private Label m_bestStreakLabel;

	[ExportGroup("Endless Mode")]
	[Export] private Label m_averageScoreLabel;
	[Export] private Label m_bestScore;

	[ExportGroup("Surival Mode")]
	[Export] private Label m_longestSurvivedLabel;
}
