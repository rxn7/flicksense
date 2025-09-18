using Godot;
using System;

[GlobalClass]
public partial class ShootManager : Node {
	[Export(PropertyHint.Layers3DPhysics)] private uint m_rayCollisionMask;
	[Export] private CameraManager m_camera;

	public event Action<bool> onShoot;
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
		parameters.CollideWithBodies = false;

		Godot.Collections.Dictionary result = spaceState.IntersectRay(parameters);

		bool hit = false;
		if(result.Count != 0 && result["collider"].AsGodotObject() is Target target && target.Visible) {
			hit = true;
			Vector3 hitPoint = (Vector3)result["position"];
			Vector3 normal = (Vector3)result["normal"];
			onTargetHit?.Invoke(target, direction, hitPoint, normal);
		}

		onShoot?.Invoke(hit);
	}

}
