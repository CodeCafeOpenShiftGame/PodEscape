using Godot;
using System;

public class Player : Actor
{
	public StateMachine StateMachine;

	public AnimationPlayer AnimationPlayer;

	public Vector2 LookDirection = new Vector2(1, 1);

	public override void _Ready()
	{
		this.StateMachine = this.GetNode("StateMachine") as StateMachine;
		this.AnimationPlayer = this.GetNode("AnimationPlayer") as AnimationPlayer;
	}

	public virtual void Flip(Vector2 direction)
	{
		if (direction.x != 0 && this.LookDirection != direction) {
			Position2D bodyPivot = this.GetNode("BodyPivot") as Position2D;

			this.LookDirection = direction;

			bodyPivot.Scale = new Vector2(
				direction.x,
				1
			);
		}
	}

}
