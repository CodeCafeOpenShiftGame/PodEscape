using Godot;
using Godot.Collections;
using System;

public class Run : Move
{
    public Timer SlowStarter;
    public Tween Tween;
    public float SlowDurationSeconds = 0.4f;
    public Player player;
    public CollisionPolygon2D runDashPolygon2D;
    public CollisionPolygon2D jumpPolygon2D;
    public CollisionPolygon2D slidePolygon2D;

    public override void _Ready()
    {
        base._Ready();
        this.SlowStarter = this.GetNode("SlowStarter") as Timer;
        this.SlowStarter.Connect("timeout", this, "OnSlowDownTimeout");
        this.Tween = this.GetNode("Tween") as Tween;
        this.player = (Player)this.Owner;
        this.runDashPolygon2D = player.GetNode<CollisionPolygon2D>("RunDashPolygon2D");
        this.jumpPolygon2D = player.GetNode<CollisionPolygon2D>("JumpPolygon2D");
        this.slidePolygon2D = player.GetNode<CollisionPolygon2D>("SlidePolygon2D");
    }

    public virtual void OnSlowDownTimeout()
    {
        // @TODO Slow down when key is released
    }

    public override void PhysicsProcess(float delta)
    {
        Player player = this.Owner as Player;

        // TODO: Epsilon check
        if (this.GetMoveDirection().x == 0f)
        {
            this.StateMachine.TransitionTo("Idle");

            return;
        }

        if (!player.IsOnFloor())
        {
            this.StateMachine.TransitionTo("Air");
        }

        base.PhysicsProcess(delta);
    }

    public override void Enter(Dictionary<string, object> msg = null)
    {
        base.Enter(msg);

        this.player.AnimationPlayer.Play("Run");
        this.runDashPolygon2D.Disabled = false;
        this.jumpPolygon2D.Disabled = true;
        this.slidePolygon2D.Disabled = true;

        if (Mathf.IsEqualApprox(this.MaxSpeed.x, this.MaxSpeedDefault.x))
        {
            this.SlowStarter.Start();
        }
    }
}
