using Godot;

[GlobalClass]
public partial class Target : Area3D {
	public int multiMeshIdx = -1;

	public void Reset() {
		Visible = false;
	}
}
