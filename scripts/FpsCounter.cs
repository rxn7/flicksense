using Godot;

public partial class FpsCounter : Label {
	public override void _PhysicsProcess(double delta) {
		Text = $"fps: {Engine.GetFramesPerSecond()}";
	}
}
