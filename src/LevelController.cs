using Godot;
using System;

public class LevelController : Node2D
{
    [Export((PropertyHint) 24, "17/17:PackedScene")]
    public Godot.Collections.Array<PackedScene> Scenes;

    private Node2D LevelHolder;

    public override void _Ready()
    {
        SpawnRandomScene();
    }

    private void SpawnRandomScene()
    {
        Random random = new Random();
        int index = random.Next(0, Scenes.Count);

        PackedScene scene = Scenes[index];
        Node sceneInstance = scene.Instance();
        sceneInstance.GetNode("VisibilityNotifier2D").Connect("screen_exited", this, "_OnPieceScreenExited");

        AddChild(sceneInstance);
    }

    public void _OnPieceScreenExited()
    {
        GD.Print("Piece isn't visible anymore. Destroy it.");
    }
}
