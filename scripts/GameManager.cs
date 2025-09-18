using Godot;

public partial class GameManager : Node {
	[Export] private TargetManager m_targetManager;
	[Export] private ShootManager m_shootManager;
	[Export] private SfxManager m_sfxManager;
	[Export] private VfxManager m_vfxManager;
	[Export] private StatsUI m_statsUI;
	[Export] private ScoreManager m_scoreManager;

	public override void _Ready() {
		m_shootManager.onShoot += OnShoot;
		m_shootManager.onTargetHit += OnTargetHit;

		m_scoreManager.updated += () => m_statsUI.UpdateStats(m_scoreManager);
		m_scoreManager.streakMultiplierChanged += m_statsUI.UpdateHitStreakMultiplier;
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
	}
	
	private void OnShoot(bool hit) {
		m_scoreManager.OnShoot(hit);
		m_sfxManager.PlaySfx(hit ? Sfx.ShootHit : Sfx.ShootMiss, (float)GD.RandRange(0.8f, 1.2f));
	}

	private void OnTargetHit(Target target, Vector3 shootDir, Vector3 hitPoint, Vector3 hitNormal) {
		m_vfxManager.ExplodeTarget(target.GlobalPosition, hitPoint, shootDir);

		int gridIdx = target.gridIdx;
		m_targetManager.HideTarget(target);
		m_targetManager.ShowRandomTarget(gridIdx);
	}
}
