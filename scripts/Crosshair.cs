using Godot;
using System;

public partial class Crosshair : CenterContainer {
	[Export] private float m_dotRadius = 3.0f;

	public override void _Draw() {
		DrawCircle(Size * 0.5f, m_dotRadius, Colors.White);
	}
}
