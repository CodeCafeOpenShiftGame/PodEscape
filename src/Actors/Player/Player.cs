using Godot;
using System;

public class Player : Actor
{
    [Signal]
    delegate void PlayerDied(string howPlayerDied);

    private GameManager gameManager;
    public StateMachine StateMachine;
    public AnimationPlayer AnimationPlayer;
    public Vector2 LookDirection = new Vector2(1, 1);
    public CPUParticles2D PlayerTrail;
    public CPUParticles2D PlayerBurst;

    public override void _Ready()
    {
        GD.Print("Player::_Ready()");
        GD.Print(this.GetPath());
        this.StateMachine = this.GetNode("StateMachine") as StateMachine;
        this.AnimationPlayer = this.GetNode("AnimationPlayer") as AnimationPlayer;
        this.PlayerTrail = this.GetNode("PlayerTrail") as CPUParticles2D;
        this.PlayerBurst = this.GetNode("PlayerBurst") as CPUParticles2D;
        this.gameManager = GetNode<GameManager>("/root/GameManager");

        this.gameManager.Connect("GracePeriodExpired", this, "_on_GracePeriodExpired");
        this.Connect("PlayerDied", this.gameManager, "_on_PlayerDied");
    }

    private void _on_GracePeriodExpired()
    {
        GD.Print("Player::_on_GracePeriodExpired()");
        this.StateMachine.TransitionTo("Die");
    }

    public void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        GD.Print("Player::_on_AnimationPlayer_animation_finished() anim_name is " + anim_name);

        if (0 == this.gameManager.GracePeriod)
        {
            EmitSignal(nameof(PlayerDied), "expired");
        }
        else if ("Fall" == anim_name)
        {
            EmitSignal(nameof(PlayerDied), "collision");
        }
    }

}
