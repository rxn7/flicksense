using Godot;

public partial class GameManager : Node {
	[Export] private TargetManager m_targetManager;
	[Export] private ShootManager m_shootManager;

	public override void _Ready() {
		m_shootManager.OnTargetHit += OnTargetHit;
	}

	private void OnTargetHit(Target target) {
		target.Hide();
		m_targetManager.ShowRandomTarget();
	}
}
