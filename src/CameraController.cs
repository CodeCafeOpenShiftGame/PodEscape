using Godot;
using System;

public class CameraController : Node2D
{
    public float CurrentSpeed = 200;
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position += CalculateCameraSpeed(delta);
    }

    public Vector2 CalculateCameraSpeed(float delta)
    {
        return new Vector2(CurrentSpeed * delta, 0);
    }
}
