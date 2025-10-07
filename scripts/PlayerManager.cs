using Godot;

public partial class PlayerManager : Node3D {
	[Export] public CameraManager CameraManager { get; private set; }

	public void Reset() {
		CameraManager.Reset();
	}
}
