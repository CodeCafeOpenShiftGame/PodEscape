using Godot;
using Godot.Collections;
using System;
using System.Text;

public class Idle : Move
{
    public Timer JumpDelay;

    public override void _Ready()
    {
//        GD.Print("Idle::_Ready()");
        base._Ready();
        this.JumpDelay = this.GetNode("JumpDelay") as Timer;
    }

    public override void PhysicsProcess(float delta)
    {
//        GD.Print("Idle::PhysicsProcess()");
        Player player = this.Owner as Player;

        // TODO: Epsilon check
        if (this.GetMoveDirection().x == 0f)
        {
            return;
        }

        // @TODO @FIXME Remove this hack
        if (this.player.IsDead)
        {
            return;
        }

        // TODO: Epsilon check
        if (player.IsOnFloor())
        {
            this.StateMachine.TransitionTo("Run");
        }
        else if (!player.IsOnFloor())
        {
            this.StateMachine.TransitionTo("Air");
        }

        //base.PhysicsProcess(delta);
    }

    public override void Enter(Dictionary<string, object> msg = null)
    {
//        GD.Print("Idle::Enter()");
        base.Enter(msg);

        Player player = this.Owner as Player;

        this.player.IsDead = false;

        player.AnimationPlayer.Play("Idle");

        this.MaxSpeed = this.MaxSpeedDefault;
        this.Velocity = Vector2.Zero;
        this.SnapVector.y = this.SnapDistance;

        if (this.JumpDelay.TimeLeft > 0f)
        {
            this.StateMachine.TransitionTo("Air");
        }
    }

    public override void Exit()
    {
//        GD.Print("Idle::Exit()");
        base.Exit();
    }

    public override String _GetConfigurationWarning()
    {
//        GD.Print("Idle::_GetConfigurationWarning()");
        StringBuilder warnings = new StringBuilder();

        if (this.JumpDelay == null)
        {
            warnings.Append(this.ToString() + " requires a Timer child named JumpDelay.\n");
        }

        return warnings.ToString();
    }
}
