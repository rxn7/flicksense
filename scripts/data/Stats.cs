public struct Stats {
	public ulong score;
	public ulong shots;
	public ulong hits;
	public ulong hitStreak;

	public float timeElapsed;

	public ulong Misses => shots - hits;
	public float Accuracy => shots > 0 ? (float)hits / shots : 0.0f;

	public Stats() {
		shots = 0;
		hits = 0;
		timeElapsed = 0f;
	}
};
