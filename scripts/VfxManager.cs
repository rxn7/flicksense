using Godot;

public partial class VfxManager : Node {
	[Export] private PackedScene m_destroyedTargetPrefab;

	public void ExplodeTarget(Vector3 targetPosition, Vector3 hitPoint, Vector3 hitNormal) {
		DestroyedTarget destroyedTarget = (DestroyedTarget)m_destroyedTargetPrefab.Instantiate();
		AddChild(destroyedTarget);
		destroyedTarget.Explode(targetPosition, hitPoint, hitNormal);
		destroyedTarget.finished += () => {
			destroyedTarget.QueueFree();
		};
	}
}
