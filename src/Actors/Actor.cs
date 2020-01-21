using Godot;
using System;

public class Actor : KinematicBody2D
{
    public Vector2 FLOOR_NORMAL = Vector2.Up;

    [Export]
    public Vector2 Speed = new Vector2(400, 500);
    [Export]
    public float Gravity = 3500;

    protected Vector2 Velocity = Vector2.Zero;

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Velocity.y += Gravity * delta;
    }
}
