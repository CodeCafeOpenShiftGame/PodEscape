using Godot;
using System;

public class LevelController : Node2D
{
	[Export((PropertyHint) 24, "17/17:PackedScene")]
	public Godot.Collections.Array<PackedScene> Scenes;

	private Node2D LevelHolder;

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
        if (prevPiece != null && prevPiece != instance) {
            GD.Print(prevPiece.Position.x);
            instance.Position = new Vector2(prevPiece.Position.x + 512f, 0f);
        } else {
            instance.Position = new Vector2(-512f, 0);
        }
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
