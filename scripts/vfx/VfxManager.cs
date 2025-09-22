using Godot;

public partial class VfxManager : Node {
	[Export] private PackedScene m_scorePopupPrefab;
	[Export] private PackedScene m_destroyedTargetPrefab;

	private VfxPool<ScorePopupVfx> m_scorePopupPool;
	private VfxPool<DestroyedTargetVfx> m_destroyedTargetPool;

	public override void _EnterTree() {
		m_scorePopupPool = new VfxPool<ScorePopupVfx>(this, m_scorePopupPrefab, 10, 20);
		m_destroyedTargetPool = new VfxPool<DestroyedTargetVfx>(this, m_destroyedTargetPrefab, 10, 20);
	}

	public void ShowScorePopup(Vector3 position, ulong scoreAdded, float reactionTimeRatio = 1.0f) {
		ScorePopupVfx vfx = m_scorePopupPool.Pop();
		vfx.Show(position, scoreAdded, reactionTimeRatio);
	}

	public void ExplodeTarget(Vector3 targetPosition, Vector3 hitPoint, Vector3 shootDir) {
		DestroyedTargetVfx vfx = m_destroyedTargetPool.Pop();
		vfx.Explode(targetPosition, hitPoint, shootDir);
	}
}
