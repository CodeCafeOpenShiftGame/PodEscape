using Godot;
using Godot.Collections;
using System;
using System.Text;

[Tool]
public class Air : Move
{
    public Timer JumpDelay;
    public Timer ControlsFreeze;
    //public Player player;

    public override void _Ready()
    {
        base._Ready();
        this.JumpDelay = this.GetNode("JumpDelay") as Timer;
        this.ControlsFreeze = this.GetNode("ControlsFreeze") as Timer;
        this.player = (Player)this.Owner;
    }

    public override void UnhandledInput(InputEvent @event)
    {
        // TODO: Epsilon check
        if (this.GetMoveDirection().x == 0f)
        {
            return;
        }

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
            string targetState = base.GetMoveDirection().x == 0 ? "Idle" : "Run";
            this.StateMachine.TransitionTo(targetState);
        }
    }

    public override void Enter(Dictionary<string, object> msg = null)
    {
        base.Enter(msg);

        // Disable the snap vector when Jumping
        this.SnapVector.y = 0;

        if (msg == null)
        {
            return;
        }

        AnimationPlayer animationPlayer = this.player.GetNode("AnimationPlayer") as AnimationPlayer;
        animationPlayer.Play("Jump");

        if (GameManager.AudioOn)
        {
            AudioStreamPlayer audio = this.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
            audio.Play();
        }

        if (msg.ContainsKey("velocity"))
        {
            this.Velocity = (Vector2)msg["velocity"];
            this.MaxSpeed.x = Math.Max(Math.Abs(this.Velocity.x), this.MaxSpeed.x);
        }

        if (msg.ContainsKey("impulse"))
        {
            this.Velocity += this.CalculateJumpVelocity((float)msg["impulse"]);
        }
    }

    public override void Exit()
    {
        this.Acceleration = this.AccelerationDefault;
        hasDashed = false;
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
