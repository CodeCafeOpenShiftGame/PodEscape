using Godot;
using System;

public class GhostEffect : Sprite
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Tween GhostTween = this.GetNode("Tween") as Tween;
        GhostTween.InterpolateProperty(
            this,
            "modulate",
            new Color(1, 1, 1, 1),
            new Color(1, 1, 1, 0),
            .6f,
            Tween.TransitionType.Sine,
            Tween.EaseType.Out
        );
        GhostTween.Start();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public virtual void _OnTweenCompleted(Godot.Object obj, NodePath key)
    {
        QueueFree();
    }
}
