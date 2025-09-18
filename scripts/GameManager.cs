using Godot;

public partial class GameManager : Node {
	[Export] private TargetManager m_targetManager;
	[Export] private ShootManager m_shootManager;
	[Export] private SfxManager m_sfxManager;
	[Export] private StatsUI m_statsUI;

	private ScoreManager m_scoreManager = new();

	public override void _Ready() {
		m_shootManager.OnShoot += OnShoot;
		m_shootManager.OnTargetHit += OnTargetHit;
		UpdateStatsUI();
	}

	public override void _UnhandledKeyInput(InputEvent ev) {
		if(ev is not InputEventKey key) {
			return;
		}

		if(key.IsPressed() && !key.Echo && key.Keycode == Key.R) {
			Reset();
		}
	}

	private void Reset() {
		m_targetManager.Reset();
		m_scoreManager.Reset();
		UpdateStatsUI();
	}
	
	private void OnShoot(bool hit) {
		m_scoreManager.OnShoot(hit);

		m_sfxManager.PlaySfx(hit ? Sfx.ShootHit : Sfx.ShootMiss, (float)GD.RandRange(0.8f, 1.2f));
		UpdateStatsUI();
	}

	private void OnTargetHit(Target target) {
		int gridIdx = target.gridIdx;
		m_targetManager.HideTarget(target);
		m_targetManager.ShowRandomTarget(gridIdx);
	}

	private void UpdateStatsUI() {
		m_statsUI.UpdateStats(m_scoreManager);
	}
}
