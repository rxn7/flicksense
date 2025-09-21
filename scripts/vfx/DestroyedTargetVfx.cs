using Godot;
using System;

public partial class DestroyedTargetVfx : GpuParticles3D, IVfxObject {
	public event Action finished;

	private RandomNumberGenerator m_rng;
	private ulong m_finishTimeMs;

	public override void _Ready() {
		Reset();
		Visible = false;
		m_rng = new();
	}

	public override void _PhysicsProcess(double delta) {
		if(Time.GetTicksMsec() >= m_finishTimeMs) {
			Reset();
			finished?.Invoke();
		}
	}

	public void Explode(Vector3 targetPosition, Vector3 hitPoint, Vector3 shootDir) {
		ProcessMode = ProcessModeEnum.Inherit;
		GlobalPosition = targetPosition;
		m_finishTimeMs = Time.GetTicksMsec() + (ulong)(Lifetime * 1000);

		Restart();
		Emitting = true;

		Visible = true;
	} 

	public void Reset() {
		ProcessMode = ProcessModeEnum.Disabled;
		Emitting = false;
		Visible = false;
	}
}
