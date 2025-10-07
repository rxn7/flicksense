using Godot;

public partial class GameManager : Node {
	public const int TIME_LIMIT_MS = 10_000;

	[Export] private TargetManager m_targetManager;
	[Export] private ShootManager m_shootManager;
	[Export] private SfxManager m_sfxManager;
	[Export] private VfxManager m_vfxManager;
	[Export] private Hud m_hud;
	[Export] private ScoreManager m_scoreManager;

	private EGameMode m_gameMode;
	private ulong m_startTimeMs = 0;

	public override void _Ready() {
		Global.Instance.ConsoleUI.onHide += () => {
			Input.MouseMode = Input.MouseModeEnum.Captured;
		};

		m_shootManager.onShoot += OnShoot;
		m_shootManager.onTargetHit += OnTargetHit;

		m_scoreManager.onUpdated += () => m_hud.UpdateStats(m_scoreManager);
		m_scoreManager.onScoreAdded += m_hud.UpdateScore;
		m_scoreManager.onStreakMultiplierChanged += m_hud.UpdateHitStreakMultiplier;

		Reset();
	}

	public override void _Process(double delta) {
		switch(m_gameMode) {
			case EGameMode.Endless:
				break;
			case EGameMode.TimeLimit:
				if(Time.GetTicksMsec() - m_startTimeMs >= TIME_LIMIT_MS) {
					// TODO: End screen + high score saving
					Reset();
				}
				break;
			case EGameMode.Survival:
				break;
		}

		m_hud.UpdateTimeText(m_gameMode);
	}

	public override void _UnhandledKeyInput(InputEvent ev) {
		if(ev is not InputEventKey key) {
			return;
		}

		if(key.IsPressed() && !key.Echo && key.Keycode == Key.R) {
			Reset();
		}
	}

	public void Setup(EGameMode gameMode) {
		m_gameMode = gameMode;
	}

	private void Reset() {
		m_startTimeMs = Time.GetTicksMsec();
		m_hud.Reset(m_startTimeMs);

		m_targetManager.Reset();
		m_scoreManager.Reset();
	}
	
	private void OnShoot(bool hit, Vector3? hitPoint, Vector3? hitNormal) {
		m_scoreManager.RegisterShot(hit);

		if(hitPoint.HasValue && hitNormal.HasValue) {
			if(!hit) {
				m_vfxManager.ShowScorePopup(hitPoint.Value, 0);
			}

			m_sfxManager.PlaySfx(hit ? ESfx.ShootHit : ESfx.ShootMiss, (float)GD.RandRange(0.8f, 1.2f));
		} 
	}

	private void OnTargetHit(Target target, Vector3 shootDir, Vector3 hitPoint, Vector3 hitNormal) {
		(ulong scoreAdded, float reactionTimeRatio) = m_scoreManager.RegisterHit();
		m_vfxManager.ShowScorePopup(hitPoint, scoreAdded, reactionTimeRatio);

		m_vfxManager.ExplodeTarget(target.GlobalPosition, hitPoint, shootDir);

		m_targetManager.HideTarget(target);
		m_targetManager.ShowTarget();
	}
}
