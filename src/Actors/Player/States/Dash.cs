using Godot;
using Godot.Collections;
using System;

public class Dash : Move
{
    public Timer DashTimer;
    public Timer GhostTimer;
    [Export((PropertyHint)24, "17/17:PackedScene")]
    public PackedScene GhostScene;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        this.DashTimer = this.GetNode<Timer>("DashTimer");
        this.GhostTimer = this.GetNode<Timer>("GhostTimer");
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

        if (this.DashTimer.TimeLeft > 0)
        {
            return;
        }

        Player player = this.Owner as Player;

        if (player.IsOnFloor())
        {
            string targetState = base.GetMoveDirection().x == 0 ? "Idle" : "Run";
            this.StateMachine.TransitionTo(targetState);
        }
        else if (!player.IsOnFloor())
        {
            this.StateMachine.TransitionTo("Air");
        }

    }

    public override void Enter(Dictionary<string, object> msg = null)
    {
        base.Enter(msg);

        if (msg == null)
            return;

        Player player = (Player)this.Owner;
        AnimationPlayer animationPlayer = player.GetNode("AnimationPlayer") as AnimationPlayer;
        animationPlayer.Play("Dash");

        this.GhostTimer.Start();
        this.DashTimer.Start();

        if (msg.ContainsKey("velocity"))
        {
            this.Velocity = (Vector2)msg["velocity"];
            this.MaxSpeed.x = Math.Max(Math.Abs(this.Velocity.x), this.MaxSpeed.x);
        }

        if (msg.ContainsKey("impulse"))
        {
            this.Velocity += this.CalculateDashVelocity((float)msg["impulse"]);

            Tween dashTween = this.GetNode("Tween") as Tween;
            dashTween.InterpolateProperty(
                player,
                "position",
                player.Position,
                new Vector2(player.Position.x + ((float)msg["impulse"] / 2), player.Position.y),
                .5f,
                Tween.TransitionType.Linear,
                Tween.EaseType.InOut
            );
            dashTween.Start();
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

    public virtual void _OnGhostTimerTimeout()
    {
        Player player = this.Owner as Player;
        Sprite playerSprite = player.GetNode("BodyPivot/Sprite") as Sprite;
        Sprite ghost = this.GhostScene.Instance() as Sprite;
        ghost.Scale = playerSprite.Scale;
        ghost.Texture = playerSprite.Texture;
        ghost.Position = player.Position;
        this.GetParent().AddChild(ghost);
    }

    public virtual void _OnDashTimerTimeout()
    {
        this.GhostTimer.Stop();
    }
}