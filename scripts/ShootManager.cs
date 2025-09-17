using Godot;
using System;

[GlobalClass]
public partial class ShootManager : Node {
	[Export(PropertyHint.Layers3DPhysics)] private uint m_rayCollisionMask;
	[Export(PropertyHint.Layers3DPhysics)] private CameraManager m_camera;

	public event Action<Target> OnTargetHit;

	private bool m_shootQueued = false;

	public override void _UnhandledInput(InputEvent ev) {
		if(ev is not InputEventMouseButton button) {
			return;
		}

		if(button.IsPressed() && button.ButtonIndex == MouseButton.Left) {
			m_shootQueued = true;
		}
	}

	public override void _PhysicsProcess(double delta) {
		if(m_shootQueued) {
			Shoot();
		}
	}

	private void Shoot() {
		m_shootQueued = false;

		PhysicsDirectSpaceState3D spaceState = m_camera.GetWorld3D().DirectSpaceState;
		PhysicsRayQueryParameters3D parameters = PhysicsRayQueryParameters3D.Create(m_camera.GlobalPosition, m_camera.GlobalPosition - m_camera.GlobalTransform.Basis.Z * 100.0f, m_rayCollisionMask);
		parameters.CollideWithAreas = true;
		parameters.CollideWithBodies = false;

		Godot.Collections.Dictionary result = spaceState.IntersectRay(parameters);

		GD.Print(result);

		if(result.Count == 0) {
			return;
		}

		if(result["collider"].AsGodotObject() is Target target) {
			GD.Print(target.Name);
			if(target.Visible) {
				OnTargetHit?.Invoke(target);
			}
		}
	}

}
