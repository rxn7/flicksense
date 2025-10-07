using Godot;

public partial class CameraManager : Camera3D {
	public float Pitch { get; private set; } = 0f;
	public float Yaw { get; private set; } = 0f;

	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Input.UseAccumulatedInput = false;
	}

	public override void _Process(double delta) {
		GlobalRotationDegrees = Vector3.Right * Pitch + Vector3.Up * Yaw;
	}

	public override void _Input(InputEvent ev) {
		if(ev is not InputEventMouseMotion motion) {
			return;
		}

		if(Input.MouseMode != Input.MouseModeEnum.Captured) {
			return;
		}

		float sens = Global.Instance.settings.sensitivity;

		Pitch -= motion.ScreenRelative.Y * sens * 0.022f; // 0.022 is a CS:GO/CS2 constant, TODO: conversion for different games
		Yaw -= motion.ScreenRelative.X * sens * 0.022f;

		Pitch = Mathf.Clamp(Pitch, -90.0f, 90.0f);
		Yaw = Mathf.PosMod(Yaw, 360.0f);
	}

	public void Reset() {
		Pitch = 0;
		Yaw = 0;
	}
}
