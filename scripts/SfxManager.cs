using Godot;

public enum ESfx {
	ShootMiss,
	ShootHit,
}

public partial class SfxManager : AudioStreamPlayer {
	[Export] private Godot.Collections.Dictionary<ESfx, AudioStream> m_sounds = new();

	private AudioStreamPolyphonic m_stream;

	public override void _Ready() {
		Stream = m_stream = new AudioStreamPolyphonic();
		m_stream.Polyphony = 8;
	}

	public void PlaySfx(ESfx sfx, float pitch = 1.0f) {
		if(!Playing) {
			Play();
		}

		AudioStreamPlaybackPolyphonic playback = (AudioStreamPlaybackPolyphonic)GetStreamPlayback();
		playback.PlayStream(m_sounds[sfx], 0.0f, 0.0f, pitch);
	}

	public static void SetMasterVolume(float volume){ 
		AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Master"), volume);
	}
}
