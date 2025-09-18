using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class TargetManager : Node {
	[Export] private PackedScene m_targetPrefab;
	[Export] private Mesh m_targetMesh;

	private const int MAX_VISIBLE_TARGETS = 3;
	private const int GRID_WIDTH = 10;
	private const int GRID_HEIGHT = 5;
	private const float GRID_TOTAL_WIDTH = GRID_WIDTH * 0.5f;
	private const float GRID_TOTAL_HEIGHT = GRID_HEIGHT * 0.5f;

	private MultiMeshInstance3D m_multiMeshInstance;

	private Target[] m_targetGrid = new Target[GRID_WIDTH * GRID_HEIGHT]; // null = empty
	private Target[] m_targets = new Target[MAX_VISIBLE_TARGETS];
	private Stack<Target> m_freeTargets = new(MAX_VISIBLE_TARGETS);

	public override void _Ready() {
		InitMultiMesh();

		InitTargets();
		Reset();
	}

	public void ShowRandomTarget(int except) {
		while(true) {
			int idx = (int)(GD.Randi() % (GRID_WIDTH * GRID_HEIGHT));

			if(idx != except && TryShowTarget(idx)) {
				break;
			}
		}
	}

	public void HideTarget(Target target) {
		m_targetGrid[target.gridIdx] = null;
		m_freeTargets.Push(target);

		target.Reset();
	}

	public void Reset() {
		m_freeTargets.Clear();
		foreach(Target target in m_targets) {
			target.Reset();
			m_freeTargets.Push(target);
		}

		ShowRandomTargets();
	}


	private void ShowRandomTargets() {
		while(m_freeTargets.Count > 0) {
			int gridIdx = (int)(GD.Randi() % (GRID_WIDTH * GRID_HEIGHT));
			TryShowTarget(gridIdx);
		}
	}

	private Vector3 GridIndexToPosition(int gridIdx) {
		int x = gridIdx % GRID_WIDTH;
		int y = gridIdx / GRID_WIDTH;
		return new Vector3(
			(x / (float)(GRID_WIDTH - 1)) * GRID_TOTAL_WIDTH - GRID_TOTAL_WIDTH * 0.5f,
			(y / (float)(GRID_HEIGHT - 1)) * GRID_TOTAL_HEIGHT,
			0.0f
		);
	}

	private bool TryShowTarget(int gridIdx) {
		if(gridIdx < 0 || gridIdx >= GRID_WIDTH * GRID_HEIGHT) {
			return false;
		}

		if(m_targetGrid[gridIdx] != null) {
			return false;
		}

		Target target = m_freeTargets.Pop();
		target.gridIdx = gridIdx;
		target.Position = GridIndexToPosition(gridIdx);
		target.Visible = true;

		m_targetGrid[gridIdx] = target;

		m_multiMeshInstance.Multimesh.SetInstanceTransform(target.multiMeshIdx, target.GlobalTransform);

		return true;
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
	}
}
