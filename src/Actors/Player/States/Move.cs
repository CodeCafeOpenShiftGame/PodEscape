using Godot;
using Godot.Collections;
using System;

abstract public class Move : State
{
    protected Vector2 moveDirection = Vector2.One;

    [Export]
    public Vector2 MaxSpeedDefault = new Vector2(500f, 1500f);
    [Export]
    public float MinSpeedX = 0f;
    [Export]
    public Vector2 AccelerationDefault = new Vector2(100000f, 3000f);
    [Export]
    public Vector2 SlowAccelerationDefault = new Vector2(-1000f, -10f); // -1000f, 3000f for normal slow jumping
    [Export]
    public float JumpImpulse = 1350f;
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

    private float PreviousPosition;
    private float NewPosition;

    public override void _Ready()
    {
        base._Ready();
        this.Acceleration = this.AccelerationDefault;
        this.MaxSpeed = this.MaxSpeedDefault;
        this.isDashing = false;
        this.PreviousPosition = 0;
        this.NewPosition = 0;
    }

    public override void UnhandledInput(InputEvent @event)
    {
        Player player = (Player)this.Owner;

        // TODO: Epsilon check
        if (this.GetMoveDirection().x == 0f)
        {
            // GD.Print("move direction x == 0");
            return;
        }

        if (this.player.IsDead)
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

        if (@event.IsActionPressed("dash"))
        {
            if (player.IsOnFloor())
            {
                StartDash();
            }
            else if (!player.IsOnFloor() && !this.hasDashed)
            {
                StartDash();
                hasDashed = true;
            }
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
        Player player = (Player)this.Owner;

        // @TODO @FIXME Remove this hack
        if (this.player.IsDead)
        {
            return;
        }

        //Calculate score by distance
        this.NewPosition = player.GlobalPosition.x;

        if (this.NewPosition > this.PreviousPosition)
        {
            GameManager.Score += (int) this.NewPosition / 1000;
            this.PreviousPosition = this.NewPosition;
        }

        // give the slow a "break" type feel - for the gamepads, key events get this by default
        if (this.Velocity.x <= 0f)
        {
//            GD.Print("disengage break, move velocity < 0");
            this.Velocity.x = this.MinSpeedX;
            this.Acceleration = this.AccelerationDefault;
        }

        Vector2 direction = this.GetMoveDirection();

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
        // @TODO @FIXME Remove this hack to prevent Die To Die transitions
        if (this.moveDirection.x <= float.Epsilon)
        {
            return;
        }

        var collisionInfo = player.MoveAndCollide(this.Velocity * delta, true, true, true);

        if (collisionInfo != null && !isDashing)
        {
            foreach (Node2D node in GetTree().GetNodesInGroup("killingObstacles"))
            {
                if (collisionInfo.ColliderId == node.GetInstanceId())
                {
                    GD.Print("Move::HandleCollisions transitioning to Die 1");
                    this.StateMachine.TransitionTo("Die");
                }
            }
            foreach (Node2D node in GetTree().GetNodesInGroup("dashable"))
            {
                if (collisionInfo.ColliderId == node.GetInstanceId())
                {
                    // @TODO @FIXME fix Die to Die loop
                    //GD.Print("Move::HandleCollisions transitioning to Die 2");
                    this.StateMachine.TransitionTo("Die");
                }
            }
        }

        if (!this.player.IsOnFloor() &&
            this.player.Position.y > 1200.0f)
        {
            this.StateMachine.TransitionTo("Die");
            this.moveDirection = Vector2.Zero;
            this.Velocity = Vector2.Zero;
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
        newVelocity.x = Mathf.Clamp(newVelocity.x, MinSpeedX, maxSpeed.x);
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


    public override void Enter(Dictionary<string, object> msg = null)
    {
//        GD.Print("Move::Enter()");
        base.Enter(msg);
    }

    public override void Exit()
    {
//        GD.Print("Move::Exit()");
        base.Exit();
    }
}