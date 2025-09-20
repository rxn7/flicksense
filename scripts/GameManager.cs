using Godot;

public partial class GameManager : Node {
	[Export] private TargetManager m_targetManager;
	[Export] private ShootManager m_shootManager;
	[Export] private SfxManager m_sfxManager;
	[Export] private VfxManager m_vfxManager;
	[Export] private StatsUI m_statsUI;
	[Export] private ScoreManager m_scoreManager;

	private ulong m_startTimeMs = 0;

	public override void _Ready() {
		m_shootManager.onShoot += OnShoot;
		m_shootManager.onTargetHit += OnTargetHit;

		m_scoreManager.updated += () => m_statsUI.UpdateStats(m_scoreManager);
		m_scoreManager.streakMultiplierChanged += m_statsUI.UpdateHitStreakMultiplier;

		Reset();
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
		m_startTimeMs = Time.GetTicksMsec();
		m_statsUI.SetStartTime(m_startTimeMs);

		m_targetManager.Reset();
		m_scoreManager.Reset();
	}
	
	private void OnShoot(bool hit, Vector3 hitPoint) {
		m_scoreManager.OnShoot(hit);

		if(!hit) {
			VfxManager vfxManager = m_vfxManager;
			vfxManager.ShowMissPopup(hitPoint);
			
			m_sfxManager.PlaySfx(Sfx.ShootMiss, (float)GD.RandRange(0.8f, 1.2f));
		} 
	}

	private void OnTargetHit(Target target, Vector3 shootDir, Vector3 hitPoint, Vector3 hitNormal) {
		ulong scoreAdded = m_scoreManager.OnHit();

		m_vfxManager.ShowScorePopup(hitPoint + hitNormal * 0.2f, scoreAdded);
		m_vfxManager.ExplodeTarget(target.GlobalPosition, hitPoint, shootDir);

		m_sfxManager.PlaySfx(Sfx.ShootHit, (float)GD.RandRange(0.8f, 1.2f));

		int gridIdx = target.gridIdx;
		m_targetManager.HideTarget(target);
		m_targetManager.ShowRandomTarget(gridIdx);
	}
}
