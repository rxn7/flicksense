using Godot;
using System.Linq;
using System.Collections.Generic;

public partial class ConsoleUI : Control {
	private const int MAX_HISTORY_SIZE = 10;
	private const int MAX_OUTPUT_PARAGRAPHS = 500;

	[Export] private RichTextLabel m_output;
	[Export] private LineEdit m_input;
	
	private List<string> m_history = new(10);
	private int m_historyIndex = -1;

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

		if(!Visible) {
			return;
		}

		if(key.Keycode == Key.Up) {
			HistoryUp();
		} else if(key.Keycode == Key.Down) {
			HistoryDown();
		}
	}

	public void ToggleConsole() {
		if(Visible) HideConsole();
		else OpenConsole();
	}

	public void OpenConsole() {
		m_input.GrabFocus();
		m_input.CaretColumn = m_input.Text.Length;
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

	public void PrintInfo(string message) {
		m_output.PushColor(Colors.LawnGreen);
		m_output.PushBoldItalics();
		m_output.AppendText("[INFO] ");
		m_output.PopAll();
		m_output.PushColor(Colors.LightGreen);
		m_output.AppendText(message);
		m_output.PopAll();
		m_output.Newline();
		DeleteOldParagraphs();
	}

	public void PrintError(string message) {
		m_output.PushColor(Colors.Red);
		m_output.PushBoldItalics();
		m_output.AppendText("[ERROR] ");
		m_output.PopAll();
		m_output.PushColor(Colors.IndianRed);
		m_output.AppendText(message);
		m_output.PopAll();
		m_output.Newline();
		DeleteOldParagraphs();
	}

	public void PrintWarning(string message) {
		m_output.PushColor(Colors.Yellow);
		m_output.PushBoldItalics();
		m_output.AppendText("[WARNING] ");
		m_output.PopAll();
		m_output.PushColor(Colors.LightYellow);
		m_output.AppendText(message);
		m_output.PopAll();
		m_output.Newline();
		DeleteOldParagraphs();
	}

	public void HandleCommand(string text) {
		string[] split = text.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
		if(split.Length < 1) {
			return;
		}

		string command = split[0];
		string[] args = split.Skip(1).ToArray();

		m_output.PushColor(Colors.Gray);
		m_output.AppendText(">> ");
		m_output.Pop();
		m_output.AppendText(text);
		m_output.Newline();

		DeleteOldParagraphs();

		string errorMessage = CommandManager.Instance.Execute(command, args);
		if(errorMessage != null) {
			PrintError(errorMessage);
		} 
	}

	private void DeleteOldParagraphs() {
		while(m_output.GetParagraphCount() > MAX_OUTPUT_PARAGRAPHS) {
			m_output.RemoveParagraph(0);	
		}
	}

	private void OnInputSubmitted(string text) {
		if(string.IsNullOrWhiteSpace(text)) {
			return;
		}

		HandleCommand(text);
		m_input.Clear();

		if(text != m_history.LastOrDefault()) {
			m_history.Add(text);
			while(m_history.Count > MAX_HISTORY_SIZE) {
				m_history.RemoveAt(0);
			}
		}
		m_historyIndex = -1;
	}

	private void OnInputChanged(string text) {
		if(text.Contains("`")) {
			m_input.Text = text.Replace("`", string.Empty);
			HideConsole();
		}
	}

	private void HistoryUp() {
		if(m_history.Count == 0) {
			return;
		}

		if(m_historyIndex == -1) {
			m_historyIndex = m_history.Count - 1;
		} else {
			--m_historyIndex;
			if(m_historyIndex == -1) {
				m_input.Clear();
				return;
			}
		}	

		m_input.Text = m_history[m_historyIndex];
		m_input.CaretColumn = m_input.Text.Length;
		m_input.GrabFocus();
	}

	private void HistoryDown() {
		if(m_history.Count == 0) {
			return;
		}

		if(m_historyIndex >= m_history.Count - 1) {
			m_historyIndex = -1;
			m_input.Clear();
			return;
		} else {
			++m_historyIndex;
		}

		m_input.Text = m_history[m_historyIndex];
		m_input.CaretColumn = m_input.Text.Length;
		m_input.GrabFocus();
	}
}
