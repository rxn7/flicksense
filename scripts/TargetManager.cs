using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class TargetManager : Node3D {
	private const int MAX_VISIBLE_TARGETS = 5;

	[Export(PropertyHint.Layers3DPhysics)] private uint m_spawnCheckCollisionMask;
	[Export] private float m_minSpawnDistance = 2.0f;
	[Export] private CollisionShape3D m_spawnAreaShapeOwner;
	[Export] private PackedScene m_targetPrefab;
	[Export] private Mesh m_targetMesh;

	private RandomNumberGenerator m_rng = new();
	private MultiMeshInstance3D m_multiMeshInstance;
	private BoxShape3D m_spawnAreaShape;
	private SphereShape3D m_targetShape;

	private Target[] m_targets = new Target[MAX_VISIBLE_TARGETS];
	private Stack<Target> m_freeTargets = new(MAX_VISIBLE_TARGETS);
	
	private Vector3 m_lastTargetPosition;

	public override void _Ready() {
		m_spawnAreaShape = m_spawnAreaShapeOwner.Shape as BoxShape3D;
		m_lastTargetPosition = m_spawnAreaShapeOwner.GlobalPosition;

		InitMultiMesh();

		InitTargets();
		Reset();
	}

	public void HideTarget(Target target) {
		m_freeTargets.Push(target);
		target.Reset();
	}

	public void Reset() {
		m_freeTargets.Clear();
		foreach(Target target in m_targets) {
			target.Reset();
			m_freeTargets.Push(target);
		}

		ShowTargets();
	}


	public void ShowTarget() {
		Vector3 pos;

		int tries = 0;
		do {
			pos = GetNextPosition();
			++tries;

			if(tries > 10) {
				Logger.Warn("Unable to find a free position for target");
				break;
			}
		} while(!IsPositionFree(pos));

		Target target = m_freeTargets.Pop();
		target.GlobalPosition = pos;
		target.Visible = true;

		m_multiMeshInstance.Multimesh.SetInstanceTransform(target.multiMeshIdx, target.GlobalTransform);
	}

	private void ShowTargets() {
		while (m_freeTargets.Count > 0) {
			ShowTarget();
		}
	}

	private Vector3 GetNextPosition() {
		Aabb targetAabb = m_targetMesh.GetAabb();
		float w = m_spawnAreaShape.Size.X - targetAabb.Size.X;
		float h = m_spawnAreaShape.Size.Y - targetAabb.Size.Y;
		float d = m_spawnAreaShape.Size.Z - targetAabb.Size.Z;

		return m_spawnAreaShapeOwner.GlobalPosition + new Vector3(
			m_rng.RandfRange(-w * 0.5f, w * 0.5f),
			m_rng.RandfRange(-h * 0.5f, h * 0.5f),
			m_rng.RandfRange(-d * 0.5f, d * 0.5f)
		);
	}

	private bool IsPositionFree(Vector3 position) {
		PhysicsDirectSpaceState3D space = GetWorld3D().DirectSpaceState;
		Transform3D transform = new (Basis.Identity, position);

		PhysicsShapeQueryParameters3D parameters = new PhysicsShapeQueryParameters3D() {
			Shape = m_targetShape,
			Transform = transform,
			CollideWithAreas = true,
			CollideWithBodies = false,
			CollisionMask = m_spawnCheckCollisionMask,
			Margin = m_minSpawnDistance,
		};

		return space.IntersectShape(parameters, 1).Count == 0;
	}

	private void InitMultiMesh() {
		MultiMesh multiMesh = new() {
			TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
			InstanceCount = MAX_VISIBLE_TARGETS,
			Mesh = m_targetMesh,
		};

		m_multiMeshInstance = new() {
			Multimesh = multiMesh,
		};

		AddChild(m_multiMeshInstance);
	}

	private void InitTargets() {
		for(int i = 0; i < MAX_VISIBLE_TARGETS; ++i) {
			Target target = m_targetPrefab.Instantiate<Target>();
			target.Name = $"Target{i}";
			target.Visible = false;
			target.multiMeshIdx = i;

			m_targets[i] = target;

			AddChild(target);
			m_freeTargets.Push(target);
		}

		m_targetShape = m_targets[0].GetChild<CollisionShape3D>(0).Shape as SphereShape3D;
	}
}
