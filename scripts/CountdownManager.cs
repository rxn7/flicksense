using Godot;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
			yield return i;
			await Task.Delay(1000, token);
		}
	}
}
