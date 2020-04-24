using Godot;
using Godot.Collections;
using System;

public class Die : Move
{

    [Signal]
    public delegate void DieSignal();

    public override void Enter(Dictionary<string, object> msg = null)
    {
        base.Enter(msg);

        player.AnimationPlayer.Play("Fall");
        player.PlayerTrail.Emitting = false;
        EmitSignal(nameof(DieSignal));

        base.moveDirection = Vector2.Zero;
        Velocity = Vector2.Zero;
    }

    public override void Exit()
    {
//        GD.Print("Die::Exit()");
        moveDirection = Vector2.Zero;
        Velocity = Vector2.Zero;
        base.Exit();
    }

    public void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        GD.Print("Die::_on_AnimationPlayer_animation_finished() anim_name is " + anim_name);
        this.Exit();
    }
}
