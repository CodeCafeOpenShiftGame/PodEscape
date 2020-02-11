using Godot;
using Godot.Collections;
using System;

public class Idle : Move
{
    public override void PhysicsProcess(float delta)
    {
        Player player = (Player)this.Owner;

        if (player.IsOnFloor() && GetMoveDirection().x != 0) {
            this.StateMachine.TransitionTo("Move/Run");
        }
        else if (!player.IsOnFloor()) {
            this.StateMachine.TransitionTo("Move/Air");
        }
        else {
            base.PhysicsProcess(delta);
        }
    }

    public override void Enter(Dictionary msg = null)
    {
        Player player = (Player)this.Owner;
        AnimationPlayer animationPlayer = (AnimationPlayer)player.GetNode("AnimationPlayer");
        animationPlayer.Play("Idle");
    }

}
