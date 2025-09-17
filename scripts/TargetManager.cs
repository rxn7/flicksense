using Godot;

[GlobalClass]
public partial class TargetManager : Node {
	[Export] private PackedScene m_targetPrefab;
	private const int VISIBLE_TARGETS = 3;
	private const int GRID_WIDTH = 10;
	private const int GRID_HEIGHT = 5;
	private const float GRID_TOTAL_WIDTH = GRID_WIDTH * 0.5f;
	private const float GRID_TOTAL_HEIGHT = GRID_HEIGHT * 0.5f;

	private Target[,] m_targetGrid = new Target[GRID_WIDTH, GRID_HEIGHT];

	public override void _Ready() {
		InitTargetGrid(true);
	}

	public void ShowRandomTarget(int except) {
		while(true) {
			int idx = (int)(GD.Randi() % (GRID_WIDTH * GRID_HEIGHT));

			if(idx == except) {
				continue;
			}

			if(TryShowTarget(idx)) {
				break;
			}
		}
	}

	public void InitTargetGrid(bool spawn) {
		if(spawn) {
			for(int x = 0; x < GRID_WIDTH; ++x) {
				for(int y = 0; y < GRID_HEIGHT; ++y) {
					Target target = m_targetPrefab.Instantiate<Target>();
					target.idx = x + y * GRID_WIDTH;
					target.Name = $"Target {x},{y}";
					target.Visible = false;

					target.Position = new Vector3(
						(x / (float)(GRID_WIDTH - 1)) * GRID_TOTAL_WIDTH - GRID_TOTAL_WIDTH * 0.5f,
						(y / (float)(GRID_HEIGHT - 1)) * GRID_TOTAL_HEIGHT,
						0.0f
					);

					AddChild(target);
					m_targetGrid[x, y] = target;
				}
			}
		} else {
			for(int x = 0; x < GRID_WIDTH; ++x) {
				for(int y = 0; y < GRID_HEIGHT; ++y) {
					m_targetGrid[x, y].Visible = false;
				}
			}
		}

		int targetsToShow = 3;
		while(targetsToShow > 0) {
			int idx = (int)(GD.Randi() % (GRID_WIDTH * GRID_HEIGHT));

			if(TryShowTarget(idx)) {
				--targetsToShow;
			}
		}
	}

	private (int, int) IndexToXY(int index) {
		int x = index % GRID_WIDTH;
		int y = index / GRID_WIDTH;
		return (x, y);
	}

	// Returns true if a target was hidden
	private bool TryShowTarget(int idx) {
		if(idx < 0 || idx >= GRID_WIDTH * GRID_HEIGHT) {
			return false;
		}

		(int x, int y) = IndexToXY(idx);

		// Already visible
		if(m_targetGrid[x, y].Visible) {
			return false;
		}

		m_targetGrid[x, y].Visible = true;
		return true;
	}
}
