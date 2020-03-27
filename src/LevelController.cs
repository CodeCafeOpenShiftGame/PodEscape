using Godot;
using System;

public class LevelController : Node2D
{
	[Export((PropertyHint) 24, "17/17:PackedScene")]
	public Godot.Collections.Array<PackedScene> Scenes;

	private Node2D LevelHolder;
    private float lastX = 0.0f;
    private float currentX = 0.0f;
    private Vector2 currentPosition;
    private Vector2 lastPosition;

	public override void _Ready()
	{
        this.LevelHolder = this.GetNode("Level") as Node2D;
        this.SpawnRandomScene();
	}

    private PackedScene GetRandomScene()
    {
        Random random = new Random();
		int index = random.Next(0, Scenes.Count);
		return Scenes[index] as PackedScene;
    }

    private Node2D InstantiateScene(PackedScene scene)
    {
        Node2D instance = scene.Instance() as Node2D;

		instance.GetNode("VisibilityNotifier2D").Connect("screen_exited", this, "_OnPieceScreenExited", null, 1);
        instance.GetNode("VisibilityNotifier2D").Connect("screen_entered", this, "_OnPieceScreenEntered", null, 1);

        this.LevelHolder.AddChild(instance);

        return instance;
    }

	private void SpawnRandomScene()
	{
		PackedScene scene = this.GetRandomScene();
		Node2D instance = this.InstantiateScene(scene);

        Node2D prevPiece = this.LevelHolder.GetChild<Node2D>(this.LevelHolder.GetChildCount() - 2);
        if (null == prevPiece)
        {
            this.lastPosition.x = -1024.0f;
            this.lastPosition.y = 1000.0f;
        }
        this.currentPosition.x = this.lastPosition.x + 1024.0f;
        this.currentPosition.y = 1000.0f;
        instance.Position = this.currentPosition;
        this.lastPosition = instance.Position;
    }

	public void _OnPieceScreenExited()
	{
		// @FIXME Probably not the best solution
        Node active = this.LevelHolder.GetChild(0);
		active.QueueFree();
		GD.Print("Piece isn't visible anymore. Destroy it.");
	}

    public void _OnPieceScreenEntered()
    {
        GD.Print("Need to spawn a new piece.");
        this.SpawnRandomScene();
    }
}
