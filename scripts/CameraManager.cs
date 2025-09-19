using Godot;

public partial class CameraManager : Camera3D {
	private float m_pitch = 0.0f;
	private float m_yaw = 0.0f;

	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Input.UseAccumulatedInput = false;
	}

	public override void _Input(InputEvent ev) {
		if(ev is not InputEventMouseMotion motion) {
			return;
		}

		float sens = Global.Instance.settings.sensitivity;

		m_pitch -= motion.ScreenRelative.Y * sens * 0.022f; // 0.022 is a CS:GO/CS2 constant
		m_yaw -= motion.ScreenRelative.X * sens * 0.022f;

		m_pitch = Mathf.Clamp(m_pitch, -90.0f, 90.0f);
		m_yaw = Mathf.PosMod(m_yaw, 360.0f);

		GlobalRotationDegrees = Vector3.Right * m_pitch + Vector3.Up * m_yaw;
	}
}
