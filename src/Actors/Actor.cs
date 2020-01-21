using Godot;
using System;

public class Actor : KinematicBody2D
{
    public Vector2 FLOOR_NORMAL = Vector2.Up;

    [Export]
    public Vector2 speed = new Vector2(400, 500);
    [Export]
    public float gravity = 3500;

    protected Vector2 velocity = Vector2.Zero;
 
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        velocity.y += gravity * delta;
    }
}
