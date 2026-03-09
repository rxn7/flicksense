using Godot;

[GlobalClass]
public partial class CrosshairUI : CenterContainer {
	public override void _Draw() {
		switch(SettingsManager.settings.crosshairType) {
			case CrosshairType.Classic:
				RenderClassic((ClassicCrosshairData)SettingsManager.settings.crosshairData);
				break;

			case CrosshairType.Dot:
				RenderDot((DotCrosshairData)SettingsManager.settings.crosshairData);
				break;
		}
	}

	private void RenderClassic(ClassicCrosshairData data) {
		Vector2 center = Size * 0.5f;

		int innerThickness = Mathf.RoundToInt(data.thickness);
		int outterThickness = Mathf.RoundToInt(data.outlineThickness);
		int totalThickness = innerThickness + outterThickness * 2;

		if(totalThickness % 2 != 0) {
			center += new Vector2(0.5f, 0.5f);
		}

		if(data.outlineThickness > 0.0f) {
			float start = data.gap - data.outlineThickness;
			float end = data.gap + data.length + data.outlineThickness;

			// Right Outline
			DrawLine(
				center + new Vector2(start, 0.0f),
				center + new Vector2(end, 0.0f),
				Colors.Black, totalThickness
			);

			// Left Outline
			DrawLine(
				center - new Vector2(start, 0.0f),
				center - new Vector2(end, 0.0f),
				Colors.Black, totalThickness
			);

			// Bottom Outline
			DrawLine(
				center + new Vector2(0.0f, start),
				center + new Vector2(0.0f, end),
				Colors.Black, totalThickness
			);

			// Top Outline
			DrawLine(
				center - new Vector2(0.0f, start),
				center - new Vector2(0.0f, end),
				Colors.Black, totalThickness
			);
		}

		// Right
		DrawLine(
			center + new Vector2(data.gap, 0.0f),
			center + new Vector2(data.gap + data.length, 0.0f),
			data.color,
			data.thickness
		);

		// Left
		DrawLine(
			center - new Vector2(data.gap, 0.0f),
			center - new Vector2(data.gap + data.length, 0.0f),
			data.color,
			data.thickness
		);

		// Bottom
		DrawLine(
			center + new Vector2(0.0f, data.gap),
			center + new Vector2(0.0f, data.gap + data.length),
			data.color,
			data.thickness
		);

		// Top
		DrawLine(
			center - new Vector2(0.0f, data.gap),
			center - new Vector2(0.0f, data.gap + data.length),
			data.color,
			data.thickness
		);
	}

	private void RenderDot(DotCrosshairData data) {
		if(data.outlineThickness > 0.0f) {
			DrawCircle(Size * 0.5f, data.radius + data.outlineThickness, Colors.Black);
		}

		DrawCircle(Size * 0.5f, data.radius, data.color);
	}
}

public enum CrosshairType : byte {
	Classic,
	Dot,
}

public abstract class CrosshairData {
	public Color color = Colors.Green;
	public float outlineThickness = 1.0f;

	public override string ToString() {
		return $"{color.R8} {color.G8} {color.B8} {outlineThickness}";
	}
}

public class DotCrosshairData : CrosshairData {
	public float radius = 2.0f;

	public override string ToString() {
		return base.ToString() + $" {radius}";
	}
}

public class ClassicCrosshairData : CrosshairData {
	public float length = 5.0f;
	public float thickness = 1.5f;
	public float gap = 2.0f;

	public override string ToString() {
		return base.ToString() + $" {length} {thickness} {gap}";
	}
}
