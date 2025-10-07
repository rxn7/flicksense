public struct SaveData {
	public ulong bestHitStreak;
	public float bestStreakMultiplier;
	public ulong bestTimeLimitScore;

	public static SaveData Default => new() {
		bestHitStreak = 0,
		bestStreakMultiplier = 1.0f,
		bestTimeLimitScore = 0
	};
}
