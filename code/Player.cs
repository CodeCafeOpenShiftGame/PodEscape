using Godot;
using System;


public class Player : KinematicBody2D
{
	private const float movementAmount = 100.0f;
	
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Vector2 linearVelocity;
	//private KinematicBody2D player;

	// Called when the node enters the scene tree for the first time.
	//public override void _Ready() {}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(float delta) {}

	public override void _PhysicsProcess(float delta)
	{
		//player = GetNode("player") as KinematicBody2D;
		
		linearVelocity = Vector2.Zero;
		if (Input.IsActionPressed("ui_right"))
		{
			linearVelocity.x = movementAmount;
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			linearVelocity.x = -movementAmount;
		}
		
		if (Input.IsActionPressed("ui_up"))
		{
			linearVelocity.y = -movementAmount;
		}
		else if (Input.IsActionPressed("ui_down"))
		{
			linearVelocity.y = movementAmount;
		}
		
		MoveAndSlide(linearVelocity);
	}
}
