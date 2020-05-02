using Godot;
using Godot.Collections;
using System;

public class Die : Move
{

    [Signal]
    public delegate void DieSignal();
    private Timer dieTimer;

    public override void Enter(Dictionary<string, object> msg = null)
    {
        base.Enter(msg);

        this.dieTimer = this.GetNode<Timer>("DieTimer");

        if (GameManager.AudioOn)
        {
            AudioStreamPlayer dieSound = this.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
            dieSound.Play();
        }
        
        this.player.IsDead = true;
        player.AnimationPlayer.Play("Fall");
        dieTimer.Start(1.5f);
        player.PlayerTrail.Emitting = false;

        EmitSignal(nameof(DieSignal));

        base.moveDirection = Vector2.Zero;
        Velocity = Vector2.Zero;
    }

    public override void Exit()
    {
        GD.Print("Die::Exit()");
        moveDirection = Vector2.Zero;
        Velocity = Vector2.Zero;
        base.Exit();
    }

    // public void _on_AnimationPlayer_animation_finished(String anim_name)
    // {
    //     GD.Print("Die::_on_AnimationPlayer_animation_finished() anim_name is " + anim_name);
    //     this.Exit();
    // }

    public void _on_DieTimer_timeout()
    {
        GD.Print("Die::_on_DieTimer_timeout()");
        this.Exit();
    }
}
