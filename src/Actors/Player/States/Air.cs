using Godot;
using Godot.Collections;
using System;
using System.Text;

[Tool]
public class Air : Move
{
    [Export]
    public float HorizontalAcceleration = 5000f;

    public Timer JumpDelay;
    public Timer ControlsFreeze;

    public override void _Ready()
    {
        base._Ready();
        this.JumpDelay = this.GetNode("JumpDelay") as Timer;
        this.ControlsFreeze = this.GetNode("ControlsFreeze") as Timer;
    }

    public override void UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("jump"))
        {
            if (base.Velocity.y >= 0.0 && this.JumpDelay.TimeLeft > 0f)
            {
                base.Velocity = this.CalculateJumpVelocity(base.JumpImpulse);
            }
            // @TODO Emmit signal "Jumped"
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
            string targetState = base.GetMoveDirection().x == 0 ? "Move/Idle" : "Move/Run";
            this.StateMachine.TransitionTo(targetState);
        }
    }

    public override void Enter(Dictionary msg = null)
    {
        base.Enter(msg);

        this.Acceleration.x = this.HorizontalAcceleration;
        this.SnapVector.y = 0;

        if (msg != null)
        {
            return;
        }

        // @TODO Play the Animation when Falling or Jumping

        // @TODO @FIXME How to get the values from dictionary?
        // if (msg.Contains("velocity"))
        // {
        //     this.Velocity = (Vector2)msg["velocity"];
        //     this.MaxSpeed.x = Math.Max(Math.Abs(this.Velocity.x), this.MaxSpeed.x);
        // }
        // if (msg.Contains("impulse"))
        // {
        //     this.Velocity += this.CalculateJumpVelocity((float)msg["impulse"]);
        // }
    }

    public override void Exit()
    {
        this.Acceleration = this.AccelerationDefault;
        base.Exit();
    }

    public virtual Vector2 CalculateJumpVelocity(float impulse = 0f)
    {
	    return base.CalculateVelocity(
            base.Velocity,
            base.MaxSpeed,
            new Vector2(0f, impulse),
            1f,
            Vector2.Up
        );
    }

    public override String _GetConfigurationWarning()
    {
        StringBuilder warnings = new StringBuilder();

        if (this.JumpDelay == null)
        {
            warnings.Append(this.ToString() + " requires a Timer child named JumpDelay.\n");
        }

        return warnings.ToString();
    }
}
