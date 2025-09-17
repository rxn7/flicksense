using Godot;

public partial class GameManager : Node {
	[Export] private TargetManager m_targetManager;
	[Export] private ShootManager m_shootManager;
	[Export] private SfxManager m_sfxManager;

	private Stats m_stats;

	public override void _Ready() {
		m_shootManager.OnShoot += OnShoot;
		m_shootManager.OnTargetHit += OnTargetHit;
	}

	public override void _UnhandledKeyInput(InputEvent ev) {
		if(ev is not InputEventKey key) {
			return;
		}

		if(key.IsPressed() && !key.Echo && key.Keycode == Key.R) {
			Restart();
		}
	}

	private void Restart() {
		m_stats = new Stats();
		m_targetManager.InitTargetGrid(false);
	}
	
	private void OnShoot(bool hit) {
		++m_stats.Shots;

		if(hit) {
			m_sfxManager.PlaySfx(Sfx.ShootHit, (float)GD.RandRange(0.8f, 1.2f));
		} else {
			m_sfxManager.PlaySfx(Sfx.ShootMiss, (float)GD.RandRange(0.8f, 1.2f));
		}
	}

	private void OnTargetHit(Target target) {
		++m_stats.Hits;

		m_targetManager.ShowRandomTarget();
		target.Hide();

		GD.Print($"Shots: {m_stats.Shots}, Hits: {m_stats.Hits}, Acc: {m_stats.Accuracy * 100.0f}%");
	}
}
