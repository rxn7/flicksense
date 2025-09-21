using Godot;
using System;

[GlobalClass]
public partial class ShootManager : Node {
	[Export(PropertyHint.Layers3DPhysics)] private uint m_rayCollisionMask;
	[Export] private CameraManager m_camera;

	public event Action<bool, Vector3?, Vector3?> onShoot;
	public event Action<Target, Vector3, Vector3, Vector3> onTargetHit;

	public override void _UnhandledInput(InputEvent ev) {
		if(ev is not InputEventMouseButton button) {
			return;
		}

		if(button.IsPressed() && button.ButtonIndex == MouseButton.Left) {
			Shoot();
		}
	}

	private void Shoot() {
		Vector3 direction = -m_camera.GlobalTransform.Basis.Z;

		PhysicsDirectSpaceState3D spaceState = m_camera.GetWorld3D().DirectSpaceState;
		PhysicsRayQueryParameters3D parameters = PhysicsRayQueryParameters3D.Create(m_camera.GlobalPosition, m_camera.GlobalPosition + direction * 100.0f, m_rayCollisionMask);
		parameters.CollideWithAreas = true;
		parameters.CollideWithBodies = true;

		Godot.Collections.Dictionary result = spaceState.IntersectRay(parameters);

		Vector3? hitPoint = null;
		Vector3? hitNormal = null;

		bool targetHit = false;
		if(result.Count != 0) {
			hitPoint = (Vector3)result["position"];
			hitNormal = (Vector3)result["normal"];

			if(result["collider"].AsGodotObject() is Target target && target.Visible) {
				targetHit = true;

				onTargetHit?.Invoke(target, direction, hitPoint.Value, hitNormal.Value);
			}
		}

		onShoot?.Invoke(targetHit, hitPoint, hitNormal);
	}

}
