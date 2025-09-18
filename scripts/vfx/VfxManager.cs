using Godot;

public partial class VfxManager : Node {
	[Export] private PackedScene m_scorePopupPrefab;
	[Export] private PackedScene m_destroyedTargetPrefab;
	[Export] private PackedScene m_missPopupPrefab;

	private VfxPool<ScorePopupVfx> m_scorePopupPool;
	private VfxPool<MissPopupVfx> m_missPopupVfx;
	private VfxPool<DestroyedTargetVfx> m_destroyedTargetPool;

	public override void _EnterTree() {
		m_scorePopupPool = new VfxPool<ScorePopupVfx>(this, m_scorePopupPrefab, 10, 20);
		m_destroyedTargetPool = new VfxPool<DestroyedTargetVfx>(this, m_destroyedTargetPrefab, 10, 20);
		m_missPopupVfx = new VfxPool<MissPopupVfx>(this, m_missPopupPrefab, 5, 10);
	}

	public void ShowScorePopup(Vector3 position, ulong scoreAdded) {
		ScorePopupVfx vfx = m_scorePopupPool.Pop();
		vfx.Show(position, scoreAdded);
	}

	public void ShowMissPopup(Vector3 position) {
		MissPopupVfx vfx = m_missPopupVfx.Pop();
		vfx.Show(position);
	}

	public void ExplodeTarget(Vector3 targetPosition, Vector3 hitPoint, Vector3 shootDir) {
		DestroyedTargetVfx vfx = m_destroyedTargetPool.Pop();
		vfx.Explode(targetPosition, hitPoint, shootDir);
	}
}
