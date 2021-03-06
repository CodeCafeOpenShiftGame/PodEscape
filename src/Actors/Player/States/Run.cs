using Godot;
using Godot.Collections;
using System;

public class Run : Move
{
    public Timer SlowStarter;
    public Tween Tween;
    public float SlowDurationSeconds = 0.4f;
    //public Player player;

    public override void _Ready()
    {
        base._Ready();
        this.SlowStarter = this.GetNode("SlowStarter") as Timer;
        this.SlowStarter.Connect("timeout", this, "OnSlowDownTimeout");
        this.Tween = this.GetNode("Tween") as Tween;
        this.player = (Player)this.Owner;
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
//        GD.Print("Run::Enter()");
        base.Enter(msg);

        this.player.AnimationPlayer.Play("Run");

        if (Mathf.IsEqualApprox(this.MaxSpeed.x, this.MaxSpeedDefault.x))
        {
            this.SlowStarter.Start();
        }

        moveDirection = Vector2.One;
    }

    public override void Exit()
    {
//        GD.Print("Run::Exit()");
        base.Exit();
    }
}
