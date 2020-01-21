using Godot;
using System;


public class Player : KinematicBody2D
{
	private const float movementAmount = 100.0f;
	private const float jumpAmount = 400.0f;
	
	private Vector2 linearVelocity;
	private Vector2 floorNormal = new Vector2(0.0f, -1.0f);
	private float gravity = 10.0f;
	private Sprite sprite;
	private KinematicBody2D player;
	private AnimatedSprite playerSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneTree tree = GetTree();
		Node currentScene = tree.CurrentScene;
		Node rootNode = tree.GetRoot();
		player = currentScene.GetNode("/root/World/Player") as KinematicBody2D;
		playerSprite = currentScene.GetNode("/root/World/Player/PlayerSprite") as AnimatedSprite;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(float delta) {}

	public override void _PhysicsProcess(float delta)
	{
		linearVelocity.x = 0.0f;
		linearVelocity.y += gravity;
		if (Input.IsActionPressed("ui_right"))
		{
			linearVelocity.x = movementAmount;
			playerSprite.FlipH = false;
			playerSprite.Play("run");
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			linearVelocity.x = -movementAmount;
			playerSprite.FlipH = true;
			playerSprite.Play("run");
		}
		else
		{
			playerSprite.Play("idle");
		}
		
		if (IsOnFloor())
		{
			if (Input.IsActionJustPressed("ui_up"))
			{
				linearVelocity.y = -jumpAmount;
			}
		}
		else
		{
			playerSprite.Play("jump");
		}
		
		linearVelocity = MoveAndSlide(linearVelocity, floorNormal);
	}
}
