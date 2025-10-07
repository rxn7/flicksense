public struct Stats {
	public ulong Score;
	public int Shots;
	public int Hits;

	public float TimeElapsed;

	public int Misses => Shots - Hits;
	public float Accuracy => Shots > 0 ? (float)Hits / Shots : 0.0f;

	public Stats() {
		Shots = 0;
		Hits = 0;
		TimeElapsed = 0f;
	}
};
