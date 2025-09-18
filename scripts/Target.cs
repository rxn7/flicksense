using Godot;

public partial class Target : Node3D {
	public int gridIdx = -1;
	public int multiMeshIdx = -1;

	public void Reset() {
		gridIdx = -1;
		Visible = false;
	}
}
