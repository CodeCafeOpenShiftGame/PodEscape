using Godot;
using Godot.Collections;
using System;

public class Dash : Move
{
    public Timer DashTimer;
    public Timer GhostTimer;
    [Export((PropertyHint)24, "17/17:PackedScene")]
    public PackedScene GhostScene;

    [Signal]
    public delegate void DashSignal();
    //public Player player;
    Tween dashTween;

    // Collision handling
    CollisionShape2D playerCollisionShape;
    CollisionShape2D slideCollisionShape;
    Boolean collisionDetected;
    Boolean dashStopped;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        this.DashTimer = this.GetNode<Timer>("DashTimer");
        this.GhostTimer = this.GetNode<Timer>("GhostTimer");
        this.player = (Player)this.Owner;
        this.dashTween = this.GetNode<Tween>("Tween");
        this.playerCollisionShape = this.GetParent().GetParent().GetNode("PlayerCollisionShape") as CollisionShape2D;
        this.slideCollisionShape = this.GetParent().GetParent().GetNode("SlideCollisionShape") as CollisionShape2D;
        this.collisionDetected = false;
        this.dashStopped = false;
    }

    public override void UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("dash"))
        {
            base.Velocity = this.CalculateDashVelocity(base.DashImpulse);
        }
        else if (@event.IsActionPressed("slow"))
        {
            dashTween.Stop(player, "position");
            dashStopped = true;
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

        // TODO: Epsilon check
        if (this.GetMoveDirection().x == 0f)
        {
            this.StateMachine.TransitionTo("Idle");

            return;
        }

        if (player.IsOnFloor())
        {
            // TODO: Epsilon check
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

        AnimationPlayer animationPlayer = player.GetNode("AnimationPlayer") as AnimationPlayer;

        if (msg.ContainsKey("impulse"))
        {
            this.Velocity += this.CalculateDashVelocity((float)msg["impulse"]);

            if (player.IsOnFloor())
            {
                animationPlayer.Play("Slide");
                this.playerCollisionShape.Disabled = true;
                this.slideCollisionShape.Disabled = false;
                player.PlayerTrail.Emitting = false;
            }
                else
            {
                animationPlayer.Play("Dash");
                base.isDashing = true;
            }

            if (GameManager.AudioOn)
            {
                AudioStreamPlayer audio = this.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
                audio.Play();
            }

            this.GhostTimer.Start();
            this.DashTimer.Start();

            this.HandleCollisions(msg);

            EmitSignal(nameof(DashSignal));
        }
    }

    private void HandleCollisions(Dictionary<string, object> msg = null)
    {
        foreach (Node2D node in GetTree().GetNodesInGroup("killingObstacles"))
            {
                if (node.GlobalPosition.x > player.Position.x && node.GlobalPosition.x < player.Position.x + ((float)msg["impulse"] / 2))
                {
                    if (player.IsOnFloor())
                    {
                        dashTween.InterpolateProperty
                        (
                            player,
                            "position",
                            player.Position,
                            new Vector2(node.GlobalPosition.x, player.Position.y),
                            .5f,
                            Tween.TransitionType.Linear,
                            Tween.EaseType.InOut
                        );
                        dashTween.Start();
                        collisionDetected = true;
                    }
                }
            }

        if (!collisionDetected)
        {
            dashTween.InterpolateProperty
            (
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
        // The dash was stopped before colliding
        if (dashStopped)
        {
            collisionDetected = false;
        }
        if (collisionDetected && !dashStopped)
        {
            this.StateMachine.TransitionTo("Die");
        }
        this.GhostTimer.Stop();
        if (playerCollisionShape.Disabled)
        {
            this.slideCollisionShape.Disabled = true;
            this.playerCollisionShape.Disabled = false;
        }
        player.PlayerTrail.Emitting = true;
        base.isDashing = false;
    }
}