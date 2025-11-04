using Godot;

public partial class SurvivalHealthUI : Control {
	[Export] private Label m_label;
	[Export] private ProgressBar m_progressBar;

	public void SetValue(float value) {
		m_label.Text = value.ToString("0");
		m_progressBar.Value = value;
	}
}
