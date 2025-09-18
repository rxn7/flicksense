using Godot;

public partial class ScoreManager : Node {
	public event System.Action updated;
	public event System.Action<float> streakMultiplierChanged;

	private const int BASE_HIT_POINTS = 10;

	private const int MAX_ELAPSED_MS = 500;
	private const int MIN_ELAPSED_MS = 100;
	private const int MAX_TIME_BONUS = 50;
	private const int MIN_TIME_BONUS = 10;

	private const float MAX_STREAK_MULTIPLIER = 5.0f;
	private const ulong STREAK_REDUCTION_INTERVAL_MS = 50;
	private const float STREAK_REDUCTION_AMOUNT = 0.01f;

	private Stats m_stats;
	private float m_streakMultiplier = 1.0f;
	private ulong m_lastHitTimeMs = 0;
	private ulong m_lastStreakReductionTime = 0;

	public override void _Ready() {
		Reset();
	}

	public override void _PhysicsProcess(double delta) {
		ulong nowMs = Time.GetTicksMsec();

		if(nowMs - m_lastStreakReductionTime >= STREAK_REDUCTION_INTERVAL_MS) {
			m_lastStreakReductionTime = nowMs;
			m_streakMultiplier = Mathf.Max(m_streakMultiplier - STREAK_REDUCTION_AMOUNT, 1.0f);
			streakMultiplierChanged?.Invoke(m_streakMultiplier);
		}
	}

	public ref Stats GetStats() {
		return ref m_stats;
	}

	public void OnShoot(bool hit) {
		++m_stats.Shots;

		if(!hit) {
			m_streakMultiplier = 1.0f;
			updated?.Invoke();
			streakMultiplierChanged?.Invoke(m_streakMultiplier);
			return;
		}

		m_streakMultiplier = Mathf.Min(m_streakMultiplier + 0.1f, MAX_STREAK_MULTIPLIER);
		streakMultiplierChanged?.Invoke(m_streakMultiplier);

		++m_stats.Hits;

		ulong nowMs = Time.GetTicksMsec();
		m_stats.Score += (ulong)((BASE_HIT_POINTS + CalculateReactionTimeBonus(nowMs)) * m_streakMultiplier);
		m_lastHitTimeMs = nowMs;

		updated?.Invoke();
	}

	public void Reset() {
		m_stats = new Stats();
		m_streakMultiplier = 1.0f;
		m_lastStreakReductionTime = Time.GetTicksMsec();
		m_lastHitTimeMs = 0;
		updated?.Invoke();
		streakMultiplierChanged?.Invoke(m_streakMultiplier);
	}

	private ulong CalculateReactionTimeBonus(ulong nowMs) {
		ulong timeBonus = 0;
		if(m_lastHitTimeMs != 0) {
			ulong elapsedMs = nowMs - m_lastHitTimeMs;
			if(elapsedMs <= MIN_ELAPSED_MS) {
				timeBonus = MAX_TIME_BONUS;
			} else if(elapsedMs <= MAX_ELAPSED_MS) {
				timeBonus = MAX_TIME_BONUS - (elapsedMs - MIN_ELAPSED_MS) * (MAX_TIME_BONUS - MIN_TIME_BONUS) / (MAX_ELAPSED_MS - MIN_ELAPSED_MS);
			}
		}
		return 10 * ((timeBonus + 9) / 10);
	}
}
