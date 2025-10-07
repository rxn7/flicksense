using Godot;

public partial class GameManager : Node {
	public const float TIME_LIMIT_SECONDS = 30.0f; // TODO: Configurable time limit

	[ExportGroup("Managers")]
	[Export] private TargetManager m_targetManager;
	[Export] private ShootManager m_shootManager;
	[Export] private SfxManager m_sfxManager;
	[Export] private VfxManager m_vfxManager;
	[Export] private ScoreManager m_scoreManager;
	[Export] private PlayerManager m_playerManager;

	[ExportGroup("UI")]
	[Export] private HudUI m_hud;
	[Export] private EndScreenUI m_endScreen;
	[Export] private PauseMenuUI m_pauseMenu;

	private Stats m_stats;
	private EGameMode m_gameMode;
	private bool m_isFinished = false;

	public override void _Ready() {
		Global.Instance.ConsoleUI.onHide += () => {
			Input.MouseMode = Input.MouseModeEnum.Captured;
		};

		m_pauseMenu.Setup(m_gameMode);

		m_shootManager.onShoot += OnShoot;
		m_shootManager.onTargetHit += OnTargetHit;

		m_scoreManager.onUpdated += () => m_hud.UpdateStats(m_stats);
		m_scoreManager.onScoreAdded += m_hud.UpdateScore;
		m_scoreManager.onStreakMultiplierChanged += m_hud.UpdateHitStreakMultiplier;

		m_endScreen.onPlayAgainPressed += () => {
			GetTree().Paused = false;
			Reset();
		};

		m_pauseMenu.onFinishSessionPressed += () => {
			Input.MouseMode = Input.MouseModeEnum.Visible;
			Finish();
		};

		m_pauseMenu.onClosed += () => {
			GetTree().Paused = false;
			Input.MouseMode = Input.MouseModeEnum.Captured;
		};

		m_pauseMenu.onOpen += () => {
			GetTree().Paused = true;
			Input.MouseMode = Input.MouseModeEnum.Visible;
		};

		Reset();
	}

	public override void _Process(double delta) {
		if(m_isFinished) {
			return;
		}

		m_stats.timeElapsed += (float)delta;

		switch(m_gameMode) {
			case EGameMode.Endless:
				break;

			case EGameMode.TimeLimit:
				m_stats.timeElapsed = Mathf.Min(m_stats.timeElapsed, TIME_LIMIT_SECONDS);
				if(m_stats.timeElapsed >= TIME_LIMIT_SECONDS) {
					Finish();
				}
				break;

			case EGameMode.Survival:
				break;
		}

		m_hud.UpdateTimeText(m_gameMode, m_stats.timeElapsed);
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
		Input.MouseMode = Input.MouseModeEnum.Captured;

		m_stats = new Stats();
		m_isFinished = false;

		m_hud.Reset();

		m_targetManager.Reset();
		m_scoreManager.Reset();
		m_playerManager.Reset();
	}

	private void Finish() {
		GetTree().Paused = true;

		m_pauseMenu.Hide();
		m_isFinished = true;
		m_endScreen.ShowEndScreen(m_gameMode, m_stats);

		switch(m_gameMode) {
			case EGameMode.TimeLimit:
				if(SaveManager.data.bestTimeLimitScore < m_stats.score) {
					SaveManager.data.bestTimeLimitScore = m_stats.score;
				}
				break;
		}

		SaveManager.Save();
	}
	
	private void OnShoot(bool hit, Vector3? hitPoint, Vector3? hitNormal) {
		m_scoreManager.RegisterShot(hit, ref m_stats);

		if(hitPoint.HasValue && hitNormal.HasValue) {
			if(!hit) {
				m_vfxManager.ShowScorePopup(hitPoint.Value, 0);
			}

			m_sfxManager.PlaySfx(hit ? ESfx.ShootHit : ESfx.ShootMiss, (float)GD.RandRange(0.8f, 1.2f));
		} 
	}

	private void OnTargetHit(Target target, Vector3 shootDir, Vector3 hitPoint, Vector3 hitNormal) {
		(ulong scoreAdded, float reactionTimeRatio) = m_scoreManager.RegisterHit(ref m_stats);
		m_vfxManager.ShowScorePopup(hitPoint, scoreAdded, reactionTimeRatio);

		m_vfxManager.ExplodeTarget(target.GlobalPosition, hitPoint, shootDir);

		m_targetManager.HideTarget(target);
		m_targetManager.ShowTarget();
	}
}
