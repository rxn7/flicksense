using Godot;

public partial class GameManager : Node {
	[Export] private TargetManager m_targetManager;
	[Export] private ShootManager m_shootManager;
	[Export] private SfxManager m_sfxManager;

	public override void _Ready() {
		m_shootManager.OnTargetHit += OnTargetHit;
	}

	private void OnTargetHit(Target target) {
		m_targetManager.ShowRandomTarget();
		target.Hide();
		m_sfxManager.PlaySfx(Sfx.TargetHit, (float)GD.RandRange(0.8f, 1.2f));
	}
}
