using Godot;

public partial class FpsCounterUI : Label {
	public override void _EnterTree() {
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _PhysicsProcess(double delta) {
		Text = $"fps: {Engine.GetFramesPerSecond()}";
	}
}
