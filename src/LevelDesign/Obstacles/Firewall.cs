using Godot;
using System;

public class Firewall : Node2D
{
    private Vector2 scaleVector2;
    private Tween fireTween;
    private Vector2 initialVector2 = Vector2.One;
    private Vector2 finalVector2 = Vector2.One;
    private bool up;
    private Node2D fireNode2D;

    [Export]
    public float WallHeight;

    [Export]
    public float WallRaiseDuration;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.fireNode2D = this.GetNode<Node2D>("GeneratorNode2D/FireNode2D");
        this.fireTween = this.GetNode<Tween>("GeneratorNode2D/FireNode2D/Tween");
        this.initialVector2 = Vector2.One;
        this.finalVector2 = Vector2.One;
        this.finalVector2.y = this.WallHeight + 1.0f;
        this.up = true;
        this.fireTween.InterpolateProperty(this.fireNode2D,
            "scale",
            this.initialVector2,
            this.finalVector2,
            this.WallRaiseDuration,
            Tween.TransitionType.Sine,
            Tween.EaseType.InOut);
        this.fireTween.Start();
    }

    public void _on_Tween_tween_completed( Godot.Object obj, NodePath key)
    {
        if (up)
        {
            up = false;
            this.initialVector2.y = this.WallHeight + 1.0f;
            this.finalVector2.y = 1.0f;
        }
        else // down
        {
            up = true;
            this.initialVector2.y = 1.0f;
            this.finalVector2.y = this.WallHeight + 1.0f;
        }
        this.fireTween.InterpolateProperty(this.fireNode2D,
            "scale",
            this.initialVector2,
            this.finalVector2,
            this.WallRaiseDuration,
            Tween.TransitionType.Sine,
            Tween.EaseType.InOut);
        this.fireTween.Start();
    }
}
