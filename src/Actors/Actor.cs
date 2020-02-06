using Godot;
using System;

public interface IActor
{
    Vector2 GetDirection();
    Vector2 CalculateVelocity(Vector2 linerarVelocity, Vector2 direction, Vector2 speed);
}

abstract public class Actor : KinematicBody2D
{
    public Vector2 FLOOR_NORMAL = Vector2.Up;

    [Export]
    public Vector2 Speed = new Vector2(400, 500);
    [Export]
    public float Gravity = 3500;

    public Vector2 Velocity = Vector2.Zero;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        Velocity.y += Gravity * delta;
    }
}
