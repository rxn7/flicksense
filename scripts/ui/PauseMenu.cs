using System;
using Godot;

public partial class PauseMenu : CanvasLayer {
	public event Action onClosed;
	public event Action onOpen;
	public event Action onFinishSessionPressed;

	[Export] private Button m_resumeButton;
	[Export] private Button m_finishButton;
	[Export] private Button m_settingsButton;
	[Export] private Button m_exitButton;

	public override void _Ready() {
		ProcessMode = ProcessModeEnum.Always;
		Close();

		m_resumeButton.Pressed += Close;
		m_finishButton.Pressed += () => onFinishSessionPressed?.Invoke();

		// m_settingsButton.Pressed += () => 

		m_exitButton.Pressed += () => {
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
			Close();
			return;
		} 

		Open();
	}

	private void Close() {
		Visible = false;
		onClosed?.Invoke();
	}

	private void Open() {
		Visible = true;
		onOpen?.Invoke();
	}
}
