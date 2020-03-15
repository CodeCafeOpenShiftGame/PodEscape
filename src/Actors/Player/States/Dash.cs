using Godot;
using Godot.Collections;
using System;

public class Dash : Move
{
	public Timer DashTimer;
	[Export]
	public float HorizontalAcceleration = 10000f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		this.DashTimer = this.GetNode("DashTimer") as Timer;
	}

	public override void UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("dash"))
		{
			base.Velocity = this.CalculateDashVelocity(base.DashImpulse);
		}
		else 
		{
			base.UnhandledInput(@event);
		}
	}

    public override void PhysicsProcess(float delta)
    {
        base.PhysicsProcess(delta);

        Player player = this.Owner as Player;

		if (player.IsOnFloor())
		{
			string targetState = base.GetMoveDirection().x == 0 ? "Idle" : "Run";
            this.StateMachine.TransitionTo(targetState);
		} else if (!player.IsOnFloor())
		{
			this.StateMachine.TransitionTo("Air");
		}

    }

	public override void Enter(Dictionary<string, object> msg = null)
	{
		base.Enter(msg);

		this.Acceleration.x = this.HorizontalAcceleration;

		if (msg == null)
			return;

		Player player = (Player)this.Owner;
        AnimationPlayer animationPlayer = player.GetNode("AnimationPlayer") as AnimationPlayer;
        animationPlayer.Play("Dash");

        if (msg.ContainsKey("velocity"))
        {
            this.Velocity = (Vector2)msg["velocity"];
            this.MaxSpeed.x = Math.Max(Math.Abs(this.Velocity.x), this.MaxSpeed.x);
        }

        if (msg.ContainsKey("impulse"))
        {
            this.Velocity += this.CalculateDashVelocity((float)msg["impulse"]);
        }

	}

	public override void Exit()
	{
		this.Acceleration = this.AccelerationDefault;
		base.Exit();
	}

	public virtual Vector2 CalculateDashVelocity(float impulse = 0f)
	{
		return base.CalculateVelocity(
			base.Velocity,
			base.MaxSpeed,
			new Vector2(impulse, 0f),
			1f,
			Vector2.Right
		);
	} 
}
