using Godot;

public partial class PauseMenu : CanvasLayer {
	[Export] private Button m_resumeButton;
	[Export] private Button m_settingsButton;
	[Export] private Button m_exitButton;

	public override void _Ready() {
		ProcessMode = ProcessModeEnum.Always;
		HideMenu();

		m_resumeButton.Pressed += HideMenu;
		// m_settingsButton.Pressed += () => 
		m_exitButton.Pressed += () => {
			GetTree().Paused = false;
			SceneSwitcher.SwitchToMainMenu();
		};
	}

	public override void _UnhandledKeyInput(InputEvent ev) {
		if(ev is not InputEventKey key || !key.IsPressed()) {
			return;
		}

		if(key.IsActionPressed("toggle_pause")) {
			Toggle();
		}
	}

	private void Toggle() {
		if(Visible) {
			HideMenu();
			return;
		} 

		ShowMenu();
	}

	private void HideMenu() {
		Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		GetTree().Paused = false;
	}

	private void ShowMenu() {
		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		GetTree().Paused = true;
	}
}
