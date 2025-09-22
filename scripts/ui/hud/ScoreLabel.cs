using Godot;

public partial class ScoreLabel : Label {
	[Export] private float m_scaleSpringFreq = 2.0f;
	[Export] private float m_rotationSpringFreq = 1.5f;

	[Export] private float m_scaleStrength = 50.0f;
	[Export] private float m_rotationStrength = 300.5f;

	private RandomNumberGenerator m_rng = new();

	private float m_scaleVelocity;
	private float m_rotationVelocity;

	public override void _Process(double delta) {
		float scale = Spring(Scale.X, 1.0f, ref m_scaleVelocity, m_scaleSpringFreq, (float)delta); 
		Scale = Vector2.One * scale;

		RotationDegrees = Spring(RotationDegrees, 0.0f, ref m_rotationVelocity, m_rotationSpringFreq, (float)delta);
	}

	public void Reset() {
		Text = "0";

		m_scaleVelocity = 0.0f;
		m_rotationVelocity = 0.0f;

		Scale = Vector2.One;
		RotationDegrees = 0.0f;
	}

	public void UpdateScore(ulong score, ulong scoreAdded) {
		Text = score.ToString();

		if(scoreAdded == 0) {
			return;
		}

		float maxScoreAdded = (ScoreManager.BASE_HIT_POINTS + ScoreManager.MAX_TIME_BONUS) * ScoreManager.MAX_STREAK_MULTIPLIER;
		float intensity = Mathf.Clamp((float)scoreAdded / maxScoreAdded, 0.0f, 1.0f);

		m_scaleVelocity += intensity * m_scaleStrength;

		float sign = m_rng.RandiRange(0, 1) * 2.0f - 1.0f;
		m_rotationVelocity += intensity * m_rotationStrength * sign;
	}

	private static float Spring(float current, float target, ref float velocity, float freq, float dt) {
		float f = freq * 2f * Mathf.Pi;
		float g = 1f / (1f + f * dt + 0.5f * (f * dt) * (f * dt));
		float x = current - target;
		float temp = (velocity + f * x) * dt;
		velocity = (velocity - f * temp) * g;
		return target + (x + temp) * g;
	}
}
