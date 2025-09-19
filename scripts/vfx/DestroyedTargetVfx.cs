using Godot;
using System;

public partial class DestroyedTargetVfx : Node3D, IVfxObject {
	private const ulong VFX_DURATION_MS = 1000;

	public event Action finished;

	private struct Piece {
		public Vector3 initialPosition;
		public Vector3 linearVelocity;
		public Vector3 angularVelocity;
	}

	private RandomNumberGenerator m_rng;
	private Piece[] m_pieces;
	private ulong m_lastExplodeTime;

	public override void _Ready() {
		Visible = false;
		m_rng = new();

		m_pieces = new Piece[GetChildCount()];
		for(int i=0; i<m_pieces.Length; ++i) {
			m_pieces[i].initialPosition = GetChild<Node3D>(i).Position;
		}

		ProcessMode = ProcessModeEnum.Disabled;
	}

	public override void _Process(double delta) {
		if(Time.GetTicksMsec() - m_lastExplodeTime > VFX_DURATION_MS) {
			Reset();
			finished?.Invoke();
			return;
		}

		float elapsedRatio = (float)(Time.GetTicksMsec() - m_lastExplodeTime) / VFX_DURATION_MS;

		for(int i=0; i<m_pieces.Length; ++i) {
			ref Piece piece = ref m_pieces[i];
			MeshInstance3D mesh = GetChild<MeshInstance3D>(i);

			mesh.GlobalPosition += piece.linearVelocity * (float)delta;
			mesh.GlobalRotation += piece.angularVelocity * (float)delta;
			mesh.Scale = Vector3.One * (1.0f - elapsedRatio);

			piece.linearVelocity += Vector3.Down * 9.8f * (float)delta;
		}
	}

	public void Explode(Vector3 targetPosition, Vector3 hitPoint, Vector3 shootDir) {
		ProcessMode = ProcessModeEnum.Inherit;

		GlobalPosition = targetPosition;

		Visible = true;
		m_lastExplodeTime = Time.GetTicksMsec();

		Vector3 shotImpulse = shootDir * m_rng.RandfRange(5f, 12f);

		for(int i=0; i<m_pieces.Length; ++i) {
			ref Piece piece = ref m_pieces[i];
			MeshInstance3D mesh = GetChild<MeshInstance3D>(i);

			mesh.Position = piece.initialPosition;
			mesh.Rotation = Vector3.Zero;

			Vector3 offset = mesh.GlobalPosition - hitPoint;
			Vector3 radialDirection = (offset).Normalized();
			Vector3 randomOffset = new Vector3(
				m_rng.RandfRange(-0.3f, 0.3f),
				m_rng.RandfRange(-0.3f, 0.3f),
				m_rng.RandfRange(-0.3f, 0.3f)
			);
			radialDirection += randomOffset;
			radialDirection = radialDirection.Normalized();

			float distanceFactor = 1.0f - (offset.Length() / 0.4f);

			Vector3 radialImpulse = radialDirection * m_rng.RandfRange(0.5f, 2.0f) * distanceFactor;
			piece.linearVelocity = radialImpulse + shotImpulse;

			piece.angularVelocity = new Vector3(
				m_rng.RandfRange(-10f, 10f),
				m_rng.RandfRange(-10f, 10f),
				m_rng.RandfRange(-10f, 10f)
			);
		}
	} 

	public void Reset() {
		ProcessMode = ProcessModeEnum.Disabled;
		Visible = false;

		for(int i=0; i<m_pieces.Length; ++i) {
			MeshInstance3D mesh = GetChild<MeshInstance3D>(i);
			mesh.GlobalPosition = m_pieces[i].initialPosition;
			mesh.GlobalRotation = Vector3.Zero;
		}
	}
}
