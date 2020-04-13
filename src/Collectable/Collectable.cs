using Godot;
using System;

public class Collectable : Node2D
{
    [Export]
    public int ScorePoints = 10;

    protected AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        this.animationPlayer = this.GetNode<AnimationPlayer>("Area2D/AnimationPlayer");
    }

    public virtual void _OnBodyEntered(PhysicsBody2D body)
    {
        this.animationPlayer.Play("fadeOut");
    }
}
