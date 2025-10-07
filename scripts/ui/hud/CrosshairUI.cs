using Godot;

public partial class CrosshairUI : CenterContainer {
	[Export] private float m_dotRadius = 1.5f;

	public override void _Draw() {
		DrawCircle(Size * 0.5f, m_dotRadius, Colors.Green);
	}
}
