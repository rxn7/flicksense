using Godot;
using System;

public partial class ScorePopupVfx : Label3D, IVfxObject {
	public event Action finished;
	private const ulong DURATION_MS = 500;

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

		Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f - elapsedRatio);
		OutlineModulate = new Color(0.0f, 0.0f, 0.0f, 1.0f - elapsedRatio);
	}

	public void Show(Vector3 position, ulong scoreAdded) {
		ProcessMode = ProcessModeEnum.Inherit;
		m_finishTimeMs = Time.GetTicksMsec() + DURATION_MS;

		Modulate = Colors.White;
		OutlineModulate = Colors.Black;

		Visible = true;
		GlobalPosition = position;
		Text = $"+{scoreAdded}";
	}
}
