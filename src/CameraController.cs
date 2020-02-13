using Godot;
using System;

public class CameraController : Node2D
{
	public float CurrentSpeed = 100f;

	public override void _PhysicsProcess(float delta)
	{
		Position += CalculateCameraSpeed(delta);
	}

	public Vector2 CalculateCameraSpeed(float delta)
	{
		return new Vector2(CurrentSpeed * delta, 0);
	}
}
