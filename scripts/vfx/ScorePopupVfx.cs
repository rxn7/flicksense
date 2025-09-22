using Godot;
using System;

public partial class ScorePopupVfx : Label3D, IVfxObject {
	private const ulong DURATION_MS = 500;

	public event Action finished;

	[Export] private Gradient m_colorGradient;
	private ulong m_finishTimeMs = 0;

	public override void _Ready() {
		ProcessMode = ProcessModeEnum.Disabled;
		Visible = false;
	}

	public override void _Process(double delta) {
		if(Time.GetTicksMsec() >= m_finishTimeMs) {
			Visible = false;
			ProcessMode = ProcessModeEnum.Disabled;
			finished?.Invoke();
			return;
		}

		float elapsedRatio = 1.0f - (float)(m_finishTimeMs - Time.GetTicksMsec()) / DURATION_MS;

		Color color = Modulate;
		color.A = 1.0f - elapsedRatio;
		Modulate = color;
	}

	public void Show(Vector3 position, ulong scoreAdded, float reactionTimeRatio = 1.0f) {
		ProcessMode = ProcessModeEnum.Inherit;
		m_finishTimeMs = Time.GetTicksMsec() + DURATION_MS;

		Visible = true;
		GlobalPosition = position;

		if(scoreAdded == 0) {
			Modulate = Colors.Red;
			Text = "X";
		} else {
			Modulate = m_colorGradient.Sample(reactionTimeRatio);
			Text = $"+{scoreAdded}";
		}
	}
}
