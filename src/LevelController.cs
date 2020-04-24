using Godot;
using System;

public class LevelController : Node2D
{
    [Export((PropertyHint)24, "17/17:PackedScene")]
    public Godot.Collections.Array<PackedScene> EasyLevels;
    [Export((PropertyHint)24, "17/17:PackedScene")]
    public Godot.Collections.Array<PackedScene> MediumLevels;
    [Export((PropertyHint)24, "17/17:PackedScene")]
    public Godot.Collections.Array<PackedScene> HardLevels;

    [Export((PropertyHint)24, "17/17:PackedScene")]
    public Godot.Collections.Array<PackedScene> DebugLevels;

    private Node2D LevelHolder;
    private float lastX = 0.0f;
    private float currentX = 0.0f;
    private Vector2 currentPosition;
    private Vector2 lastPosition;
    private VisibilityNotifier2D previousVisibilityNotifier2D;

    private Timer EasyTimer;
    private Timer MediumTimer;
    private GameManager.LevelDifficulty currentDifficulty;

    [Export]
    private Boolean levelDebug = false;

    public override void _Ready()
    {
        this.LevelHolder = this.GetNode("Level") as Node2D;
        this.SpawnRandomScene();
        this.EasyTimer = this.GetNode<Timer>("EasyTimer");
        this.MediumTimer = this.GetNode<Timer>("MediumTimer");
        this.EasyTimer.Start(60);
        this.currentDifficulty = GameManager.LevelDifficulty.easy;
    }

    private PackedScene GetRandomScene()
    {
        if (levelDebug == true)
        {
            return GetRandomSceneDifficulty(this.DebugLevels);
        }
        if (currentDifficulty == GameManager.LevelDifficulty.easy)
        {
            return GetRandomSceneDifficulty(this.EasyLevels);
        }
        else if (currentDifficulty == GameManager.LevelDifficulty.medium)
        {
            return GetRandomSceneDifficulty(this.MediumLevels);
        }
        else
        {
            return GetRandomSceneDifficulty(this.HardLevels);
        }

    }

    private PackedScene GetRandomSceneDifficulty(Godot.Collections.Array<PackedScene> levels)
    {
        Random random = new Random();
        int index = random.Next(0, levels.Count);
        return levels[index] as PackedScene;
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

        VisibilityNotifier2D vn2d = instance.GetNode<VisibilityNotifier2D>("VisibilityNotifier2D");
        Node2D prevPiece = this.LevelHolder.GetChild<Node2D>(this.LevelHolder.GetChildCount() - 2);
        if (null == prevPiece)
        {
            this.lastPosition.x = -vn2d.Position.x;
            this.lastPosition.y = instance.Position.y;
            this.previousVisibilityNotifier2D = vn2d;
        }

        this.currentPosition.x = this.lastPosition.x + this.previousVisibilityNotifier2D.Position.x;
        this.currentPosition.y = instance.Position.y;
        instance.Position = this.currentPosition;
        this.lastPosition = instance.Position;
        previousVisibilityNotifier2D = vn2d;
    }

    public void _OnPieceScreenExited()
    {
        // @FIXME Probably not the best solution
        Node active = this.LevelHolder.GetChild(0);
        active.QueueFree();
    }

    public void _OnPieceScreenEntered()
    {
        this.SpawnRandomScene();
    }

    public void _on_EasyTimer_timeout()
    {
        this.currentDifficulty = GameManager.LevelDifficulty.medium;
        this.MediumTimer.Start(30);
    }

    public void _on_MediumTimer_timeout()
    {
        this.currentDifficulty = GameManager.LevelDifficulty.hard;
    }
}
