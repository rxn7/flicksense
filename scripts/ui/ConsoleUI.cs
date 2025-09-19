using Godot;
using System.Linq;

public partial class ConsoleUI : Control {
	[Export] private RichTextLabel m_output;
	[Export] private LineEdit m_input;

	public override void _Ready() {
		HideConsole();

		m_input.TextSubmitted += OnInputSubmitted;
		m_input.TextChanged += OnInputChanged;
	}

	public override void _UnhandledKeyInput(InputEvent ev) {
		if(ev is not InputEventKey key) {
			return;
		}

		if(key.IsActionPressed("toggle_console")) {
			ToggleConsole();
		}
	}

	public void ToggleConsole() {
		if(Visible) HideConsole();
		else OpenConsole();
	}

	public void OpenConsole() {
		m_input.GrabFocus();
		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	public void HideConsole() {
		m_input.ReleaseFocus();
		Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public void Clear() {
		m_output.Clear();
	}

	public void AddText(string text) {
		m_output.AppendText(text);

		while(m_output.GetLineCount() > 500) {
			m_output.RemoveParagraph(0);
		}
	}

	public void PrintInfo(string message) => AddText($"[INFO] {message}\n");
	public void PrintError(string message) => AddText($"[ERROR] {message}\n");
	public void PrintWarning(string message) => AddText($"[WARNING] {message}\n");
	
	public void HandleCommand(string text) {
		string[] split = text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
		if(split.Length < 1) {
			return;
		}

		string command = split[0];
		string[] args = split.Skip(1).ToArray();

		m_output.AppendText($">> {text}\n");

		string errorMessage = CommandManager.Instance.Execute(command, args);
		if(errorMessage != null) {
			m_output.AppendText($"[ERROR] {errorMessage}\n");
		} 
	}

	private void OnInputSubmitted(string text) {
		HandleCommand(text);
		m_input.Clear();
	}

	private void OnInputChanged(string text) {
		if(text.EndsWith("`")) {
			m_input.Clear();
			HideConsole();
		}
	}
}
