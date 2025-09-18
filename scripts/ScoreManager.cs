public class ScoreManager {
	private Stats m_stats;

	public ScoreManager() {
		Reset();
	}

	public ref Stats GetStats() {
		return ref m_stats;
	}

	public void OnShoot(bool hit) {
		++m_stats.Shots;
		if(hit) {
			++m_stats.Hits;
		}
	}

	public void Reset() {
		m_stats = new Stats();
	}
}
