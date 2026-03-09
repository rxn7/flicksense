using Godot;

[GlobalClass]
public partial class SettingsScreenUI : Control {
	[Export] private Button m_close_save_button;
	[Export] private Button m_close_no_save_button;

	[Export] private SpinBox m_max_fps_input;
	[Export] private SpinBox m_sens_input;
	[Export] private Slider m_volume_input;
	[Export] private OptionButton m_crosshair_type_input;
	[Export] private ColorPicker m_crosshair_color_input;
	[Export] private SpinBox m_crosshair_outline_input;
	[Export] private SpinBox m_classic_crosshair_length_input;
	[Export] private SpinBox m_classic_crosshair_thickness_input;
	[Export] private SpinBox m_classic_crosshair_gap_input;
	[Export] private SpinBox m_dot_crosshair_radius_input;

	[Export] private Control m_classic_crosshair_section;
	[Export] private Control m_dot_crosshair_section;

	public override void _Ready() {
		Visible = false;

		m_close_save_button.Pressed += () => {
			SaveSettings();
			Visible = false;
		};

		m_close_no_save_button.Pressed += () => {
			Visible = false;
		};
	}

	public void Open() {
		Visible = false;

		ref Settings settings = ref SettingsManager.settings;

		m_max_fps_input.Value = settings.maxFps;

		m_sens_input.Value = settings.sensitivity;
		m_volume_input.Value = settings.audioVolume;
		m_crosshair_type_input.Selected = (int)settings.crosshairType;
		m_crosshair_color_input.Color = settings.crosshairData.color;
		m_crosshair_outline_input.Value = settings.crosshairData.outlineThickness;

		m_classic_crosshair_section.Visible = false;
		m_dot_crosshair_section.Visible = false;

		switch(settings.crosshairType) {
			case CrosshairType.Classic: {
				ClassicCrosshairData c = (ClassicCrosshairData)settings.crosshairData;

				m_classic_crosshair_section.Visible = true;
				m_classic_crosshair_length_input.Value = c.length;
				m_classic_crosshair_thickness_input.Value = c.thickness;
				m_classic_crosshair_gap_input.Value = c.gap;
				break;
			}

			case CrosshairType.Dot: {
				DotCrosshairData c = (DotCrosshairData)settings.crosshairData;

				m_dot_crosshair_section.Visible = true;
				m_dot_crosshair_radius_input.Value = c.radius;
				break;
			}
		}

		Visible = true;
	}

	private void SaveSettings() {
		SettingsManager.settings = new Settings {
			maxFps = (uint)m_max_fps_input.Value,
			audioVolume = (float)m_volume_input.Value,
			sensitivity = (float)m_sens_input.Value,
			crosshairType = (CrosshairType)m_crosshair_type_input.Selected,
		};

		switch(SettingsManager.settings.crosshairType) {
			case CrosshairType.Classic:
				SettingsManager.settings.crosshairData = new ClassicCrosshairData() {
					color = m_crosshair_color_input.Color,
					outlineThickness = (float)m_crosshair_outline_input.Value,
					length = (float)m_classic_crosshair_length_input.Value,
					thickness = (float)m_classic_crosshair_thickness_input.Value,
					gap = (float)m_classic_crosshair_gap_input.Value,
				};
				break;

			case CrosshairType.Dot:
				SettingsManager.settings.crosshairData = new DotCrosshairData() {
					color = m_crosshair_color_input.Color,
					outlineThickness = (float)m_crosshair_outline_input.Value,
					radius = (float)m_dot_crosshair_radius_input.Value
				};
				break;
		}

		SettingsManager.Save();
		SettingsManager.ApplySettings();
	}
}
