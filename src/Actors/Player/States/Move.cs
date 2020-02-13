using Godot;
using Godot.Collections;
using System;

public class Move : State
{
    [Export]
    public Vector2 MaxSpeedDefault = new Vector2(500f, 1500f);
    [Export]
    public Vector2 AccelerationDefault = new Vector2(100000f, 3000f);
    [Export]
    public float JumpImpulse = 900f;

    public Vector2 Acceleration;
    public Vector2 MaxSpeed;
    public Vector2 Velocity = Vector2.Zero;
    public float SnapDistance = 32f;
    public Vector2 SnapVector = new Vector2(0f, 32f);

    public override void _Ready()
    {
        base._Ready();
        this.Acceleration = this.AccelerationDefault;
        this.MaxSpeed = this.MaxSpeedDefault;
    }

    public override void UnhandledInput(InputEvent @event)
    {
        Player player = (Player)this.Owner;

        if (player.IsOnFloor() && @event.IsActionPressed("jump"))
        {
            Dictionary<string, object> msg = new Dictionary<string, object>();
            msg.Add("impulse", JumpImpulse);
            this.StateMachine.TransitionTo("Move/Air", msg);
        }
    }

    public override void PhysicsProcess(float delta)
    {
        Player player = (Player)this.Owner;
        player.Flip(this.GetMoveDirection());

        this.Velocity = this.CalculateVelocity(
            this.Velocity,
            this.MaxSpeed,
            this.Acceleration,
            delta,
            this.GetMoveDirection()
        );

        this.Velocity.y += 3500f * delta;

        this.Velocity = player.MoveAndSlideWithSnap(
            this.Velocity,
            this.SnapVector,
            player.FLOOR_NORMAL
        );
    }

    public virtual void Flip(Player player)
    {
        Position2D bodyPivot = player.GetNode("BodyPivot") as Position2D;
        bodyPivot.Scale = new Vector2(
            this.GetMoveDirection().x,
            1
        );
    }

    public virtual Vector2 CalculateVelocity(
		Vector2 oldVelocity,
		Vector2 maxSpeed,
		Vector2 acceleration,
		float delta,
		Vector2 moveDirection
	) {
        Vector2 newVelocity = oldVelocity;

        newVelocity += moveDirection * acceleration * delta;
        newVelocity.x = Mathf.Clamp(newVelocity.x, -maxSpeed.x, maxSpeed.x);
        newVelocity.y = Mathf.Clamp(newVelocity.y, -maxSpeed.y, maxSpeed.y);

        return newVelocity;
    }

    public virtual Vector2 GetMoveDirection()
    {
	    return new Vector2(
		    Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
		    1f
	    );
    }
}