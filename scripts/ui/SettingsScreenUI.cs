using Godot;

[GlobalClass]
public partial class SettingsScreenUI : Control {
	[Export] private Button m_closeSaveButton;
	[Export] private Button m_closeNoSaveButton;

	[Export] private SpinBox m_maxFpsInput;
	[Export] private SpinBox m_sensInput;
	[Export] private Slider m_volumeInput;
	[Export] private OptionButton m_crosshairTypeInput;
	[Export] private ColorPicker m_crosshairColorInput;
	[Export] private SpinBox m_crosshairOutlineInput;
	[Export] private SpinBox m_classicCrosshairLengthInput;
	[Export] private SpinBox m_classicCrosshairThicknessInput;
	[Export] private SpinBox m_classicCrosshairGapInput;
	[Export] private SpinBox m_dotCrosshairRadiusInput;

	[Export] private Control m_classicCrosshairSection;
	[Export] private Control m_dotCrosshairSection;

	public override void _Ready() {
		Visible = false;

		m_closeSaveButton.Pressed += () => {
			SaveSettings();
			Visible = false;
		};

		m_closeNoSaveButton.Pressed += () => {
			Visible = false;
		};

		m_crosshairTypeInput.ItemSelected += (long idx) => {
			UpdateCrosshairType((CrosshairType)idx);
		};
	}

	public void Open() {
		Visible = false;

		ref Settings settings = ref SettingsManager.settings;

		m_maxFpsInput.Value = settings.maxFps;

		m_sensInput.Value = settings.sensitivity;
		m_volumeInput.Value = settings.audioVolume;
		m_crosshairTypeInput.Selected = (int)settings.crosshairType;
		m_crosshairColorInput.Color = settings.crosshairData.color;
		m_crosshairOutlineInput.Value = settings.crosshairData.outlineThickness;

		UpdateCrosshairType(settings.crosshairType);

		Visible = true;
	}

	private void SaveSettings() {
		SettingsManager.settings = new Settings {
			maxFps = (uint)m_maxFpsInput.Value,
			audioVolume = (float)m_volumeInput.Value,
			sensitivity = (float)m_sensInput.Value,
			crosshairType = (CrosshairType)m_crosshairTypeInput.Selected,
		};

		switch(SettingsManager.settings.crosshairType) {
			case CrosshairType.Classic:
				SettingsManager.settings.crosshairData = new ClassicCrosshairData() {
					color = m_crosshairColorInput.Color,
					outlineThickness = (float)m_crosshairOutlineInput.Value,
					length = (float)m_classicCrosshairLengthInput.Value,
					thickness = (float)m_classicCrosshairThicknessInput.Value,
					gap = (float)m_classicCrosshairGapInput.Value,
				};
				break;

			case CrosshairType.Dot:
				SettingsManager.settings.crosshairData = new DotCrosshairData() {
					color = m_crosshairColorInput.Color,
					outlineThickness = (float)m_crosshairOutlineInput.Value,
					radius = (float)m_dotCrosshairRadiusInput.Value
				};
				break;
		}

		SettingsManager.Save();
		SettingsManager.ApplySettings();
	}
	
	private void UpdateCrosshairType(CrosshairType type) {
		m_classicCrosshairSection.Visible = false;
		m_dotCrosshairSection.Visible = false;

		ref Settings settings = ref SettingsManager.settings;
		switch(type) {
			case CrosshairType.Classic: {
				ClassicCrosshairData c = type == settings.crosshairType ? (ClassicCrosshairData)settings.crosshairData : new();

				m_classicCrosshairSection.Visible = true;
				m_classicCrosshairLengthInput.Value = c.length;
				m_classicCrosshairThicknessInput.Value = c.thickness;
				m_classicCrosshairGapInput.Value = c.gap;
				break;
			}

			case CrosshairType.Dot: {
				DotCrosshairData c = type == settings.crosshairType ? (DotCrosshairData)settings.crosshairData : new();

				m_dotCrosshairSection.Visible = true;
				m_dotCrosshairRadiusInput.Value = c.radius;
				break;
			}
		}
	}
}
