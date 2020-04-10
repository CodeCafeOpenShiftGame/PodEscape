using Godot;
using Godot.Collections;
using System;

public class Die : Move
{
    public override void Enter(Dictionary<string, object> msg = null)
    {
        base.Enter(msg);

        player.AnimationPlayer.Play("Fall");
        player.PlayerTrail.Emitting = false;

        base.moveDirection = Vector2.Zero;
    }

    public override void Exit()
    {
        GD.Print("Die::Exit()");
        base.Exit();

        //this.StateMachine.TransitionTo("[stop]");
    }

    public void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        GD.Print("Die::_on_AnimationPlayer_animation_finished() anim_name is " + anim_name);
        this.Exit();
    }
}
