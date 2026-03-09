using Godot;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

[GlobalClass]
public partial class CountdownManager : CanvasLayer {
	[Export] private AudioStreamPlayer m_player;

	public override void _Ready() {
		Visible = false;
	}

	public async IAsyncEnumerable<int> CountdownCoroutineAsync([EnumeratorCancellation] CancellationToken token = default) {
		m_player.Play(0.0f);

		int steps = 3;
		for(int i = steps; i > 0; --i) {
			if(token.IsCancellationRequested) {
				yield break;
			}

			yield return i;
			await ToSignal(GetTree().CreateTimer(1.0f, false, false, false), Godot.Timer.SignalName.Timeout);
		}
	}
}
