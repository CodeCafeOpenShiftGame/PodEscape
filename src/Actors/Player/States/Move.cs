using Godot;
using Godot.Collections;
using System;

abstract public class Move : State
{
    protected Vector2 moveDirection = Vector2.One;

    [Export]
    public Vector2 MaxSpeedDefault = new Vector2(500f, 1500f);
    [Export]
    public Vector2 AccelerationDefault = new Vector2(100000f, 3000f);
    [Export]
    public Vector2 SlowAccelerationDefault = new Vector2(-1000f, -10f); // -1000f, 3000f for normal slow jumping
    [Export]
    public float JumpImpulse = 1200f;
    [Export]
    public float DashImpulse = 1500f;
    [Export]
    public float Gravity = 300f;

    public Vector2 Acceleration;
    public Vector2 MaxSpeed;
    public Vector2 Velocity = Vector2.Zero;
    public float SnapDistance = 32f;
    public Vector2 SnapVector = new Vector2(0f, 32f);

    public Boolean hasDashed;
    public Boolean isDashing;

    public override void _Ready()
    {
        base._Ready();
        this.Acceleration = this.AccelerationDefault;
        this.MaxSpeed = this.MaxSpeedDefault;
        this.isDashing = false;
    }

    public override void UnhandledInput(InputEvent @event)
    {
        Player player = (Player)this.Owner;

        // TODO: Epsilon check
        if (this.GetMoveDirection().x == 0f)
        {
            return;
        }

        if (@event.IsActionPressed("slow") && isDashing)
        {
            // StopDash();
        }

        if (player.IsOnFloor() && @event.IsActionPressed("jump"))
        {
            Dictionary<string, object> msg = new Dictionary<string, object>();
            msg.Add("impulse", JumpImpulse);
            this.StateMachine.TransitionTo("Air", msg);
        }

        if (@event.IsActionPressed("dash") && !player.IsOnFloor() && !this.hasDashed)
        {
            StartDash();
            hasDashed = true;
        }

        if (@event.IsActionPressed("slide") && player.IsOnFloor())
        {
            StartDash();
        }

        if (@event.IsActionPressed("slow") && player.IsOnFloor())
        {
            GD.Print("slow");
            this.Acceleration = this.SlowAccelerationDefault;
        }
        else {
            this.Acceleration = this.AccelerationDefault;
        }
    }

    public void StartDash()
    {
        Dictionary<string, object> msg = new Dictionary<string, object>();
        msg.Add("impulse", DashImpulse);
        this.StateMachine.TransitionTo("Dash", msg);
    }

    public override void PhysicsProcess(float delta)
    {
        Vector2 direction = this.GetMoveDirection();

        Player player = (Player)this.Owner;
        //player.Flip(direction);

        this.Velocity = this.CalculateVelocity(
            this.Velocity,
            this.MaxSpeed,
            this.Acceleration,
            delta,
            direction
        );

        this.Velocity.y += this.Gravity * delta;

        this.Velocity = player.MoveAndSlideWithSnap(
            this.Velocity,
            this.SnapVector,
            player.FLOOR_NORMAL
        );

        this.HandleCollisions(delta);
    }

    public void HandleCollisions(float delta)
    {
        var collisionInfo = player.MoveAndCollide(this.Velocity * delta, true, true, true);

        if (collisionInfo != null && !isDashing)
        {
            foreach (Node2D node in GetTree().GetNodesInGroup("killingObstacles"))
            {
                if (collisionInfo.ColliderId == node.GetInstanceId())
                {
                    this.StateMachine.TransitionTo("Die");
                }
            }
            foreach (Node2D node in GetTree().GetNodesInGroup("dashable"))
            {
                if (collisionInfo.ColliderId == node.GetInstanceId())
                {
                    this.StateMachine.TransitionTo("Die");
                }
            }
        }
    }

    public virtual Vector2 CalculateVelocity(
        Vector2 oldVelocity,
        Vector2 maxSpeed,
        Vector2 acceleration,
        float delta,
        Vector2 moveDirection
    )
    {
        Vector2 newVelocity = oldVelocity;

        newVelocity += moveDirection * acceleration * delta;
        newVelocity.x = Mathf.Clamp(newVelocity.x, -maxSpeed.x, maxSpeed.x);
        newVelocity.y = Mathf.Clamp(newVelocity.y, -maxSpeed.y, maxSpeed.y);

        return newVelocity;
    }

    public virtual Vector2 GetMoveDirection()
    {
        return this.moveDirection;
//        return new Vector2(
//            1f,//Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
//            1f
//        );
    }
}