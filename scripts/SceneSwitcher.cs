using Godot;

public static class SceneSwitcher {
	public static readonly PackedScene MainMenuScene = GD.Load<PackedScene>("uid://cavalb7rmjuo8");
	public static readonly PackedScene GameScene = GD.Load<PackedScene>("uid://b2mq0co28h1rx");

	public static void SwitchToGame(EGameMode gameMode) {
		SceneTree tree = Global.Instance.GetTree();
		tree.CurrentScene.QueueFree();

		GameManager gameManager = GameScene.Instantiate<GameManager>();
		gameManager.Setup(gameMode);
		tree.Root.AddChild(gameManager);

		tree.CurrentScene = gameManager;
	}

	public static void SwitchToMainMenu() {
		SceneTree tree = Global.Instance.GetTree();
		tree.Paused = false;
		tree.ChangeSceneToPacked(MainMenuScene);
	}
}
