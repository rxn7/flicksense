using Godot;

[GlobalClass]
public partial class CrosshairUI : CenterContainer {
	private CrosshairType m_type = CrosshairType.Classic;
	private CrosshairData m_data = new ClassicCrosshairData();

	public override void _Ready() {
		m_type = SettingsManager.settings.crosshairType;
		m_data = SettingsManager.settings.crosshairData;
	}

	public override void _Draw() {
		switch(m_type) {
			case CrosshairType.Classic:
				RenderClassic((ClassicCrosshairData)m_data);
				break;

			case CrosshairType.Dot:
				RenderDot((DotCrosshairData)m_data);
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
