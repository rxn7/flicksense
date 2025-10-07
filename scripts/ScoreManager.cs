using Godot;
using System;

public partial class ScoreManager : Node {
	public event Action onUpdated;
	public event Action<ulong, ulong> onScoreAdded;
	public event Action<float> onStreakMultiplierChanged;

	public const int BASE_HIT_POINTS = 10;

	public const int MAX_ELAPSED_MS = 500;
	public const int MIN_ELAPSED_MS = 100;
	public const int MAX_TIME_BONUS = 50;
	public const int MIN_TIME_BONUS = 10;

	public const float MAX_STREAK_MULTIPLIER = 5.0f;
	public const ulong STREAK_REDUCTION_INTERVAL_MS = 100;
	public const float STREAK_REDUCTION_AMOUNT = 0.01f;

	private float m_streakMultiplier = 1.0f;
	private ulong m_lastHitTimeMs = 0;
	private ulong m_streakReductionTime = 0;

	public float StreakMultiplier => m_streakMultiplier;

	public override void _Ready() {
		Reset();
	}

	public override void _PhysicsProcess(double delta) {
		ulong nowMs = Time.GetTicksMsec();

		if(nowMs >= m_streakReductionTime) {
			m_streakReductionTime = nowMs + STREAK_REDUCTION_INTERVAL_MS;
			m_streakMultiplier = Mathf.Max(m_streakMultiplier - STREAK_REDUCTION_AMOUNT, 1.0f);
			onStreakMultiplierChanged?.Invoke(m_streakMultiplier);
		}
	}

	public void RegisterShot(bool hit, ref Stats stats) {
		++stats.Shots;

		if(!hit) {
			m_streakMultiplier = 1.0f;
			onStreakMultiplierChanged?.Invoke(m_streakMultiplier);
		}

		onUpdated?.Invoke();
	}

	public (ulong, float) RegisterHit(ref Stats stats) {
		++stats.Hits;

		ulong nowMs = Time.GetTicksMsec();

		float reactionTimeRatio = (float)(nowMs - m_lastHitTimeMs) / (MAX_ELAPSED_MS - MIN_ELAPSED_MS);

		ulong baseScore = BASE_HIT_POINTS + CalculateReactionTimeBonus(nowMs);
		ulong addedScore = (ulong)(baseScore * m_streakMultiplier);

		stats.Score += addedScore;

		onScoreAdded?.Invoke(stats.Score, addedScore);
		onUpdated?.Invoke();

		m_streakMultiplier = Mathf.Min(m_streakMultiplier + 0.1f, MAX_STREAK_MULTIPLIER);
		onStreakMultiplierChanged?.Invoke(m_streakMultiplier);

		m_lastHitTimeMs = nowMs;

		return (addedScore, reactionTimeRatio);
	}

	public void Reset() {
		m_streakMultiplier = 1.0f;
		m_streakReductionTime = Time.GetTicksMsec() + STREAK_REDUCTION_INTERVAL_MS;
		m_lastHitTimeMs = 0;
		onUpdated?.Invoke();
		onStreakMultiplierChanged?.Invoke(m_streakMultiplier);
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
		return timeBonus;
	}
}
