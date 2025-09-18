using Godot;

public class ScoreManager {
	private const int BASE_HIT_POINTS = 10;

	private const int MAX_ELAPSED_MS = 500;
	private const int MIN_ELAPSED_MS = 100;
	private const int MAX_TIME_BONUS = 50;
	private const int MIN_TIME_BONUS = 10;

	private const int MAX_STREAK = 30;
	private const int MIN_STREAK = 1;
	private const float MAX_STREAK_MULTIPLIER = 3.0f;
	private const float MIN_STREAK_MULTIPLIER = 1.0f;

	private Stats m_stats;
	private int m_hitStreak = 0;
	private ulong m_lastHitTime = 0;

	public ScoreManager() {
		Reset();
	}

	public ref Stats GetStats() {
		return ref m_stats;
	}

	public void OnShoot(bool hit) {
		++m_stats.Shots;

		if(!hit) {
			m_hitStreak = 0;
			return;
		}

		++m_hitStreak;
		++m_stats.Hits;

		ulong nowMs = Time.GetTicksMsec();

		m_stats.Score += (ulong)((BASE_HIT_POINTS + CalculateReactionTimeBonus(nowMs)) * CalculateStreakMultiplier());
		m_lastHitTime = nowMs;
	}

	public void Reset() {
		m_stats = new Stats();
	}

	public float CalculateStreakMultiplier() {
		if(m_hitStreak <= MIN_STREAK) {
			return 1.0f;
		} else if(m_hitStreak >= MAX_STREAK) {
			return MAX_STREAK_MULTIPLIER;
		} else {
			return MIN_STREAK_MULTIPLIER + (m_hitStreak - MIN_STREAK) * (MAX_STREAK_MULTIPLIER - MIN_STREAK_MULTIPLIER) / (MAX_STREAK - MIN_STREAK);
		}
	}
	
	private ulong CalculateReactionTimeBonus(ulong nowMs) {
		ulong timeBonus = 0;
		if(m_lastHitTime != 0) {
			ulong elapsedMs = nowMs - m_lastHitTime;
			if(elapsedMs <= MIN_ELAPSED_MS) {
				timeBonus = MAX_TIME_BONUS;
			} else if(elapsedMs <= MAX_ELAPSED_MS) {
				timeBonus = MAX_TIME_BONUS - (elapsedMs - MIN_ELAPSED_MS) * (MAX_TIME_BONUS - MIN_TIME_BONUS) / (MAX_ELAPSED_MS - MIN_ELAPSED_MS);
			}
		}
		return 10*((timeBonus + 9)/10);
	}
}
