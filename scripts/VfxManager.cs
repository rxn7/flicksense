using Godot;
using System.Collections.Generic;

public partial class VfxManager : Node {
	[Export] private PackedScene m_destroyedTargetPrefab;

	private const int DESTROYED_TARGET_POOL_SIZE = 10;
	private Stack<DestroyedTarget> m_destroyedTargetPool = new();

	public override void _Ready() {
		for(int i=0; i<DESTROYED_TARGET_POOL_SIZE; ++i) {
			m_destroyedTargetPool.Push(CreateDestroyedTarget(true));
		}
	}

	public void ExplodeTarget(Vector3 targetPosition, Vector3 hitPoint, Vector3 shootDir) {
		DestroyedTarget destroyedTarget;
		if(m_destroyedTargetPool.Count == 0) {
			GD.PushWarning("Destroyed target pool is empty");
			destroyedTarget = CreateDestroyedTarget(false);
		} else {
			destroyedTarget = m_destroyedTargetPool.Pop();
		}

		destroyedTarget.Explode(targetPosition, hitPoint, shootDir);
	}

	private void FreeDestroyedTarget(DestroyedTarget destroyedTarget) {
		m_destroyedTargetPool.Push(destroyedTarget);
	}

	public DestroyedTarget CreateDestroyedTarget(bool returnToPool) {
		DestroyedTarget destroyedTarget = (DestroyedTarget)m_destroyedTargetPrefab.Instantiate();
		AddChild(destroyedTarget);
		destroyedTarget.Visible = false;

		if(returnToPool) {
			destroyedTarget.finished += () => FreeDestroyedTarget(destroyedTarget);
		} else {
			destroyedTarget.finished += () => destroyedTarget.QueueFree();
		}

		return destroyedTarget;
	}
}
