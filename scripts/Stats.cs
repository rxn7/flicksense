public struct Stats {
	public int Shots;
	public int Hits;

	public int Misses => Shots - Hits;
	public float Accuracy => Shots > 0 ? (float)Hits / Shots : 0.0f;

	public Stats() {
		Shots = 0;
		Hits = 0;
	}
};
